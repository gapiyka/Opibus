using System;
using UnityEngine;

namespace Opibus
{
    [Serializable]
    public class ResourceAmountPair
    {
        [field: SerializeField] public ResourceType Type { get; private set; }
        [field: SerializeField] public int Amount { get; private set; }
    }
}