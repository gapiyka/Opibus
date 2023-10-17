using System.Collections.Generic;
using UnityEngine;

namespace Opibus.Resource
{
    public class StorageManager : MonoBehaviour
    {
        private ResourceSystem resourceSystem;

        public bool CanUseResources(List<ResourceAmountPair> needs) =>
            resourceSystem.CanUseResources(needs);

        public void UseResources(List<ResourceAmountPair> needs) =>
            resourceSystem.UseResources(needs);

        public bool IsFulled(ResourceType type) =>
            resourceSystem.IsFulled(type);

        public void ReceiveResources(ResourceType type, int amount) =>
            resourceSystem.ReceiveResources(type, amount);

        private void Awake()
        {
            resourceSystem = new();
        }
    }
}