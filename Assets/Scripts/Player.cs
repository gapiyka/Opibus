using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Opibus;
using Unity.VisualScripting;
using UnityEngine;

namespace Opibus
{
    [RequireComponent(typeof(CharacterController))]
    public class Player : MonoBehaviour
    {
        [SerializeField] private float speed;
        [SerializeField] private float resourcesTransferTime;
        [SerializeField] private int resourcesPerTransfer;
        [SerializeField] private int resourcesLimit;
        [SerializeField] private float itemOffset;
        [SerializeField] private Transform backpack;
        [SerializeField] private GameObject storePrefab;

        private CharacterController controller;
        private ResourceSystem inventory;
        private bool isTransferring;
        private int itemsInBackPack;

        private void Start()
        {
            controller = gameObject.GetComponent<CharacterController>();
            inventory = new(resourcesLimit, true);
        }

        private void Update()
        {
            Vector3 move =
                new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
            controller.Move(move * Time.deltaTime * speed);

            if (move != Vector3.zero)
            {
                gameObject.transform.forward = move;
            }
        }

        private IEnumerator CollectFromBuilding(Building building)
        {
            isTransferring = true;
            while (!inventory.IsFulled(building.ResourceType))
            {
                yield return new WaitForSeconds(resourcesTransferTime);
                if (!building.TryToCollect(resourcesPerTransfer))
                    continue;
                inventory.ReceiveResources(building.ResourceType, resourcesPerTransfer);
                AddToBackpack();
            }
            isTransferring = false;
        }

        private IEnumerator PutIntoStorage(StorageManager storage)
        {
            isTransferring = true;
            List<ResourceAmountPair> list = inventory.Resources
                .Where(res => res.Value != 0)
                .Select(res => new ResourceAmountPair(res.Key, res.Value))
                .ToList();
            while (list.Count != 0)
            {
                yield return new WaitForSeconds(resourcesTransferTime);
                var resource = list[0];
                var resourcePair = new List<ResourceAmountPair>(1)
                    { new(resource.Type, resourcesPerTransfer) };
                if (!inventory.CanUseResources(resourcePair) ||
                storage.IsFulled(resource.Type))
                    continue;
                inventory.UseResources(resourcePair);
                storage.ReceiveResources(resource.Type, resourcesPerTransfer);
                RemoveFromBackpack();
                list[0].Amount -= resourcesPerTransfer;
                if (list[0].Amount <= 0) list.RemoveAt(0);
            }
            isTransferring = false;
        }

        private void AddToBackpack()
        {
            itemsInBackPack++;
            Instantiate(storePrefab,
                backpack.position + Vector3.up * itemsInBackPack * itemOffset,
                transform.rotation, backpack);
        }


        private void RemoveFromBackpack()
        {
            itemsInBackPack--;
            Destroy(backpack.GetChild(itemsInBackPack).gameObject);
        }

        private void OnTriggerEnter(Collider collider)
        {
            var parent = collider.transform.parent;
            if (parent.CompareTag("Producer"))
            {
                var building = parent.GetComponent<Building>();
                if (!isTransferring)
                    StartCoroutine(CollectFromBuilding(building));
                return;
            }
            if (parent.CompareTag("Storage"))
            {
                var storage = parent.GetComponent<StorageManager>();
                if (!isTransferring)
                    StartCoroutine(PutIntoStorage(storage));
                return;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            StopAllCoroutines();
            isTransferring = false;
            Debug.Log("EXIT");
        }
    }
}