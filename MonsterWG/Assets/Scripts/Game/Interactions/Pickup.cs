using System;
using System.Collections;
using Game.Character;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Game.Interactions
{
    public class Pickup : BaseInteraction
    {
        [SerializeField] private Transform handGrabPosition;
        [SerializeField] private Transform placementPosition;
        [SerializeField] private GameObject interactImage;
        public bool _isInHand;
        private bool _isInTrigger;
        private Transform _baseParent;

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
            interactionTarget.transform.position = handGrabPosition.position;
            interactionTarget.transform.parent = handGrabPosition;
        }

        private void PutDown()
        {
            interactionTarget.GetComponent<Rigidbody>().useGravity = true;
            interactionTarget.transform.position = placementPosition.position;
            interactionTarget.transform.parent = _baseParent;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player")) return;
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
