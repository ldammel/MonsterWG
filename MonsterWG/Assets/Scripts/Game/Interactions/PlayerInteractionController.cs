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
        
        public Pickup currentItem;
        private bool _plan;
        private ActivatePlan _activatePlan;
        public float inputI;
        public float inputS;

        private void Update()
        {
            inputI = isPlayerOne ? character.controls.Player.Interact.ReadValue<float>() : character.controls.Player2.Interact.ReadValue<float>();
            inputS = isPlayerOne ? character.controls.Player.Select.ReadValue<float>() : character.controls.Player2.Select.ReadValue<float>();
        }

        private void OnTriggerEnter(Collider other)
        {

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

            if (_plan)
            {
                _plan = false;
                _activatePlan.player = null;
                _activatePlan = null;
            }

        } 
    }
}
