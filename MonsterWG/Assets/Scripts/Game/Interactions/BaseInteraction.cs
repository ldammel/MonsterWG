using System.Collections.Generic;
using Game.Character;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Interactions
{
    public class BaseInteraction : MonoBehaviour
    {
        public UnityEvent onComplete;
        public Animator animator;
        public GameObject interactImage;
        [HideInInspector]public CharacterMovement character;
        [HideInInspector]public CharacterMovement character2;
        [HideInInspector]public bool isInTrigger = false;
        public int scoreGain = 10000;
        [HideInInspector]public bool isPlayerOne;
        
        private void Start()
        {
            character = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterMovement>();
            character2 = GameObject.FindGameObjectWithTag("Player2").GetComponent<CharacterMovement>();
            
            character.controls.Player.Interact.performed += _ => Interact(1);
            character2.controls.Player1.Interact.performed += _ => Interact(2);
            
            character.controls.Player.Select.performed += _ => PickUp(1);
            character2.controls.Player1.Select.performed += _ => PickUp(2);
            
            character.controls.Player.Interact.canceled += _ => Cancel(1);
            character2.controls.Player1.Interact.canceled += _ => Cancel(2);
        }

        public virtual void Interact(int player){}
        public virtual void PickUp(int player){}
        public virtual void Cancel(int player){}
        
        
    }
}
