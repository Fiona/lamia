﻿using System.Collections.Generic;
using System.Linq;
using Godot;
using LamiaSimulation;

/*
 * Thin wrapper around all ClientQuery calls as you (understandably) cannot call generic C# methods from GDScript. 
 */
public partial class Query: Node
{
    public override void _Ready()
    {
    }
    
    public Godot.Collections.Array<Godot.Collections.Array<string> > AvailablePages()
    {
        var pages =  Simulation.Instance.Query<(string, string)[]>(ClientQuery.AvailablePages);
        var pageReturn = new Godot.Collections.Array<Godot.Collections.Array<string> >();
        foreach (var pageTuple in pages)
            pageReturn.Add(
                new Godot.Collections.Array<string>(new [] { pageTuple.Item1, pageTuple.Item2 })
                );
        return pageReturn;
    }

    public string[] UnreadMessages()
    {
        return Simulation.Instance.Query<string[]>(ClientQuery.UnreadMessages);
    }

    // ----------------------
    // RESOURCES
    // ----------------------
    
    public string ResourceName(string resourceId)
    {
        return Simulation.Instance.Query<string, string>(ClientQuery.ResourceName, resourceId);
    }
    
    public string ResourceDescription(string resourceId)
    {
        return Simulation.Instance.Query<string, string>(ClientQuery.ResourceDescription, resourceId);
    }
    
    // ----------------------
    // LOCATIONS
    // ----------------------

    public string[] Locations()
    {
        return Simulation.Instance.Query<string[]>(ClientQuery.Locations);
    }
    
    public string[] LocationResources(string locationUuid)
    {
        return Simulation.Instance.Query<string[], string>(ClientQuery.LocationResources, locationUuid);
    }

    public float LocationResourceAmount(string locationUuid, string resourceId)
    {
        return Simulation.Instance.Query<float, string, string>(ClientQuery.LocationResourceAmount, locationUuid, resourceId );
    }

    // ----------------------
    // SETTLEMENTS
    // ----------------------
    
    public string[] Settlements()
    {
        return Simulation.Instance.Query<string[]>(ClientQuery.Settlements);
    }

    public string SettlementLocation(string uuid)
    {
        return Simulation.Instance.Query<string, string>(ClientQuery.SettlementLocation, uuid);
    }
    
    public string SettlementName(string uuid)
    {
        return Simulation.Instance.Query<string, string>(ClientQuery.SettlementName, uuid);
    }
    
    public int SettlementCurrentPopulation(string uuid)
    {
        return Simulation.Instance.Query<int, string>(ClientQuery.SettlementCurrentPopulation, uuid);
    }

    public int SettlementPopulationMax(string uuid)
    {
        return Simulation.Instance.Query<int, string>(ClientQuery.SettlementPopulationMax, uuid);
    }
    
    public string[] SettlementPopulationMembers(string uuid)
    {
        return Simulation.Instance.Query<string[], string>(ClientQuery.SettlementPopulationMembers, uuid);
    }
    
    public string[] SettlementPopulationSpecies(string uuid)
    {
        return Simulation.Instance.Query<string[], string>(ClientQuery.SettlementPopulationSpecies, uuid);
    }

    public string[] SettlementPopulationSpeciesMembers(string uuid, string speciesId)
    {
        return Simulation.Instance.Query<string[], string, string>(ClientQuery.SettlementPopulationSpeciesMembers, uuid, speciesId);
    }

    public string[] SettlementInventory(string uuid)
    {
        return Simulation.Instance.Query<string[], string>(ClientQuery.SettlementInventory, uuid);
    }

    public float SettlementInventoryResourceAmount(string uuid, string resourceId)
    {
        return Simulation.Instance.Query<float, string, string>(ClientQuery.SettlementInventoryResourceAmount, uuid, resourceId);
    }
    
    public float SettlementInventoryResourceMax(string uuid, string resourceId)
    {
        return Simulation.Instance.Query<float, string, string>(ClientQuery.SettlementInventoryResourceMax, uuid, resourceId);
    }
    
    // ----------------------
    // POPULATION
    // ----------------------
    
    public string PopulationMemberName(string settlementUuid, string populationUuid)
    {
        return Simulation.Instance.Query<string, string, string>(ClientQuery.PopulationMemberName, settlementUuid, populationUuid);
    }

    public string PopulationMemberSpecies(string settlementUuid, string populationUuid)
    {
        return Simulation.Instance.Query<string, string, string>(ClientQuery.PopulationMemberSpecies, settlementUuid, populationUuid);
    }

    public string PopulationMemberTask(string settlementUuid, string populationUuid)
    {
        return Simulation.Instance.Query<string, string, string>(ClientQuery.PopulationMemberTask, settlementUuid, populationUuid);
    }

    public string PopulationMemberCurrentAction(string settlementUuid, string populationUuid)
    {
        return Simulation.Instance.Query<string, string, string>(ClientQuery.PopulationMemberCurrentAction, settlementUuid, populationUuid);
    }
    
    public float PopulationMemberCurrentActionProgress(string settlementUuid, string populationUuid)
    {
        return Simulation.Instance.Query<float, string, string>(ClientQuery.PopulationMemberCurrentActionProgress, settlementUuid, populationUuid);
    }

    public string PopulationMemberCurrentActionName(string settlementUuid, string populationUuid)
    {
        return Simulation.Instance.Query<string, string, string>(ClientQuery.PopulationMemberCurrentActionName, settlementUuid, populationUuid);
    }
    
    // ----------------------
    // SPECIES
    // ----------------------
    
    public string SpeciesName(string speciesId)
    {
        return Simulation.Instance.Query<string, string>(ClientQuery.SpeciesName, speciesId);
    }
    
    public string[] SpeciesDescription(string speciesId)
    {
        return Simulation.Instance.Query<string[], string>(ClientQuery.SpeciesDescription, speciesId);
    }
    
    // ----------------------
    // TASKS
    // ----------------------
    
    public string[] SettlementTasks(string uuid)
    {
        return Simulation.Instance.Query<string[], string>(ClientQuery.SettlementTasks, uuid);
    }

    public string[] SettlementTaskAssignments(string settlementUuid, string taskId)
    {
        return Simulation.Instance.Query<string[], string, string>(ClientQuery.SettlementTaskAssignments, settlementUuid, taskId);
    }

    public string SettlementTaskName(string settlementUuid, string taskId)
    {
        return Simulation.Instance.Query<string, string, string>(ClientQuery.SettlementTaskName, settlementUuid, taskId);
    }
    
    public string[] SettlementTaskDescription(string settlementUuid, string taskId)
    {
        return Simulation.Instance.Query<string[], string, string>(ClientQuery.SettlementTaskDescription, settlementUuid, taskId);
    }
    
    public int SettlementTaskAssignedNum(string settlementUuid, string taskId)
    {
        return Simulation.Instance.Query<int, string, string>(ClientQuery.SettlementTaskAssignedNum, settlementUuid, taskId);
    }
    
    public int SettlementTaskMaximumCapacity(string settlementUuid, string taskId)
    {
        return Simulation.Instance.Query<int, string, string>(ClientQuery.SettlementTaskMaximumCapacity, settlementUuid, taskId);
    }
    
}