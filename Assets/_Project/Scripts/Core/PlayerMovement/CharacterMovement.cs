using UnityEngine;

namespace _Project.Scripts.Core.PlayerMovement
{
    public class CharacterMovement : PlayerInputHandler
    {
        public float moveSpeed = 5f;
        private CharacterController _characterController;
        Vector3 _currentMovement;
        bool _isMovementPressed;
        readonly float _rotationFactorPerFrame = 10.0f;

        protected override void Awake()
        {
            base.Awake();
            // Get and store the CharacterController component attached to the player
            _characterController = GetComponent<CharacterController>();
        }
        
        void Update()
        {
            HandleRotation();
        }

        protected override void ProcessMovement(float horizontal, float vertical)
        {
            // Assign horizontal and vertical inputs to the movement vector
            _currentMovement.x = horizontal;
            _currentMovement.z = vertical;
            
            // Check if any movement input is being pressed
            _isMovementPressed = _currentMovement != Vector3.zero;
            
            // If movement input is detected, move the character using CharacterController
            if (_isMovementPressed)
            {
                _characterController.Move(_currentMovement * (moveSpeed * Time.deltaTime));
            }
        }
        
        void HandleRotation()
        {
            Vector3 positionToLookAt;
            positionToLookAt.x = _currentMovement.x;
            positionToLookAt.y = 0.0f;
            positionToLookAt.z = _currentMovement.z;
            Quaternion currentRotation = transform.rotation;
            
            // If movement input is pressed, rotate the character to face the movement direction
            if (_isMovementPressed)
            {
                // Calculate the target rotation based on the movement direction
                Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
                
                // Smoothly interpolate the character's rotation towards the target rotation
                transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, _rotationFactorPerFrame * Time.deltaTime);
            }
        
        }
    }
}
