using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Opibus
{
    public class Building : MonoBehaviour
    {
        [Tooltip("Time (in seconds) necessary to each produce operation")]
        [SerializeField] private float produceTime;
        [Tooltip("Amount of resources produced in one operation")]
        [SerializeField] private int produceAmount;
        [SerializeField] private List<ResourceAmountPair> requirements;
        [SerializeField] private ResourceType produceType;
        [SerializeField] private StorageManager storage;

        public ResourceType ResourceType { get => produceType; }
        public int ProducedResources { get; private set; }

        private bool isProducing;

        private void Update()
        {
            // TODO: change on events or while loop
            if (!isProducing && storage.CanUseResources(requirements))
                StartCoroutine(Produce());
            Debug.Log($"Produced {ProducedResources} by {gameObject.name}");
        }

        private IEnumerator Produce()
        {
            isProducing = true;
            storage.UseResources(requirements);
            yield return new WaitForSeconds(produceTime);
            ProducedResources += produceAmount;
            storage.ReceiveResources(produceType, produceAmount);
            isProducing = false;
        }

        private void OnDestroy() => StopAllCoroutines();
    }
}