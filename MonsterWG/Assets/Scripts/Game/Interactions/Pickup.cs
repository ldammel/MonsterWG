using System;
using System.Collections;
using Game.Character;
using Game.UI;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Game.Interactions
{
    public class Pickup : BaseInteraction
    {
        private Transform _handGrabPosition;
        [SerializeField] private Transform handGrabPosition1;
        [SerializeField] private Transform handGrabPosition2;
        [SerializeField] private GameObject interactImage;
        [SerializeField] private GameObject spawnPoint;
        public bool _isInHand;
        private bool _isInTrigger;
        private Transform _baseParent;
        [SerializeField]private CharacterMovement character;
        [SerializeField]private CharacterMovement character2;

        private void Start()
        {
            character.controls.Player.Interact.performed += _ => Interact();
            character2.controls.Player1.Interact.performed += _ => Interact();
        }

        public override void Interact()
        {
            if (!_isInTrigger && !_isInHand) return;
            Debug.Log("Pressed");
            if (!_isInHand)
            {
                PickupObject();
            }
            else
            {
                PutDown();
            }

            _isInHand = !_isInHand;
        }

        private void PickupObject()
        {
            _baseParent = interactionTarget.transform.parent;
            interactionTarget.GetComponent<Rigidbody>().useGravity = false;
            interactionTarget.transform.position = _handGrabPosition.position;
            interactionTarget.transform.parent = _handGrabPosition;
        }

        private void PutDown()
        {
            interactionTarget.GetComponent<Rigidbody>().useGravity = true;
            interactionTarget.transform.parent = _baseParent;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("PickupDest") && !_isInHand)
            {
                interactionTarget.transform.position = spawnPoint.transform.position;
                interactionTarget.transform.rotation = spawnPoint.transform.rotation;
                ScoreDisplay.instance.AddScore(10000);
            }

            if (other.CompareTag("Untagged")) return;
            
            _handGrabPosition = other.CompareTag("Player2") ? handGrabPosition2 : handGrabPosition1;
            interactImage.SetActive(true);
            _isInTrigger = true;
        }
        private void OnTriggerExit(Collider other)
        {
            interactImage.SetActive(false);
            _isInTrigger = false;
        }
    }
}
