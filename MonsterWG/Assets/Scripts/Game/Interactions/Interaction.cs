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
        
        private PlayerInteractionController _player;
        private float _startTime;
        private bool _stop = true;
        private bool _isDone;
        private int _interactAmount = 0;


        public void Interact()
        {
            if (_isDone) return;
            if (!_player.isInTrigger) return;
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

        private void Cancel()
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
            _player = other.GetComponent<PlayerInteractionController>();
            if (_player.isPlayerOne)
            {
                _player.character.controls.Player.Interact.Enable();
                _player.character.controls.Player.Interact.performed += _ => Interact();
                _player.character.controls.Player.Interact.canceled += _ => Cancel();
            }
            else
            {
                _player.character.controls.Player2.Interact.Enable();
                _player.character.controls.Player2.Interact.performed += _ => Interact();
                _player.character.controls.Player2.Interact.canceled += _ => Cancel();
            }
            _player.interactImage.SetActive(true);
            _player.isInTrigger = true;
        }
                
        private void OnTriggerExit(Collider other)
        {
            if (_player == null) return; 
            _player.character.controls.Player.Interact.Disable();
            _player.character.controls.Player2.Interact.Disable();
            _player.interactImage.SetActive(false);
            _player.isInTrigger = false;
            if(useTimer)timerbase.SetActive(false);
            _player = null;
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
