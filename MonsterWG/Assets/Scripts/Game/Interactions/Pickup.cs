using System.Collections;
using Game.Utility;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;


namespace Game.Interactions
{
    public class Pickup : MonoBehaviour
    {
        #region Variables
        [FoldoutGroup("Settings")]
        [SerializeField] private Transform baseParent;
        [FoldoutGroup("Settings")]
        [SerializeField] private bool cleaned;
        [FoldoutGroup("Settings")]
        public bool canNotBeStored;

        public bool canBeStored => !canNotBeStored;
        [FoldoutGroup("Settings")]
        public bool isTrash;
        [FoldoutGroup("Settings")]
        [ShowIf(nameof(isTrash))]
        public GameObject trashBag;
        [FoldoutGroup("Settings")]
        [ShowIf(nameof(isTrash))]
        public GameObject itemObject;
        [FoldoutGroup("Events")]
        public UnityEvent onPickUp;
        [FoldoutGroup("Events")]
        public UnityEvent onDrop;

        [FoldoutGroup("Settings")]
        public bool NeedsWater;
        [FoldoutGroup("Settings"), ShowIf(nameof(NeedsWater))]
        public int CurrentWaterAmount;
        public bool isInHand = false;
        public bool inTrigger;
        public bool pressedButton;
        public PlayerInteractionController player;

        private GameObject _interactionTarget;
        public bool _isPickedUp;
        private bool _currentPlayer;
        private Rigidbody _rigidBody;
        #endregion
        
        #region Start/Update
        private void Start()
        {
            _interactionTarget = gameObject;
            _rigidBody = _interactionTarget.GetComponent<Rigidbody>();
        }

        private void Update()
        {
            if(player)pressedButton = player.InputPickUp >= 1f;

            if (_isPickedUp)
            {
                if (isTrash)
                {
                    // Trashbag is now in animation
                    //trashBag.SetActive(true);
                    itemObject.SetActive(false);
                    SoundManager.Instance.Play(gameObject, SoundManager.Sounds.Interagieren);
                }
                _rigidBody.isKinematic = true;
                _interactionTarget.transform.position = player.handGrabPosition.position;
                _interactionTarget.transform.parent = player.handGrabPosition;
                isInHand = true;
            }
            else
            {
                if (isTrash)
                {
                    trashBag.SetActive(false);
                    itemObject.SetActive(true);
                }
                _interactionTarget.transform.parent = baseParent;
                _rigidBody.isKinematic = false;
                isInHand = false;
            }
            
        }
        #endregion
        
        #region PickUp Functions
        public void PickUp()
        {
            player.CurrentItem = this;
            _isPickedUp = true;
            onPickUp.Invoke();
        }

        public void CancelPickUp()
        {
            if (gameObject.activeSelf)
            {
                StartCoroutine(Cancel());
            }
        }

        public void ForceCancelPickUp()
        {
            if (gameObject.activeSelf)
            {
                StartCoroutine(Cancel());
            }
        }

        private IEnumerator Cancel()
        {
            yield return new WaitForSeconds(0.1f);
            if(player)player.CurrentItem = null;
            _isPickedUp = false;
            pressedButton = false;
            player = null;
        }

        #endregion
        
        #region OnTriggerEnter/Exit
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Untagged")) return;
            if (other.CompareTag("Player") || other.CompareTag("Player2"))
            {
                if (player) return;
                player = other.GetComponentInChildren<PlayerInteractionController>();
                inTrigger = true;
            }

            if (other.CompareTag("PickupDest") && !isInHand)
            {
                if(other.gameObject.GetComponent<ItemCollector>())other.gameObject.GetComponent<ItemCollector>().InsertItem(this.gameObject.GetComponent<Item>());
                onDrop.Invoke();
            }

            if (other.CompareTag("Storage") && !isInHand)
            {
                other.gameObject.GetComponent<StoreInteraction>().AddObject(this.gameObject);
                gameObject.SetActive(false);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player") || other.CompareTag("Player2"))
            {
                inTrigger = false;
                if (isInHand) return;
                player = null;
            }
        }
        #endregion
    }
}
