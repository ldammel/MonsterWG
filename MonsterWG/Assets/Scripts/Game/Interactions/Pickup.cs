﻿using System;
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
        
        [HideInInspector]public PlayerInteractionController player;
        public bool isInHand = false;
        private GameObject _interactionTarget;

        private bool _currentPlayer;
        private Rigidbody _rigidbody;

        private void Start()
        {
            _interactionTarget = gameObject;
            _rigidbody = _interactionTarget.GetComponent<Rigidbody>();
        }

        public void PickUp()
        {
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
            if (other.CompareTag("PickupDest") && !isInHand)
            {
                if (respawn)
                {
                    ResetPosition();
                }
                other.gameObject.GetComponent<ItemCollector>().collectedItems.Add(this);
                onDrop.Invoke();
            }

            if (other.CompareTag("Storage") && !isInHand)
            {
                if (!other.gameObject.GetComponent<StoreInteraction>().storedObjects.Contains(gameObject))
                {
                    other.gameObject.GetComponent<StoreInteraction>().AddObject(gameObject);
                    gameObject.SetActive(false);
                }
            }
        }

        public void ResetPosition()
        {
            _interactionTarget.transform.position = spawnPoint.transform.position;
            _interactionTarget.transform.rotation = spawnPoint.transform.rotation;
        }
    }
}
