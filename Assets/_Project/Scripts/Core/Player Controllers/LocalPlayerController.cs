using _Project.Scripts.Core.Player_Controllers.Input_Controllers;
using UnityEngine;

namespace _Project.Scripts.Core.Player_Controllers
{
    /// <summary>
    /// This class is responsible for handling the player's input.
    /// </summary>
    [RequireComponent(typeof(LocalInputController))]
    public class LocalPlayerController : PlayerController
    {
        /// <summary>
        /// Component that handles player input.
        /// </summary>
        private LocalInputController _localInputController;
        
        // This will go in player info eventually.
        private const float AimSensitivity = 0.1f;

        protected override void Awake()
        {
            base.Awake();
            _localInputController = GetComponent<LocalInputController>();
        }

        private void OnEnable()
        {
            // Subscribe to input events
            _localInputController.OnMoveInputUpdated += UpdateMoveDirection;

            _localInputController.OnAttackInputBegan += BeginAttack;
            _localInputController.OnAttackInputEnded += EndAttack;

            _localInputController.OnLookInputUpdated += LookInputUpdated;
        }

        private void OnDisable()
        {
            // Unsubscribe from input events
            _localInputController.OnMoveInputUpdated -= UpdateMoveDirection;

            _localInputController.OnAttackInputBegan -= BeginAttack;
            _localInputController.OnAttackInputEnded -= EndAttack;

            _localInputController.OnLookInputUpdated -= LookInputUpdated;
        }

        /// <summary>
        /// Update the player's look direction.
        /// </summary>
        /// <param name="lookInput"></param>
        protected override void LookInputUpdated(Vector2 lookInput)
        {
            MovementController.lookDirection += lookInput * AimSensitivity;
        }
    }
}