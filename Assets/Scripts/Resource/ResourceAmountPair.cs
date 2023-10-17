using System;
using UnityEngine;

namespace Opibus.Resource
{
    [Serializable]
    public class ResourceAmountPair
    {
        [field: SerializeField] public ResourceType Type { get; private set; }
        [field: SerializeField] public int Amount { get; set; }
        public ResourceAmountPair(ResourceType type, int amount)
        {
            Type = type;
            Amount = amount;
        }
    }
}