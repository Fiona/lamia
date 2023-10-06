namespace LamiaSimulation
{
    public enum ClientQuery
    {
        /*
         * Available game pages
         *   Result - (string, string)[]: page id and display name
         */
        AvailablePages,

        /*
         * Unread messages to show the player
         *   Result - string[]: Messages to display
         */
        UnreadMessages,

        // --------------------------------------------------------------------
        // SETTLEMENTS
        // --------------------------------------------------------------------

        /*
         * All UUIDs of player settlements
         *   Result - string[]: UUIDs
         */
        Settlements,

        /*
         * Displayable name of the settlement
         *   Result - string
         *   Params:
         *     - string: uuid of settlement to query
         */
        SettlementName,

        /*
         * Population count of a settlement
         *   Result - int
         *   Params:
         *     - string: uuid of settlement to query
         */
        SettlementCurrentPopulation,

        /*
         * Maximum population count of a settlement
         *   Result - int
         *   Params:
         *     - string: uuid of settlement to query
         */
        SettlementPopulationMax,

        /*
         * IDs of all population members in a settlement
         *   Result - string[]: uuids of pop members
         *   Params:
         *     - string: uuid of settlement to query
         */
        SettlementPopulationMembers,

        /*
         * IDs of all species represented in settlement population
         *   Result - string[]: IDs of species
         *   Params:
         *     - string: uuid of settlement to query
         */
        SettlementPopulationSpecies,

        /*
         * IDs of all members of a particular species in settlement
         *   Result - string[]: uuids of pop members
         *   Params:
         *     - string: uuid of settlement to query
         *     - string: ID of species to query
         */
        SettlementPopulationSpeciesMembers,
        

        // --------------------------------------------------------------------
        // POPULATION
        // --------------------------------------------------------------------
        /*
         * Displayable name of a population member
         *   Result - string
         *   Params:
         *     - string: uuid of settlement to query
         *     - string: uuid of population to query
         */
        PopulationMemberName,

        /*
         * Species of a population member
         *   Result - string: ID of species
         *   Params:
         *     - string: uuid of settlement to query
         *     - string: uuid of population to query
         */
        PopulationMemberSpecies,

        /*
         * Task assigned to a population member
         *   Result - string: ID of assigned task
         *   Params:
         *     - string: uuid of settlement to query
         *     - string: uuid of population to query
         */
        PopulationMemberTask,

        /*
         * Short name for the action the population is doing
         *   Result - string: Action short name
         *   Params:
         *     - string: uuid of settlement to query
         *     - string: uuid of population to query
         */
        PopulationMemberCurrentAction,
        
        /*
         * Progress of the current action that the population is doing
         *   Result - float: Number between 0 and 1 representing the action progress
         *   Params:
         *     - string: uuid of settlement to query
         *     - string: uuid of population to query
         */
        PopulationMemberCurrentActionProgress,

        /*
         * The display name for the current action that the pop member is doing right now
         *   Result - float: Number between 0 and 1 representing the action progress
         *   Params:
         *     - string: uuid of settlement to query
         *     - string: uuid of population to query
         */
        PopulationMemberCurrentActionName,
        
        
        // --------------------------------------------------------------------
        // SPECIES
        // --------------------------------------------------------------------
        /*
         * Displayable name of a population species
         *   Result - string
         *   Params:
         *     - string: ID of species
         */
        SpeciesName,

        /*
         * Displayable description of a population species
         *   Result - string[]: Lines of description
         *   Params:
         *     - string: ID of species
         */
        SpeciesDescription,

        // --------------------------------------------------------------------
        // TASKS
        // --------------------------------------------------------------------

        /*
         * Available population tasks
         *   Result - string[]: ID of tasks
         *   Params:
         *     - string: uuid of settlement to query
         */
        SettlementTasks,

        /*
         * Ids of population members assigned to a task
         *   Result -  string[]: UUIDs of population members assigned to a particular task
         *   Params:
         *     - string: uuid of settlement to query
         *     - string: id of task
         */
        SettlementTaskAssignments,

        /*
         * Display name of task
         *   Result - string: Displayable name for task
         *   Params:
         *     - string: uuid of settlement to query
         *     - string: id of task
         */
        SettlementTaskName,

        /*
         * Description of task
         *   Result - string[]: Lines of description
         *   Params:
         *     - string: uuid of settlement to query
         *     - string: id of task
         */
        SettlementTaskDescription,

        /*
         * Number of population assigned to a task
         *   Result - int: Num of population assigned
         *   Params:
         *     - string: uuid of settlement to query
         *     - string: id of task
         */
        SettlementTaskAssignedNum,

        /*
         * How many populations are allowed to be assigned to a task
         *   Result - int: Capacity for task querying. -1 indicates infinite capacity.
         *   Params:
         *     - string: uuid of settlement to query
         *     - string: id of task
         */
        SettlementTaskMaximumCapacity,

        /*
         * All resources that are being stored in a settlement's inventory
         *   Result - string[]: Resource IDs being stored
         *   Params:
         *     - string: uuid of settlement to query
         */
        SettlementInventory,

        /*
         * Amount of a particular resource that is being stored in a settlement 
         *   Result - float: Amount of resource being stored
         *   Params:
         *     - string: uuid of settlement to query
         *     - string: resource ID to query
         */
        SettlementInventoryResourceAmount,
        
        /*
         * Maximum amount of a resource that can be stored in a settlement
         *   Result - float: Maximum amount of the resource can be stored
         *   Params:
         *     - string: uuid of settlement to query
         *     - string: resource ID to query
         */
        SettlementInventoryResourceMax,

    }
}