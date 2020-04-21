using Game.Character;
using UnityEngine;

namespace Game.Interactions
{
    public class BaseInteraction : MonoBehaviour
    {
        public GameObject interactionTarget;
        public Animator animator;
        public GameObject interactImage;
        public CharacterMovement _character;
        public CharacterMovement _character2;
        public bool _isInTrigger = false;
        
        private void Start()
        {
            _character = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterMovement>();
            _character2 = GameObject.FindGameObjectWithTag("Player2").GetComponent<CharacterMovement>();
            _character.controls.Player.Interact.performed += _ => Interact();
            _character2.controls.Player1.Interact.performed += _ => Interact();
        }

        public virtual void Interact(){}
        
    }
}
