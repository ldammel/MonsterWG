using System.Collections;
using Game.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game.Interactions
{
    public class Interaction : MonoBehaviour
    {
        [SerializeField] private float duration = 5f;
        [SerializeField] private bool useTimer;
        [SerializeField] private Image timerImage;
        [SerializeField] private GameObject timerbase;
        [SerializeField] private bool saveProgress;
        public UnityEvent[] onStart;
        public UnityEvent[] onEnd;
        
        public PlayerInteractionController player;
        private float _startTime;
        private bool _stop = true;
        private bool _isDone;
        private int _interactAmount = 0;
        public bool Stop => _stop;


        public void Interact()
        {
            if (_isDone) return;
            if (useTimer)
            {
                StartTimer();
                timerbase.SetActive(true);
            }
            onStart[_interactAmount].Invoke();
        }

        public void Reset()
        {
            _isDone = false;
            _interactAmount = 0;
        }

        public void SetDuration(int newDuration)
        {
            duration = newDuration;
        }

        public void Cancel()
        {
            _stop = true;
            if (!saveProgress)
            {
                _startTime = 0;
            }
            if(useTimer)timerbase.SetActive(false);
        }

        private void Update()
        {
            if(_stop) return;
            _startTime += Time.deltaTime;
            if (!(_startTime >= duration)) return;
            _stop = true;
            timerbase.SetActive(false);
            onEnd[_interactAmount].Invoke();
            if (_interactAmount < onEnd.Length) _interactAmount++;
            if (_interactAmount >= onEnd.Length) _isDone = true;
            _startTime = 0;
        }

        public void StartTimer()
        {
            _stop = false;
            StartCoroutine(UpdateCoroutine());
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Untagged")) return;
            //player.interactImage.SetActive(true);
            player.isInTrigger = true;
        }
                
        private void OnTriggerExit(Collider other)
        {
            //if (player == null) return;
            //player.interactImage.SetActive(false);
            if(useTimer)timerbase.SetActive(false);
        }
        
        private IEnumerator UpdateCoroutine()
        {
            while(!_stop)
            {
                timerImage.fillAmount = _startTime / duration;
                yield return new WaitForSeconds(0.2f);
            }
        }

    }
}
