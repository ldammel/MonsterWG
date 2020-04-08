using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Character
{
    public class CharacterMovement : MonoBehaviour
    {
        #region Variables
        [SerializeField] private float movementSpeed = 2.0f;
        public InputMaster controls = null;
        #endregion

        #region Event Functions
        private void Awake() => controls = new InputMaster();
        private void OnEnable() => controls.Player.Enable();
        private void OnDisable() => controls.Player.Disable();
        private void Update() => Move();
        #endregion
        
        #region Public Functions
        public void Move()
        {
            var movementInput = controls.Player.Move.ReadValue<Vector2>();
            var movement = new Vector3
            {
                x = movementInput.x,
                z = movementInput.y
            }.normalized;

            transform.Translate(Time.deltaTime * movementSpeed * movement);

        }
        #endregion
    }
}
