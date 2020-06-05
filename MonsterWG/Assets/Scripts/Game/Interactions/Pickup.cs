using System;
using System.Runtime.CompilerServices;
using Game.Character;
using Game.Utility;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;


namespace Game.Interactions
{
    public class Pickup : MonoBehaviour
    {
        [SerializeField] private GameObject spawnPoint;
        [SerializeField] private Transform baseParent;
        [SerializeField] private Sprite itemIcon;
        [SerializeField] private bool respawn;
        [SerializeField] private bool cleaned;
        public UnityEvent onPickUp;
        public UnityEvent onDrop;
        
        [HideInInspector]public PlayerInteractionController player;
        public bool isInHand = false;
        private GameObject _interactionTarget;

        private Outline _outline;
        private bool _currentPlayer;
        private Rigidbody _rigidbody;

        private void Start()
        {
            _interactionTarget = gameObject;
            _rigidbody = _interactionTarget.GetComponent<Rigidbody>();
            _outline = GetComponentInChildren<Outline>();
        }

        public void PickUp()
        {
            if (_outline)
            {
                if (_outline.roomTarget)
                {
                    if (!_outline.roomTarget.RoomCleared) return;
                }
            }
            if (isInHand)
            {
                CancelPickUp();
                return;
            }
            //player.playerInvImage.sprite = itemIcon;
            //player.playerInvImage.gameObject.SetActive(true);
            player.currentItem = this;
            _rigidbody.useGravity = false;
            _interactionTarget.transform.position = player.handGrabPosition.position;
            _interactionTarget.transform.parent = player.handGrabPosition;
            isInHand = true;
            onPickUp.Invoke();
        }

        private void CancelPickUp()
        {
           // player.playerInvImage.sprite = null;
           // player.playerInvImage.gameObject.SetActive(false);
            _interactionTarget.transform.parent = baseParent;
            player.currentItem = null;
            _rigidbody.useGravity = true;
            isInHand = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Untagged")) return;
            if (_outline != null)
            {
                if (_outline.roomTarget)
                {
                    if (!_outline.roomTarget.RoomCleared) return;
                }
            }
            if (other.CompareTag("PickupDest") && !isInHand)
            {
                if (respawn)
                {
                    ResetPosition();
                }
                if(other.gameObject.GetComponent<ItemCollector>())other.gameObject.GetComponent<ItemCollector>().InsertItem(this);
                onDrop.Invoke();
            }

            if (other.CompareTag("Storage") && !isInHand)
            {
                if (respawn)
                {
                    ResetPosition();
                }
                if (other.gameObject.GetComponent<StoreInteraction>().isQuestStorage)
                {
                    if(!cleaned) return;
                }
                other.gameObject.GetComponent<StoreInteraction>().AddObject(this);
                gameObject.SetActive(false);
            }
        }

        public void ResetPosition()
        {
            _interactionTarget.transform.position = spawnPoint.transform.position;
            _interactionTarget.transform.rotation = spawnPoint.transform.rotation;
        }
    }
}
