using System.Linq;

namespace LamiaSimulation
{
    public static class Helpers
    {
        public static PopulationSpeciesType GetSpeciesTypeById(string ID)
        {
            var filtered = DataQuery<PopulationSpeciesType>.GetByID(ID);
            if(filtered == null)
                throw new ClientActionException(T._("Species does not exist."));
            return filtered;
        }

        public static TaskType GetTaskTypeById(string ID)
        {
            var filtered = DataQuery<TaskType>.GetByID(ID);
            if(filtered == null)
                throw new ClientActionException(T._("Task does not exist."));
            return filtered;
        }

        public static ResourceType GetResourceTypeById(string ID)
        {
            var filtered = DataQuery<ResourceType>.GetByID(ID);
            if(filtered == null)
                throw new ClientActionException(T._("Resource does not exist."));
            return filtered;
        }
        
        public static LocationType GetLocationTypeById(string ID)
        {
            var filtered = DataQuery<LocationType>.GetByID(ID);
            if(filtered == null)
                throw new ClientActionException(T._("Location type does not exist."));
            return filtered;
        }
        
        public static T1 GetDataTypeById<T1>(string ID) where T1 : DataType
        {
            var filtered = DataQuery<T1>.GetByID(ID);
            if(filtered == null)
                throw new ClientActionException(T._("Resource does not exist."));
            return filtered;
        }
        

    }
}