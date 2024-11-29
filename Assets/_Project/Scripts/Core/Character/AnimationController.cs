using _Project.Scripts.Core.Player_Controllers.Input_Controllers;
using UnityEngine;

namespace _Project.Scripts.Core.Character
{
    public class AnimationController: CharacterComponent
    {
        private static readonly int Horizontal = Animator.StringToHash("DirectionX");
        private static readonly int Vertical = Animator.StringToHash("DirectionZ");
        private static readonly int IsFighting = Animator.StringToHash("IsFighting");
        [SerializeField] private Animator animator;
        private InputController _inputController;
        private void Awake()
        {
            _inputController = GetComponent<InputController>();
        }
        private void OnEnable()
        {
            // Subscribe to events
            if (!_inputController) return;
            _inputController.OnMoveInputUpdated += OnMoveDetected; 
            _inputController.OnAttackInputBegan += OnAttackDetected;
        }

        /// <summary>
        /// Unsubscribe from events to avoid memory leaks
        /// </summary>
        private void OnDisable()
        {
            // Unsubscribe from events to avoid memory leaks
            if (!_inputController) return;
            _inputController.OnMoveInputUpdated -= OnMoveDetected;
            _inputController.OnAttackInputBegan -= OnAttackDetected;
        }
        
        private void OnMoveDetected(Vector2 moveInput)
        {
            SetMovementAnimation(moveInput);
        }
        private void OnAttackDetected()
        {
            SetMeleeAttackAnimation(true);
        }
        private void SetMovementAnimation(Vector2 moveInput)
        {
            //Set Movement Blend Tree Parameters
            animator.SetFloat(Horizontal, moveInput.x, 0.1f, Time.deltaTime);
            animator.SetFloat(Vertical, moveInput.y, 0.1f, Time.deltaTime);
        }
        private void SetMeleeAttackAnimation(bool isFighting)
        {
            animator.SetBool(IsFighting, isFighting);
        }
    }
}
