using System;
using System.Collections;
using System.Linq;
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
            
            if (Clips.Any(c => c == null)) {
                Debug.LogWarning(nameof(Clips) + " in " + nameof(SoundManager) + " may not contain null values", this);
                Application.Quit();
            }
            
            if (Enum.GetNames(typeof(Sounds)).Length != Clips.Length) {
                Debug.LogWarning("Enum item count of " + nameof(Sounds) + " in " + nameof(SoundManager) + " has to be equal to the length of " + nameof(Clips), this);
                Application.Quit();
            }
        }
        public AudioClip[] Clips;
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
            Glitzern,
            Laufen,
            Interagieren,
            InputCorrect,
            InputWrong,
            RoySchnarchen,
            RoyMurmeln,     
            JimmySchnarchen,
            JimmyMurmeln,
            TelefonKlingeln,
            TimerCountdown,
            TimerAuto,
            Putzplan,
            StadtAmbiente,
            VögelAmbiente
        }
        
        public static void Play(GameObject source, Sounds sound, int priority = 128, float volume = 1f) {
            PlaySound(source, Instance.Clips[(int) sound], priority, volume);
        }

        public static void PlayDelayed(float delay, GameObject source, Sounds sound, int priority = 128, float volume = 1f) {
            Instance.StartCoroutine(ExecuteDelayed(
                delay, delegate { PlaySound(source, Instance.Clips[(int) sound], priority, volume); }
            ));
        }

        private static IEnumerator ExecuteDelayed(float delay, UnityAction action) {
            yield return new WaitForSeconds(delay);
            action.Invoke();
        }

        private static void PlaySound(GameObject source, AudioClip clip, int priority = 128, float volume = 1f) {
            var audioSource = source.AddComponent<AudioSource>();
            audioSource.priority = priority;
            audioSource.clip = clip;
            audioSource.volume = volume;
            audioSource.Play();
            Destroy(audioSource, clip.length);
        }

        public static AudioClip GetClip(Sounds s) {
            return Instance.Clips[(int) s];
        }
    }
}
