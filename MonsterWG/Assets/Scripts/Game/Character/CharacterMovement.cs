using System;
using UnityEngine;

namespace Game.Character
{
    public class CharacterMovement : MonoBehaviour
    {
        #region Variables
        [SerializeField] private float movementSpeed = 2.0f;
        [SerializeField] private bool playerOne;
        [SerializeField] private GameObject playerModel;
        private Vector3 _forwardAxis, _rightAxis;

        public InputMaster controls = null;
        public bool canMove = true;
        private Rigidbody _rb;
        
        #endregion

        #region Event Functions
        private void Awake() => controls = new InputMaster();

        private void OnEnable()
        {
            if(playerOne)controls.Player.Enable();
            else controls.Player2.Enable();
            canMove = true;
        }

        private void Start()
        {
            _rb = GetComponent<Rigidbody>();
            Transform cameraAlign = Camera.main.transform;
            Vector3 forward = cameraAlign.forward;
            _forwardAxis = new Vector3(forward.x, 0, forward.z).normalized;
            _rightAxis = Quaternion.Euler(0, 90, 0) * _forwardAxis;
        }

        private void OnDisable()
        {
            if(playerOne)controls.Player.Disable();
            else controls.Player2.Disable();
        }

        private void Update()
        {
            Move();
            _rb.velocity = Vector3.zero;
        }
        #endregion

        #region Public Functions
        public void Move()
        {
            if (!canMove) return;
            Vector2 movementInput = playerOne ? controls.Player.Move.ReadValue<Vector2>() : controls.Player2.Move.ReadValue<Vector2>();
            
            Vector3 rightMovement = movementSpeed * Time.deltaTime * movementInput.x * _rightAxis;
            Vector3 upMovement = movementSpeed * Time.deltaTime * movementInput.y * _forwardAxis;

            Vector3 heading = Vector3.Normalize(rightMovement + upMovement);
            Vector3 position = transform.position;
            Debug.DrawRay(position, heading * movementSpeed, Color.red);
            transform.forward = Vector3.Slerp(transform.forward, heading, 0.15f);
            position += rightMovement;
            position += upMovement;
            transform.position = position;
        }

        #endregion
    }
    
}
