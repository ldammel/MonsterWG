using Game.Interactions;
using UnityEngine;

namespace Game.Utility
{
    public class DisableMovement : MonoBehaviour
    {

        private PlayerInteractionController _move1;
        private PlayerInteractionController _move2;
        void Start()
        {
            _move1 = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<PlayerInteractionController>();
            _move2 = GameObject.FindGameObjectWithTag("Player2").GetComponentInChildren<PlayerInteractionController>();
        }

        public void StopMovement()
        {
            _move1.character.canMove = false;
            _move2.character.canMove = false;
        }
    }
}
