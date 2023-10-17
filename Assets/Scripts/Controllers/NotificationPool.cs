using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Pool;


namespace Opibus.Controllers
{
    public class NotificationPool : MonoBehaviour
    {
        [SerializeField] private GameObject notificationPrefab;
        [SerializeField] private int maxSpawnSize;

        private const string stateName = "PopUp";
        private const string speed = "SpeedMltpl";

        private ObjectPool<GameObject> pool;

        public void NewNotification(Vector3 pos, int amount, float time)
        {
            var obj = pool.Get();
            obj.transform.position = pos;
            obj.GetComponentInChildren<TextMeshPro>().text =
                $"+{amount}";
            var animator = obj.GetComponent<Animator>();
            animator.SetFloat(speed, 1 / time);
            animator.Play(stateName);
            StartCoroutine(Release(obj, time));
        }

        private void Awake()
        {
            pool = new ObjectPool<GameObject>(
                CreateText, OnGetText, OnReleaseText,
                OnDestroyText, maxSize: maxSpawnSize);
        }

        private IEnumerator Release(GameObject obj, float time)
        {
            yield return new WaitForSeconds(time);
            pool.Release(obj);
        }

        private void OnDestroyText(GameObject text) => Destroy(text);

        private void OnReleaseText(GameObject text) => text.SetActive(false);

        private void OnGetText(GameObject text) => text.SetActive(true);

        private GameObject CreateText()
        {
            var newObj = Instantiate(notificationPrefab, transform);
            newObj.SetActive(false);
            return newObj;
        }
    }
}