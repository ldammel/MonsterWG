using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Utility
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance;
        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogWarning(nameof(SoundManager) + " is a singleton class and may only be initialized once");
                Application.Quit();
            }
            Instance = this;
            
            /*if (Clips.Any(c => c == null)) {
                Debug.LogWarning(nameof(Clips) + " in " + nameof(SoundManager) + " may not contain null values", this);
                Application.Quit();
            }
            
            if (Enum.GetNames(typeof(Sounds)).Length != Clips.Length) {
                Debug.LogWarning("Enum item count of " + nameof(Sounds) + " in " + nameof(SoundManager) + " has to be equal to the length of " + nameof(Clips), this);
                Application.Quit();
            }*/
            _audioSourceList = new List<AudioSource>();
        }
        public AudioClip[] Clips;
        private List<AudioSource> _audioSourceList;
        public enum Sounds
        {
            MüllWegwerfen,
            SchrankStopfen,
            SchrankKnallen,
            BodenWischen,
            MopNässen,
            GeschirrSpülen,
            BettSchütteln,
            BadReinigen,
            BlumenGießen,
            TvAusschalten,
            TvStaticSound,
            Interagieren,
            InputCorrect,
            InputWrong,
            Putzplan,
            Ambiente
        }
        
        public void Play(GameObject source, Sounds sound, int priority = 128, float volume = 1f) {
            if(Instance.Clips[(int) sound] != null)PlaySound(source, Instance.Clips[(int) sound], priority, volume);
        }

        public void Stop()
        {
            _audioSourceList.ForEach(Destroy);
        }

        public void PlayDelayed(float delay, GameObject source, Sounds sound, int priority = 128, float volume = 1f) {
            Instance.StartCoroutine(ExecuteDelayed(
                delay, delegate { PlaySound(source, Instance.Clips[(int) sound], priority, volume); }
            ));
        }

        private IEnumerator ExecuteDelayed(float delay, UnityAction action) {
            yield return new WaitForSeconds(delay);
            action.Invoke();
        }

        private void PlaySound(GameObject source, AudioClip clip, int priority = 128, float volume = 1f) {
            var audioSource = source.AddComponent<AudioSource>();
            _audioSourceList.Add(audioSource);
            audioSource.priority = priority;
            audioSource.clip = clip;
            audioSource.volume = volume;
            audioSource.Play();
            Destroy(audioSource, clip.length);
        }

        public AudioClip GetClip(Sounds s) {
            return Instance.Clips[(int) s];
        }
    }
}
