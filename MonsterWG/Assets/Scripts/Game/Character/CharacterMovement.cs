using System;
using Game.AI;
using UnityEngine;
using StateMachineBehaviour = Game.AI.StateMachineBehaviour;

namespace Game.Character
{
    public class CharacterMovement : MonoBehaviour
    {
        #region Variables
        [SerializeField] private float movementSpeed = 2.0f;
        [SerializeField] private bool playerOne;
        [SerializeField] private GameObject playerModel;
        [SerializeField] private NavMeshWalker mom;
        [SerializeField]private StateMachineBehaviour behaviour;
        [SerializeField] private GameObject scoreScreen1;
        
        public InputMaster controls = null;
        public bool canMove = true;
        
        private State _calledState;
        #endregion

        #region Event Functions
        private void Awake() => controls = new InputMaster();

        private void OnEnable()
        {
            if(playerOne)controls.Player.Enable();
            else controls.Player2.Enable();
            canMove = true;
        }

        private void OnDisable()
        {
            if(playerOne)controls.Player.Disable();
            else controls.Player2.Disable();
        }

        private void Update()
        {
            if (mom.FinalWayPoint || scoreScreen1.activeSelf) return;
            Move();
            if(Input.GetKeyDown(KeyCode.M)) NavMeshWalker.instance.StartMiniQuest();
            if (Input.GetKeyDown(playerOne? KeyCode.V : KeyCode.N))
            {
                CallMom();
            }
        }
        #endregion

        #region Public Functions
        public void Move()
        {
            if (!canMove) return;
            var movementInput = playerOne ? controls.Player.Move.ReadValue<Vector2>() : controls.Player2.Move.ReadValue<Vector2>();
            var movement = new Vector3
            {
                x = movementInput.x,
                z = movementInput.y
            }.normalized;
            
            if (movement != Vector3.zero) {
                playerModel.transform.rotation = Quaternion.Slerp(playerModel.transform.rotation, Quaternion.LookRotation(movement), 0.15F);
            }

            transform.Translate(Time.deltaTime * movementSpeed * movement);
        }

        public void CallMom()
        {
            behaviour.Call(transform, 3);
        }

        #endregion
    }
}
