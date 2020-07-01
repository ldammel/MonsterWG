using System;
using System.Collections.Generic;
using Game.Character;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Interactions
{
    public class PlayerInteractionController : MonoBehaviour
    {
        public Transform handGrabPosition;
        public bool isPlayerOne;
        public CharacterMovement character;

        public List<Interaction> interactions;
        public List<OnActivation> activations;
        public List<Pickup> pickups;

        public Pickup currentItem;
        private bool _plan;
        private bool _pressedActivation;
        private bool _storeInteraction;
        private ActivatePlan _activatePlan;
        private StoreInteraction _storeInteractionObject;
        public float inputI;
        public float inputS;
        private bool _pressedPickup;

        private void Update()
        {
            inputI = isPlayerOne
                ? character.controls.Player.Interact.ReadValue<float>()
                : character.controls.Player2.Interact.ReadValue<float>();
            inputS = isPlayerOne
                ? character.controls.Player.Select.ReadValue<float>()
                : character.controls.Player2.Select.ReadValue<float>();
            
            Interact(inputI);
            Pickups(inputS);
            Activation(inputI);
        }
        
        public void Interact(float input)
        {
            if (interactions.Count < 1) return;
            if (input >= 1f && interactions[0].gameObject.activeSelf)
            {
                interactions[0].Interact();
            }
            else if (!interactions[0].Stop)
            {
                interactions[0].Cancel();
            }
        }
        
        public void Pickups(float input)
        {
            if (input >= 1f && !_pressedPickup)
            {
                _pressedPickup = true;

                if (_storeInteraction && currentItem)
                {
                    _storeInteractionObject.AddObject(currentItem);
                    return;
                }
                
                if (_plan)
                {
                    _activatePlan.Toggle();
                    return;
                }

                /*if (pickups.Count < 1) return;
                if (!pickups[0].gameObject.activeSelf) return;
                if (currentItem != pickups[0] && currentItem) return;
                pickups[0].player = this;
                pickups[0].PickUp();*/
            }
            else if(input < 1f)
            {
                _pressedPickup = false;
            }
        }

        public void Activation(float input)
        {
            if (input >= 1f && !_pressedActivation)
            {
                if (activations.Count < 1) return;
                if (!activations[0].gameObject.activeSelf) return;
                activations[0].player = this;
                activations[0]?.PickUp();
                _pressedActivation = true;
            }
            else if (input < 1f)
            {
                _pressedActivation = false;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Interaction>() && other.gameObject.activeSelf)
            {
                if (!interactions.Contains(other.GetComponent<Interaction>()))
                {
                    interactions.Add(other.GetComponent<Interaction>());
                    other.GetComponent<Interaction>().player = this;
                }
            }
            if (other.GetComponent<OnActivation>())
            {
                if (!activations.Contains(other.GetComponent<OnActivation>()) && other.gameObject.activeSelf)
                {
                    activations.Add(other.GetComponent<OnActivation>());
                    other.GetComponent<OnActivation>().player = this;
                }
            }
            
            //if (other.GetComponent<Pickup>())
            //{
            //    if (!pickups.Contains(other.GetComponent<Pickup>()) && other.gameObject.activeSelf)
            //    {
            //        pickups.Add(other.GetComponent<Pickup>());
            //        other.GetComponent<Pickup>().player = this;
            //    }
            //}
            
            if (other.GetComponent<StoreInteraction>())
            {
                if (other.gameObject.activeSelf)
                {
                    _storeInteraction = true;
                    _storeInteractionObject = other.GetComponent<StoreInteraction>();
                }
            }
            
            if (other.GetComponent<ActivatePlan>())
            {
                if (other.gameObject.activeSelf)
                {
                    _plan = true;
                    _activatePlan = other.GetComponent<ActivatePlan>();
                    _activatePlan.player = this;
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
            
            if (activations.Contains(other.GetComponent<OnActivation>()))
            {
                activations.Remove(other.GetComponent<OnActivation>());
            }
            
           //if (pickups.Contains(other.GetComponent<Pickup>()))
           //{
           //    pickups.Remove(other.GetComponent<Pickup>());
           //    other.GetComponent<Pickup>().player = null;
           //}

            if (_storeInteraction)
            {
                _storeInteraction = false;
                _storeInteractionObject = null;
            }
            
            if (_plan)
            {
                _plan = false;
                _activatePlan.player = null;
                _activatePlan = null;
            }

        } 
    }
}
