using UnityEngine;
using UnityEngine.InputSystem;

namespace _Project.Scripts.Core.PlayerMovement
{
    public abstract class PlayerInputHandler : MonoBehaviour
    {
        private Vector2 MoveInput { get; set; }
        private PlayerInput _playerInput;
        private bool IsMoving { get; set; }

        protected virtual void Awake()
        {
            // Initialize PlayerInput from Unity's New Input System
            _playerInput = new PlayerInput();
        }

        protected virtual void OnEnable()
        {
            _playerInput.PlayerControl.Enable();

            // Subscribe to input events
            _playerInput.PlayerControl.Move.started += OnMovementInputStarted;
            _playerInput.PlayerControl.Move.performed += OnMovementInputPerformed;
            _playerInput.PlayerControl.Move.canceled += OnMovementInputCancelled;
        }

        protected virtual void OnDisable()
        {
            // Unsubscribe from input events
            _playerInput.PlayerControl.Move.started -= OnMovementInputStarted;
            _playerInput.PlayerControl.Move.performed -= OnMovementInputPerformed;
            _playerInput.PlayerControl.Move.canceled -= OnMovementInputCancelled;

            _playerInput.PlayerControl.Disable();
        }

        // Input Event Handlers
        private void OnMovementInputStarted(InputAction.CallbackContext context)
        {
            MoveInput = context.ReadValue<Vector2>();
            IsMoving = true;
        }

        private void OnMovementInputPerformed(InputAction.CallbackContext context)
        {
            MoveInput = context.ReadValue<Vector2>();
        }

        private void OnMovementInputCancelled(InputAction.CallbackContext context)
        {
            MoveInput = Vector2.zero;
            IsMoving = false;
        }

        // Abstract method for processing movement input
        protected abstract void ProcessMovement(float horizontal, float vertical);

        // method for PlayerController that inherit PlayerInputHandler
        public virtual void HandleMovementInput()
        {
            if (IsMoving)
            {
                ProcessMovement(MoveInput.x, MoveInput.y);
            }
        }
    }
}
