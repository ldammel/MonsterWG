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
        
        [HideInInspector]public bool isInHand = false;
        [HideInInspector]public bool inTrigger;
        [HideInInspector]public bool pressedButton;

        private GameObject _interactionTarget;
        private PlayerInteractionController _player;
        private bool _isPickedUp;
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
            if(_player)pressedButton = _player.InputS >= 1f;

            if (_isPickedUp)
            {
                _rigidBody.isKinematic = true;
                _interactionTarget.transform.position = _player.handGrabPosition.position;
                _interactionTarget.transform.parent = _player.handGrabPosition;
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
            _player.CurrentItem = this;
            _isPickedUp = true;
            onPickUp.Invoke();
        }

        public void CancelPickUp()
        {
            _player.CurrentItem = null;
            _isPickedUp = false;
            pressedButton = false;
            _player = null;
        }
        #endregion
        
        #region OnTriggerEnter/Exit
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Untagged")) return;
            if (other.CompareTag("Player") || other.CompareTag("Player2"))
            {
                if (_player) return;
                _player = other.GetComponentInChildren<PlayerInteractionController>();
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
                _player = null;
            }
        }
        #endregion
    }
}
