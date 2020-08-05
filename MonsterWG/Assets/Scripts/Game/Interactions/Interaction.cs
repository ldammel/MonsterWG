using System.Collections;
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
        public bool useCleaningCondition;
        [FoldoutGroup("Settings")]
        [SerializeField] private bool canBeDone = true;
        [FoldoutGroup("Settings")]
        public CleaningCondition cleaningCondition;
        [FoldoutGroup("Settings")]
        public bool isWaterSource;
        [FoldoutGroup("Settings")]
        public bool consumesItem;
        [FoldoutGroup("UI")]
        [SerializeField] private Image timerImage;
        [FoldoutGroup("UI")]
        [SerializeField] private GameObject timerbase;
        [FoldoutGroup("Events")]
        [Header("Size: 1 = Wash, 2 = Dry, 3 = Fold")]
        public UnityEvent[] onStart;
        [FoldoutGroup("Events")]
        public UnityEvent onCancel;
        [FoldoutGroup("Events")]
        public UnityEvent[] onEnd;
        
        [HideInInspector] public bool endHold;
        [HideInInspector] public bool pressedButton;
        public PlayerInteractionController player;
        private Outline _outline;
        private float _startTime;
        private bool _stop = true;
        private bool _isDone;
        private int _interactAmount = 0;
        private CleanState _cleanState;
        public bool Stop => _stop;
        public bool canStart;

        private void Start()
        {
            _outline = GetComponentInChildren<Outline>();
            endHold = true;
            if(!collector)canStart = true;
        }


        public bool Interact()
        {
            if (useCleaningCondition)
            {
                if (!cleaningCondition.IsConditionMet()) return false;
            }
            if (_isDone) return false;
            if (useTimer)
            {
                StartTimer();
                timerbase.SetActive(true);
            }

            if (isWaterSource)
            {
                if(player.CurrentItem && player.CurrentItem.NeedsWater)
                {
                    Debug.Log("Item Watered!", player.CurrentItem);
                    player.CurrentItem.CurrentWaterAmount = 100;
                }
            }

            if(onStart != null && _interactAmount < onStart.Length)
                onStart[_interactAmount].Invoke();

            return true;
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
            if (player) player.character.canMove = true;
            if (!saveProgress)
            {
                _startTime = 0;
            }
            if(useTimer)timerbase.SetActive(false);

            if (onCancel != null)
                onCancel.Invoke();
        }

        public void SetDone()
        {
            if (canBeDone)
            {
                _isDone = true;
            }

            if (consumesItem)
            {
                Pickup pickup = player.CurrentItem;
                if(pickup)pickup.ForceCancelPickUp();

                Destroy(pickup.gameObject);
            }

            if (dishDisplay)
            {
                dishDisplay.AddDisplay();
            }
        }

        public bool IsDone()
        {
            if (canBeDone)
            {
                return _isDone;
            }

            return false;
        }

        private void Update()
        {
            if(player)pressedButton = player.InputInteraction >= 1f;
            if(_stop) return;
            if(player)player.character.canMove = !pressedButton;
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
            if (player) player.character.canMove = true;
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

    }
}
