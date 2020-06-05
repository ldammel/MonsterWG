using System;
using System.Collections;
using Game.UI;
using Game.Utility;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Outline = Game.Utility.Outline;

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
        private Outline _outline;
        private float _startTime;
        private bool _stop = true;
        private bool _isDone;
        private int _interactAmount = 0;
        public bool Stop => _stop;

        private void Start()
        {
            _outline = GetComponentInChildren<Outline>();
        }


        public void Interact()
        {
            if (_outline)
            {
                if (_outline.roomTarget)
                {
                    if (!_outline.roomTarget.RoomCleared) return;
                }
            }
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
            if (_outline)
            {
                if (_outline.roomTarget)
                {
                    if (!_outline.roomTarget.RoomCleared) return;
                }
            }
            _isDone = false;
            _interactAmount = 0;
        }

        public void SetDuration(int newDuration)
        {
            duration = newDuration;
        }

        public void Cancel()
        {
            if (_outline)
            {
                if (_outline.roomTarget)
                {
                    if (!_outline.roomTarget.RoomCleared) return;
                }
            }
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
            if (_outline)
            {
                if (_outline.roomTarget)
                {
                    if (!_outline.roomTarget.RoomCleared) return;
                }
            }
            _startTime += Time.deltaTime;
            if (!(_startTime >= duration)) return;
            _stop = true;
            timerbase.SetActive(false);
            onEnd[_interactAmount].Invoke();
            if (player.interactions.Contains(this)) player.interactions.Remove(this);
            if (_interactAmount < onEnd.Length) _interactAmount++;
            if (_interactAmount >= onEnd.Length) _isDone = true;
            _startTime = 0;
        }

        public void StartTimer()
        {
            _stop = false;
            StartCoroutine(UpdateCoroutine());
        }
                
        private void OnTriggerExit(Collider other)
        {
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
