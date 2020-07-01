using System.Collections;
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
        [SerializeField] private ItemCollector collector;
        [Header("Size: 1 = Wash, 2 = Dry, 3 = Fold")]
        public UnityEvent[] onStart;
        public UnityEvent[] onEnd;
        public bool endHold;
        public bool pressedButton;
        
        public PlayerInteractionController player;
        private Outline _outline;
        private float _startTime;
        private bool _stop = true;
        private bool _isDone;
        private int _interactAmount = 0;
        private CleanState _cleanState;
        public bool Stop => _stop;

        private void Start()
        {
            _outline = GetComponentInChildren<Outline>();
            endHold = true;
        }


        public void Interact()
        {
            if (!CheckRoom()) return;
            if (_isDone) return;
            if (!collector) return;
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
            if (!CheckRoom()) return;
            _stop = true;
            if (!saveProgress)
            {
                _startTime = 0;
            }
            if(useTimer)timerbase.SetActive(false);
        }

        private void Update()
        {
            if(player)pressedButton = player.inputI >= 1f;
            if(_stop) return;
            if (!CheckRoom()) return;
            _startTime += Time.deltaTime;
            if (!(_startTime >= duration)) return;
            _stop = true;
            timerbase.SetActive(false);
            onEnd[_interactAmount].Invoke();
            SetCleanState();
            endHold = true;
            if (_interactAmount < onEnd.Length) _interactAmount++;
            if (_interactAmount >= onEnd.Length)_isDone = true;
            _startTime = 0;
        }

        private void SetCleanState()
        {
            switch (_interactAmount)
            {
                case 1:
                    _cleanState = CleanState.Wash;
                    break;
                case 2:
                    _cleanState = CleanState.Dry;
                    break;
                case 3:
                    _cleanState = CleanState.Fold;
                    break;
                default:
                    break;
            }
        }

        public void StartTimer()
        {
            _stop = false;
            endHold = false;
            StartCoroutine(UpdateCoroutine());
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Untagged")) return;
            if (!other.CompareTag("Player") && !other.CompareTag("Player2")) return;
            if (player) return;
            player = other.GetComponentInChildren<PlayerInteractionController>();
        }

        private void OnTriggerExit(Collider other)
        {
            if(useTimer)timerbase.SetActive(false);
            player = null;
        }
        
        private IEnumerator UpdateCoroutine()
        {
            while(!_stop)
            {
                timerImage.fillAmount = _startTime / duration;
                yield return new WaitForSeconds(0.2f);
            }
        }

        private bool CheckRoom()
        {
            if (!_outline) return true;
            if (!_outline.roomTarget) return true;
            if (collector.collectedItems.Count < 1) return false;
            return _outline.roomTarget.RoomCleared;
        }

    }
}
