﻿using System.Collections.Generic;
using Game.Character;
using UnityEngine;

namespace Game.Interactions
{
    public class PlayerInteractionController : MonoBehaviour
    {
        public Transform handGrabPosition;
        public bool isPlayerOne;
        public CharacterMovement character;


        public Pickup CurrentItem;
        public float InputInteraction { get; private set; }
        public float InputPickUp { get; private set; }
        public float InputMenu { get; private set; }
        public float InputCall { get; private set; }

        private bool _plan;
        private bool _pressedActivation;
        public bool StoreInteraction;
        private ActivatePlan _activatePlan;
        private StoreInteraction _storeInteractionObject;
        private bool _pressedPickup;
        public List<Interaction> _interactions = new List<Interaction>();
        private readonly List<OnActivation> _activations = new List<OnActivation>();

        private void Update()
        {
            InputInteraction = isPlayerOne
                ? character.controls.Player.Interact.ReadValue<float>()
                : character.controls.Player2.Interact.ReadValue<float>();
            InputPickUp = isPlayerOne
                ? character.controls.Player.Select.ReadValue<float>()
                : character.controls.Player2.Select.ReadValue<float>();
            InputMenu = isPlayerOne
                ? character.controls.Player.Menu.ReadValue<float>()
                : character.controls.Player2.Menu.ReadValue<float>();
            InputCall = isPlayerOne
                ? character.controls.Player.Notify.ReadValue<float>()
                : character.controls.Player2.Notify.ReadValue<float>();
            
            Interact(InputInteraction);
            Pickups(InputPickUp);
            Activation(InputInteraction);
        }
        
        public void Interact(float input)
        {
            if (_interactions.Count < 1) return;
            if (input >= 1f && _interactions[0].gameObject.activeSelf)
            {
                _interactions[0].Interact();
            }
            else if (!_interactions[0].Stop)
            {
                _interactions[0].Cancel();
            }
        }
        
        public void Pickups(float input)
        {
            if (input >= 1f && !_pressedPickup)
            {
                _pressedPickup = true;

                if (StoreInteraction && CurrentItem)
                {
                    _storeInteractionObject.AddObject(CurrentItem.gameObject);
                    return;
                }
                
                if (_plan)
                {
                    _activatePlan.Toggle();
                    return;
                }
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
                if (_activations.Count < 1) return;
                if (!_activations[0].gameObject.activeSelf) return;
                _activations[0].player = this;
                _activations[0]?.PickUp();
                _pressedActivation = true;
            }
            else if (input < 1f)
            {
                _pressedActivation = false;
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.GetComponent<Interaction>() && other.gameObject.activeSelf)
            {
                if (!_interactions.Contains(other.GetComponent<Interaction>()))
                {
                    _interactions.Add(other.GetComponent<Interaction>());
                    other.GetComponent<Interaction>().player = this;
                }
            }
            if (other.GetComponent<OnActivation>())
            {
                if (!_activations.Contains(other.GetComponent<OnActivation>()) && other.gameObject.activeSelf)
                {
                    _activations.Add(other.GetComponent<OnActivation>());
                    other.GetComponent<OnActivation>().player = this;
                }
            }
            
            if (other.GetComponent<StoreInteraction>())
            {
                if (other.gameObject.activeSelf)
                {
                    StoreInteraction = true;
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
            if (_interactions.Contains(other.GetComponent<Interaction>()))
            {
                _interactions.Remove(other.GetComponent<Interaction>());
                other.GetComponent<Interaction>().player = null;
            }
            
            if (_activations.Contains(other.GetComponent<OnActivation>()))
            {
                _activations.Remove(other.GetComponent<OnActivation>());
            }

            if (StoreInteraction)
            {
                StoreInteraction = false;
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
