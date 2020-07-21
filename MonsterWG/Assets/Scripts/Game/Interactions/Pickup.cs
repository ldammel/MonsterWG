using Game.Quests;
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
        [FoldoutGroup("Events")]
        public UnityEvent onPickUp;
        [FoldoutGroup("Events")]
        public UnityEvent onDrop;
        
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
            if(player)pressedButton = player.InputS >= 1f;

            if (_isPickedUp)
            {
                _rigidBody.isKinematic = true;
                _interactionTarget.transform.position = player.handGrabPosition.position;
                _interactionTarget.transform.parent = player.handGrabPosition;
                isInHand = true;
            }
            else
            {
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
                if (other.gameObject.GetComponent<StoreInteraction>().isQuestStorage)
                {
                    //if(!cleaned) return;
                }
                other.gameObject.GetComponent<StoreInteraction>().AddObject(this.GetComponent<QuestAssign>());
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
