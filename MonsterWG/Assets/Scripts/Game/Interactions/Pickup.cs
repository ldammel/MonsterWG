using UnityEngine;
using UnityEngine.Events;


namespace Game.Interactions
{
    public class Pickup : MonoBehaviour
    {
        [SerializeField] private Transform baseParent;
        [SerializeField] private bool cleaned;
        public UnityEvent onPickUp;
        public UnityEvent onDrop;
        
        public PlayerInteractionController player;
        public bool isInHand = false;
        private GameObject _interactionTarget;
        public bool inTrigger;
        public bool pressedButton;

        private bool _isPickedUp;

        private bool _currentPlayer;
        private Rigidbody _rigidbody;

        private void Update()
        {
            if(player)pressedButton = player.inputS >= 1f;

            if (_isPickedUp)
            {
                _rigidbody.isKinematic = true;
                _interactionTarget.transform.position = player.handGrabPosition.position;
                _interactionTarget.transform.parent = player.handGrabPosition;
                isInHand = true;
            }
            else
            {
                _interactionTarget.transform.parent = baseParent;
                _rigidbody.isKinematic = false;
                isInHand = false;
            }
            
        }

        private void Start()
        {
            _interactionTarget = gameObject;
            _rigidbody = _interactionTarget.GetComponent<Rigidbody>();
        }

        public void PickUp()
        {
            player.currentItem = this;
            _isPickedUp = true;
            onPickUp.Invoke();
        }

        public void CancelPickUp()
        {
            player.currentItem = null;
            _isPickedUp = false;
            pressedButton = false;
            player = null;
        }

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
                    if(!cleaned) return;
                }
                other.gameObject.GetComponent<StoreInteraction>().AddObject(this);
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
    }
}
