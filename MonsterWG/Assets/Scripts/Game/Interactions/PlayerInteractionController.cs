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
        
        private Animator _animator;
        public CharacterMovement character;

        private void Start()
        {
            character = GetComponent<CharacterMovement>();
            _animator = GetComponent<Animator>();
        }

    }
}
