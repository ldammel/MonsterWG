using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Character
{
    public class CharacterMovement : MonoBehaviour
    {
        #region Variables
        [SerializeField] private float movementSpeed = 2.0f;
        private InputMaster _controls = null;
        #endregion

        #region Event Functions
        private void Awake() => _controls = new InputMaster();
        private void OnEnable() => _controls.Player.Enable();
        private void OnDisable() => _controls.Player.Disable();
        private void Update() => Move();
        #endregion
        
        #region Public Functions
        public void Move()
        {
            var movementInput = _controls.Player.Move.ReadValue<Vector2>();
            var movement = new Vector3
            {
                x = movementInput.x,
                z = movementInput.y
            }.normalized;

            transform.Translate(Time.deltaTime * movementSpeed * movement);

        }

        public void Interact()
        {
            //do stuff
        }

        #endregion
    }
}
