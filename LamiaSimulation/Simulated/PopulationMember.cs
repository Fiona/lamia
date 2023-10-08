using System;
using System.Collections.Generic;

namespace LamiaSimulation
{
    [Serializable]
    internal class PopulationMember: SimulationObject, IActionReceiver, IQueryable, ISimulated
    {
        public PopulationSpeciesType species;
        public string state;
        public string name;
        public string taskAssigment;
        public string currentAction;
        public float hunger;

        private string settlementUuid;
        private string currentLocationUuid;
        private float timeToCompleteCurrentAction;
        private float currentActionProgress;
        private Dictionary<ResourceType, float> inventory;
        private float maxInventory;
        private string waitMessage = "";
        private float deathTimer = 0f;

        public PopulationMember(string speciesID, string settlementUuid, string settlementLocationUuid)
        {
            inventory = new Dictionary<ResourceType, float>();
            var species = DataQuery<PopulationSpeciesType>.GetByID(speciesID);
            this.species = species ?? throw new ClientActionException(T._("Species does not exist."));
            this.settlementUuid = settlementUuid;
            currentLocationUuid = settlementLocationUuid;
            name = NameGenerator.Instance.GenerateFullName();
            maxInventory = species.maxInventory;
            hunger = 1.0f;
            taskAssigment = "idle";
            currentAction = "idle";
            state = "task";
        }

        // ---------------------------------------------------
        // IActionReceiver
        // ---------------------------------------------------

        public void PerformAction(ClientAction action)
        {
        }

        public void PerformAction<T>(ClientAction action, ClientParameter<T> param1)
        {
        }

        public void PerformAction<T1, T2>(ClientAction action, ClientParameter<T1> param1, ClientParameter<T2> param2)
        {
        }

        public void PerformAction<T1, T2, T3>(ClientAction action, ClientParameter<T1> param1, ClientParameter<T2> param2,
            ClientParameter<T3> param3)
        {
            if(param2.Get as string != ID)
                return;
            switch(action)
            {
                case ClientAction.PopulationAssignToTask:
                    AssignToTask(param3.Get as string);
                    break;
            }
        }

        // ---------------------------------------------------
        // IQueryable
        // ---------------------------------------------------

        public void Query<T>(ref QueryResult<T> result, ClientQuery query)
        {
        }

        public void Query<T, T1>(ref QueryResult<T> result, ClientQuery query, ClientParameter<T1> param1)
        {
        }

        public void Query<T, T1, T2>(ref QueryResult<T> result, ClientQuery query, ClientParameter<T1> param1,
            ClientParameter<T2> param2)
        {
            if(param2.Get as string != ID)
                return;
            switch(query)
            {
                case ClientQuery.PopulationMemberCurrentAction:
                    result = new QueryResult<string>(currentAction) as QueryResult<T>;
                    break;
                case ClientQuery.PopulationMemberCurrentActionProgress:
                    result = new QueryResult<float>(CurrentActionProgress()) as QueryResult<T>;
                    break;
                case ClientQuery.PopulationMemberCurrentActionName:
                    result = new QueryResult<string>(CurrentActionName()) as QueryResult<T>;
                    break;
                case ClientQuery.PopulationMemberState:
                    result = new QueryResult<string>(state) as QueryResult<T>;
                    break;
                case ClientQuery.PopulationMemberWaitMessage:
                    result = new QueryResult<string>(waitMessage) as QueryResult<T>;
                    break;
                case ClientQuery.PopulationMemberInventoryProgress:
                    result = new QueryResult<float>(CurrentInventoryProgress()) as QueryResult<T>;
                    break;
                case ClientQuery.PopulationMemberHunger:
                    result = new QueryResult<float>(hunger) as QueryResult<T>;
                    break;
            }
        }

        public void Query<T, T1, T2, T3>(ref QueryResult<T> result, ClientQuery query, ClientParameter<T1> param1,
            ClientParameter<T2> param2, ClientParameter<T3> param3)
        {
        }

        // ---------------------------------------------------
        // ISimulated
        // ---------------------------------------------------

        public void Simulate(float deltaTime)
        {
            var starvingState = DetermineStarvingState();
            if (starvingState.isStarving)
            {
                if (state != "starving" && !starvingState.foodAvailable)
                {
                    deathTimer = Consts.populationStarvationDeathTime;
                    state = "starving";
                }
                state = starvingState.foodAvailable ? "eating" : "starving";
                currentAction = "eating";
            }
            var waitSate = DetermineWaitState();
            if (waitSate.doWait)
            {
                timeToCompleteCurrentAction = 0f;
                currentActionProgress = 0f;
                state = "wait";
                waitMessage = waitSate.waitMessage;
            }
            else
            {
                if(state == "wait")
                    StartNewAction();
            }

            switch (state)
            {
                case "starving":
                    deathTimer -= deltaTime;
                    if (deathTimer <= 0f)
                    {
                        Simulation.Instance.PerformAction(
                            ClientAction.SendMessage,
                            new ClientParameter<string>(
                                string.Format(T._("{0} has starved to death!"), name)
                            )
                        );
                        Simulation.Instance.PerformAction(
                            ClientAction.SettlementRemovePopulation,
                            new ClientParameter<string>(settlementUuid),
                            new ClientParameter<string>(ID)
                        );
                    }
                    break;
                case "task":
                case "eating":
                    PerformCurrentAction(deltaTime);
                    break;
            }
        }

        private void AssignToTask(string newTaskId)
        {
            if(taskAssigment == newTaskId)
                throw new ClientActionException(T._("Population member already assigned to task."));
            taskAssigment = newTaskId;
            currentActionProgress = 0f;
            StartNewAction();
        }

        private void PerformCurrentAction(float deltaTime)
        {
            if (currentAction == "idle")
                return;
            currentActionProgress += deltaTime;
            if (currentActionProgress >= timeToCompleteCurrentAction)
            {
                currentActionProgress = 0f;
                CompleteCurrentAction();
                StartNewAction();
            }
        }

        private void CompleteCurrentAction()
        {
            switch (currentAction)
            {
                case "deposit":
                    foreach (var inventoryItem in inventory)
                    {
                        Simulation.Instance.PerformAction(
                            ClientAction.AddResourceToSettlementInventory,
                            new ClientParameter<string>(settlementUuid),
                            new ClientParameter<string>(inventoryItem.Key.ID),
                            new ClientParameter<float>(inventoryItem.Value)
                        );
                        inventory[inventoryItem.Key] = 0f;
                    }
                    hunger -= Consts.depositInventoryHungerReduction;
                    break;
                case "extract":
                    var task = Helpers.GetTaskTypeById(taskAssigment);
                    if (task.behaviour != TaskTypeBehaviour.EXTRACT)
                        return;
                    var resource = Helpers.GetResourceTypeById(task.extractResourceType);
                    var locationResource = Simulation.Instance.Query<float, string, string>(
                        ClientQuery.LocationResourceAmount, currentLocationUuid, task.extractResourceType
                    );
                    var amountToExtract = Math.Min(locationResource, task.amount);
                    if (amountToExtract <= 0f)
                        break;
                    Simulation.Instance.PerformAction(
                        ClientAction.SubtractResourceFromLocation,
                        new ClientParameter<string>(currentLocationUuid), 
                        new ClientParameter<string>(task.extractResourceType), 
                        new ClientParameter<float>(amountToExtract)
                    );
                    inventory.TryAdd(resource, 0f);
                    inventory[resource] += task.amount;
                    hunger -= task.hungerReduction;
                    break;
                case "eating":
                    var availableFood = Simulation.Instance.Query<float, string>(
                        ClientQuery.SettlementAvailableFoodPortion, settlementUuid
                    );
                    if (availableFood > 0f)
                    {
                        Simulation.Instance.PerformAction(
                            ClientAction.SettlementTakeAvailableFoodPortion, new ClientParameter<string>(settlementUuid)
                            );
                        hunger += availableFood;
                        hunger = Math.Min(hunger, 1f);
                    }
                    break;
            }

            hunger = Math.Max(0f, hunger);
        }

        private void StartNewAction()
        {
            if (state == "eating")
            {
                var availableFood = Simulation.Instance.Query<float, string>(
                    ClientQuery.SettlementAvailableFoodPortion, settlementUuid
                );
                if (availableFood > 0f && hunger < 1f)
                {
                    currentAction = "eating";
                    timeToCompleteCurrentAction = Consts.populationEatingTime;
                    return;
                }
            }
            state = "task";
            if (InventoryFull())
            {
                currentAction = "deposit";
                timeToCompleteCurrentAction = Consts.depositInventoryTime;
                return;
            }
            switch (taskAssigment)
            {
                case "idle":
                    currentAction = "idle";
                    timeToCompleteCurrentAction = 0.0f;
                    break;
                case "forage":
                case "cut_trees":
                    currentAction = "extract";
                    timeToCompleteCurrentAction =  Helpers.GetTaskTypeById(taskAssigment).timeToComplete;
                    break;
                default:
                    throw new ClientActionException(T._("Current task not supported properly.")); 
            }
        }

        private (bool isStarving, bool foodAvailable) DetermineStarvingState()
        {
            if (hunger > 0f)
                return (false, false);
            var availableFood = Simulation.Instance.Query<float, string>(
                ClientQuery.SettlementAvailableFoodPortion, settlementUuid
            );
            return (true, availableFood > 0f);
        }
        
        private (bool doWait, string waitMessage) DetermineWaitState()
        {
            if (state == "starving" || state == "eating")
                return (false, "");
            if (currentAction == "extract")
            {
                var task = Helpers.GetTaskTypeById(taskAssigment);
                if (task.behaviour != TaskTypeBehaviour.EXTRACT)
                    return (false, "");
                var locationResourceAmount = Simulation.Instance.Query<float, string, string>(
                    ClientQuery.LocationResourceAmount, currentLocationUuid, task.extractResourceType
                );
                if (locationResourceAmount == 0f)
                    return (true, T._("No resource remaining at location.")); 
                var settlementResourceAmount = Simulation.Instance.Query<float, string, string>(
                    ClientQuery.SettlementInventoryResourceAmount, settlementUuid, task.extractResourceType
                );
                var settlementResourceCapacity = Simulation.Instance.Query<float, string, string>(
                    ClientQuery.SettlementInventoryResourceCapacity, settlementUuid, task.extractResourceType
                );
                if (settlementResourceAmount >= settlementResourceCapacity)
                    return (true, T._("No space left to store resource.")); 
            }

            return (false, "");
        }
        
        private string CurrentActionName()
        {
            switch (state)
            {
                case "wait":
                    return T._("Waiting");
                case "starving":
                    return T._("Starving!!!");
                case "eating":
                    return T._("Eating");
            }

            switch (currentAction)
            {
                case "idle":
                    return ". . .";
                case "deposit":
                    return T._("Depositing");
                case "extract":
                    switch (taskAssigment)
                    {
                        case "forage":
                            return T._("Foraging");
                        case "cut_trees":
                            return T._("Cutting Trees");
                        default:
                            return T._("Extracting");
                    }
            }

            return currentAction;
        }
        
        private float CurrentActionProgress()
        {
            if (currentAction == "idle")
                return 0.0f;
            return currentActionProgress / timeToCompleteCurrentAction;
        }

        private float CurrentInventoryProgress()
        {
            var currentAmount = 0f;
            foreach (var inventoryItem in inventory)
                currentAmount += inventory[inventoryItem.Key] * inventoryItem.Key.weight;
            return Math.Min(1f, currentAmount / maxInventory);
        }
        
        private bool InventoryFull()
        {
            return Math.Abs(CurrentInventoryProgress() - 1f) < .01f;
        }
        
    }
}