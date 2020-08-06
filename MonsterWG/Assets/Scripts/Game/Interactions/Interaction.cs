using System.Collections;
using Game.Utility;
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
        private bool _soundPlayed;

        private void Start()
        {
            _outline = GetComponentInChildren<Outline>();
            endHold = true;
            if(!collector)canStart = true;
        }

        public void ClearInteractions()
        {
            player._interactions.Clear();
        }


        public InteractionResult Interact()
        {
            if (useCleaningCondition)
            {
                InteractionResult result = cleaningCondition.IsConditionMet();

                if(result != InteractionResult.Success)
                {
                    return result;
                }
            }
            if (_isDone) return InteractionResult.Failed;
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
                    if (!_soundPlayed)
                    {
                        _soundPlayed = true;
                        SoundManager.Instance.Play(gameObject, SoundManager.Sounds.MopNässen,128,2);
                    }
                }else _soundPlayed = false;
            }

            if(onStart != null && _interactAmount < onStart.Length)
                onStart[_interactAmount].Invoke();

            if (consumesItem)
            {
                player.CurrentItem.SetMeshVisibility(false);
            }

            if (player.CurrentItem && player.CurrentItem.NeedsWater && !isWaterSource)
            {
                player.CurrentItem.SetMeshVisibility(false);
            }

            return InteractionResult.Success;
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

            if (player.CurrentItem && player.CurrentItem.NeedsWater && !isWaterSource)
            {
                player.CurrentItem.SetMeshVisibility(true);
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

        public void StartAnimation(int id)
        {
            if (player)
            {
                player.character.animationHelper.SetInt("Interaction", id);
            }
        }

        public void StartOneShotAnimation(int id)
        {
            if (player)
            {
                player.character.animationHelper.SetInt("Interaction", id);

                StartCoroutine(StopAnimationAfterTime(0.1f));
            }
        }

        public void StopAnimation()
        {
            player.character.animationHelper.SetInt("Interaction", 0);
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
            if (other.CompareTag("Untagged")) return;
            if (!other.CompareTag("Player") && !other.CompareTag("Player2")) return;

            if (useTimer)timerbase.SetActive(false);
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

        private IEnumerator StopAnimationAfterTime(float seconds)
        {
            yield return new WaitForSeconds(seconds);
            StopAnimation();
        }

        public enum InteractionResult
        {
            Invalid = -1,
            Failed,
            Success,
            NeedsFreeHands,
            NeedsItem,
            NeedsWater,
            WrongType
        }

    }
}
