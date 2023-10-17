using System;
using System.Collections.Generic;

namespace Opibus
{
    public class ResourceSystem
    {
        public Dictionary<ResourceType, int> Resources { get; private set; }
        public Dictionary<ResourceType, int> StorageSpace { get; private set; }

        private const int resourceTypes = 3;

        private int defaultSpace;

        public ResourceSystem(
        int storageDefaultSpace = Int32.MaxValue,
        bool divideSpace = false,
        int startResourcesAmount = 0)
        {
            defaultSpace = divideSpace ? storageDefaultSpace / resourceTypes : storageDefaultSpace;
            Resources = new Dictionary<ResourceType, int>()
            {
                {ResourceType.Ore, startResourcesAmount},
                {ResourceType.Metal, startResourcesAmount},
                {ResourceType.Energy, startResourcesAmount}
            };
            StorageSpace = new Dictionary<ResourceType, int>()
            {
                {ResourceType.Ore, defaultSpace},
                {ResourceType.Metal, defaultSpace},
                {ResourceType.Energy, defaultSpace},
            };
        }

        public bool CanUseResources(List<ResourceAmountPair> resources)
        {
            foreach (var resource in resources)
                if (resource.Amount > Resources[resource.Type])
                    return false;

            return true;
        }

        public void UseResources(List<ResourceAmountPair> resources)
        {
            foreach (var resource in resources)
            {
                Resources[resource.Type] -= resource.Amount;
                if (Resources[resource.Type] < 0)
                    Resources[resource.Type] = 0;

            }
        }

        public bool IsFulled(ResourceType type) =>
            Resources[type] >= StorageSpace[type];

        public void ReceiveResources(ResourceType type, int amount)
        {
            Resources[type] += amount;
            if (IsFulled(type))
                Resources[type] = StorageSpace[type];
        }

        public void UpdateStorageSpace(ResourceType type, bool increase)
        {
            if (defaultSpace != Int32.MaxValue)
                StorageSpace[type] += increase ? defaultSpace : -defaultSpace;
        }
    }
}