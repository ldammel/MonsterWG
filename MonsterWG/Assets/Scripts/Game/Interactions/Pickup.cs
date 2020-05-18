using System;
using System.Runtime.CompilerServices;
using Game.Character;
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
        public UnityEvent onPickUp;
        public UnityEvent onDrop;
        
        private PlayerInteractionController _player;
        private bool _isInHand = false;
        private GameObject _interactionTarget;

        private bool _currentPlayer;

        private void Start()
        {
            _interactionTarget = gameObject;
        }

        private void PickUp()
        {
            if (_player == null) return;
            if (_currentPlayer != _player.isPlayerOne) return;
            if (_isInHand)
            {
                CancelPickUp();
                return;
            }
            _player.playerInvImage.sprite = itemIcon;
            _player.playerInvImage.gameObject.SetActive(true);
            _interactionTarget.GetComponent<Rigidbody>().useGravity = false;
            _interactionTarget.transform.position = _player.handGrabPosition.position;
            _interactionTarget.transform.parent = _player.handGrabPosition;
            _isInHand = true;
            onPickUp.Invoke();
            Debug.Log("Picked up Item");
        }

        private void CancelPickUp()
        {
            _player.playerInvImage.sprite = null;
            _player.playerInvImage.gameObject.SetActive(false);
            _interactionTarget.GetComponent<Rigidbody>().useGravity = true;
            _interactionTarget.transform.parent = baseParent;
            _isInHand = false;
            Debug.Log("Dropped Item");
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Untagged")) return;
            if (_player != null) return;
            _player = other.GetComponent<PlayerInteractionController>();
            if (_player.isPlayerOne)
            { 
                _player.character.controls.Player.Select.Enable();
                _player.character.controls.Player.Select.performed += _ => PickUp();
                _currentPlayer = true;
            }
            else
            {
                _player.character.controls.Player2.Select.Enable();
                _player.character.controls.Player2.Select.performed += _ => PickUp();
                _currentPlayer = false;
            }
            if (other.CompareTag("PickupDest") && !_isInHand)
            {
                if (respawn)
                {
                    ResetPosition();
                }
                other.gameObject.GetComponent<ItemCollector>().collectedItems.Add(this);
                onDrop.Invoke();
            }

            if (other.CompareTag("Storage") && !_isInHand)
            {
                if (!other.gameObject.GetComponent<StoreInteraction>().storedObjects.Contains(gameObject))
                {
                    other.gameObject.GetComponent<StoreInteraction>().AddObject(gameObject);
                    gameObject.SetActive(false);
                }
            }            
            _player.interactImage.SetActive(true);
            _player.isInTrigger = true;
            Debug.Log("Registered Player");
        }

        public void ResetPosition()
        {
            _interactionTarget.transform.position = spawnPoint.transform.position;
            _interactionTarget.transform.rotation = spawnPoint.transform.rotation;
        }

        private void OnTriggerExit(Collider other)
        {
            if (_isInHand) return;
            if (_player == null) return;
            _player.character.controls.Player.Select.Disable();
            _player.character.controls.Player2.Select.Disable();
            _player.interactImage.SetActive(false);
            _player.isInTrigger = false;
            _player = null;
            Debug.Log("Unregistered Player");
        }
    }
}
