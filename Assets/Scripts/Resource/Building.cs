using System.Collections;
using System.Collections.Generic;
using Opibus.Controllers;
using UnityEngine;

namespace Opibus.Resource
{
    public class Building : MonoBehaviour
    {
        [Tooltip("Time (in seconds) necessary to each produce operation")]
        [SerializeField] private float produceTime;
        [Tooltip("Amount of resources produced in one operation")]
        [SerializeField] private int produceAmount;
        [SerializeField] private int produceLimit;
        [SerializeField] private List<ResourceAmountPair> requirements;
        [SerializeField] private ResourceType produceType;
        [SerializeField] private StorageManager storage;
        [SerializeField] private NotificationPool notificationPool;
        [SerializeField] private UiController ui;

        public ResourceType ResourceType { get => produceType; }
        public int ProducedResources { get; private set; }

        private const string resourcesWarn = "Not enough resources on storage";
        private const string storageWarn = "Storage is full";
        private const string producingText = "Is Producing";
        private bool isProducing;


        public bool TryToCollect(int amount)
        {
            if (ProducedResources < amount) return false;
            ProducedResources -= amount;
            return true;
        }

        private void Update()
        {
            // TODO: change on events or while loop
            if (!isProducing)
            {
                if (!storage.CanUseResources(requirements))
                {
                    ui.ChangeBuildingDescription(this,
                        GetWarnMessage(resourcesWarn));
                    return;
                }
                if (IsLocalStorageFull())
                {
                    ui.ChangeBuildingDescription(this,
                        GetWarnMessage(storageWarn));
                    return;
                }
                ui.ChangeBuildingDescription(this, producingText);
                StartCoroutine(Produce());
            }
        }

        private IEnumerator Produce()
        {
            isProducing = true;
            storage.UseResources(requirements);
            notificationPool.NewNotification(transform.position, produceAmount, produceTime);
            yield return new WaitForSeconds(produceTime);
            ProducedResources += produceAmount;
            if (IsLocalStorageFull())
                ProducedResources = produceLimit;
            isProducing = false;
        }

        private string GetWarnMessage(string reason) =>
            $"Stopped due to [{reason}]";

        private bool IsLocalStorageFull() => ProducedResources >= produceLimit;

        private void OnDestroy() => StopAllCoroutines();
    }
}