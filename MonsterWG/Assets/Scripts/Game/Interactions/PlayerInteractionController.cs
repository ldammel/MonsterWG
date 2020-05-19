using System;
using System.Collections.Generic;
using Game.Character;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Interactions
{
    public class PlayerInteractionController : MonoBehaviour
    {
        public GameObject interactImage;
        public Image playerInvImage;
        public Transform handGrabPosition;
        [HideInInspector]public bool isInTrigger = false;
        public bool isPlayerOne;
        public CharacterMovement character;
        public Animator animator;

        public List<Interaction> interactions;
        public List<Pickup> pickups;
        public List<OnActivation> activations;

        private bool _pressed;
        public Pickup currentItem;

        private void Update()
        {
            Interact();
            Pickups();
        }

        public void Interact()
        {
            var input = isPlayerOne ? character.controls.Player.Interact.ReadValue<float>() : character.controls.Player2.Interact.ReadValue<float>();
            if (interactions.Count < 1) return;
            if (input >= 1f)
            {
                interactions[0].Interact();
            }
            else if (!interactions[0].Stop)
            {
                interactions[0].Cancel();
            }
        }

        public void Pickups()
        {
            var input = isPlayerOne ? character.controls.Player.Select.ReadValue<float>() : character.controls.Player2.Select.ReadValue<float>();
            if(activations.Count >= 1) activations[0].PickUp();
            if (pickups.Count < 1) return;
            if (input >= 1f && !_pressed)
            {
                if (currentItem != pickups[0] && currentItem) return;
                _pressed = true;
                pickups[0].PickUp();
                pickups[0].player = this;
            }
            else if (input < 1f)
            {
                _pressed = false;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
             if (other.GetComponent<Interaction>())
             {
                 if (!interactions.Contains(other.GetComponent<Interaction>()))
                 {
                     interactions.Add(other.GetComponent<Interaction>());
                     other.GetComponent<Interaction>().player = this;
                 }
             }
             if (other.GetComponent<Pickup>())
             {
                 if (!pickups.Contains(other.GetComponent<Pickup>()))
                 {
                     pickups.Add(other.GetComponent<Pickup>());
                     other.GetComponent<Pickup>().player = this;
                 }
                 if (!activations.Contains(other.GetComponent<OnActivation>()))
                 {
                     activations.Add(other.GetComponent<OnActivation>());
                 }
             }
             if (other.GetComponent<OnActivation>())
             {
                 if (!activations.Contains(other.GetComponent<OnActivation>()))
                 {
                     activations.Add(other.GetComponent<OnActivation>());
                 }
             }
        }
        private void OnTriggerExit(Collider other)
        {
            if (interactions.Contains(other.GetComponent<Interaction>()))
            {
                interactions.Remove(other.GetComponent<Interaction>());
                other.GetComponent<Interaction>().player = null;
            }

            if (pickups.Contains(other.GetComponent<Pickup>()))
            {
                pickups.Remove(other.GetComponent<Pickup>());
                other.GetComponent<Pickup>().player = null;
            }
            
            if (activations.Contains(other.GetComponent<OnActivation>()))
            {
                activations.Remove(other.GetComponent<OnActivation>());
            }
        } 
    }
}
