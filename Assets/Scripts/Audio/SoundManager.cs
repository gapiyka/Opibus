using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Opibus.Audio
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundManager : MonoBehaviour
    {
        [SerializeField] private SoundsDatabase database;
        [SerializeField] private int playSoundsPerOnce;
        private const float pitchDif = 0.1f;

        private AudioSource audioSource;
        private Dictionary<SoundType, int> playSoundCounters;

        public void PlaySound(SoundType soundType)
        {
            if (playSoundCounters[soundType] >= playSoundsPerOnce)
                return;
            playSoundCounters[soundType]++;
            var clip = database.sounds.Find(s => s.Type == soundType).Clip;
            audioSource.pitch = GetPitch();
            audioSource.PlayOneShot(clip);
            StartCoroutine(ReduceCounter(soundType, clip.length));
        }

        private void Awake()
        {
            audioSource = transform.GetComponent<AudioSource>();
            playSoundCounters = new();
            foreach (var sound in database.sounds)
                playSoundCounters.Add(sound.Type, 0);
        }

        private IEnumerator ReduceCounter(SoundType type, float sec)
        {
            yield return new WaitForSeconds(sec);
            playSoundCounters[type]--;
        }

        private float GetPitch()
        {
            return 1 + Random.Range(-1, 2) * pitchDif;
        }
    }
}
