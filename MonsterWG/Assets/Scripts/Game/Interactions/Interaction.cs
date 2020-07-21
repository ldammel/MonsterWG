﻿using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Outline = Game.Utility.Outline;
using Sirenix.OdinInspector;

namespace Game.Interactions
{
    public class Interaction : MonoBehaviour
    {
        [FoldoutGroup("Settings")]
        [SerializeField] private float duration = 5f;
        [FoldoutGroup("Settings")]
        [SerializeField] private bool useTimer;
        [FoldoutGroup("Settings")]
        [SerializeField] private bool saveProgress;
        [FoldoutGroup("Settings")]
        [SerializeField] private ItemCollector collector;
        [FoldoutGroup("Settings")]
        [SerializeField] private DishDisplay dishDisplay;
        [FoldoutGroup("Settings")]
        [SerializeField] private bool useCleaningCondition;
        [FoldoutGroup("Settings")]
        [SerializeField] private CleaningCondition cleaningCondition;
        [FoldoutGroup("UI")]
        [SerializeField] private Image timerImage;
        [FoldoutGroup("UI")]
        [SerializeField] private GameObject timerbase;
        [FoldoutGroup("Events")]
        [Header("Size: 1 = Wash, 2 = Dry, 3 = Fold")]
        public UnityEvent[] onStart;
        [FoldoutGroup("Events")]
        public UnityEvent[] onEnd;
        
        [HideInInspector] public bool endHold;
        [HideInInspector] public bool pressedButton;
        public PlayerInteractionController player;
        private Outline _outline;
        public float _startTime;
        public bool _stop = true;
        public bool _isDone;
        public int _interactAmount = 0;
        private CleanState _cleanState;
        public bool Stop => _stop;
        public bool CanStart;

        private void Start()
        {
            _outline = GetComponentInChildren<Outline>();
            endHold = true;
            if(!collector)CanStart = true;
        }


        public void Interact()
        {
            if (useCleaningCondition)
            {
                if (!cleaningCondition.IsMet) return;
            }
            if (!CheckRoom()) return;
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
            if(player)pressedButton = player.InputI >= 1f;
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
            if (dishDisplay)
            {
                if (dishDisplay.displayAmount > 0)
                {
                    Reset();
                    _startTime = 0;
                    return;
                }
            }
            if (_isDone) player._interactions.Remove(this);
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
            if(collector) if(collector.collectedItems.Count >= 1) CanStart = true;
            if (!CanStart) return false;
            if (!_outline) return true;
            if (!_outline.roomTarget) return true;
            return _outline.roomTarget.RoomCleared;
        }

    }
}
