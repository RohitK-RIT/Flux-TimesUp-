using _Project.Scripts.Core.Player_Controllers.Input_Controllers;
using UnityEngine;

namespace _Project.Scripts.Core.Player_Controllers
{
    /// <summary>
    /// This class is responsible for handling the player's input.
    /// </summary>
    [RequireComponent(typeof(LocalInputController), typeof(CameraController))]
    public class LocalPlayerController : PlayerController
    {
        /// <summary>
        /// Component that handles player input.
        /// </summary>
        private LocalInputController _localInputController;

        private CameraController _cameraController;

        // This will go in player info eventually.
        [SerializeField] private float aimSensitivity = 1f;

        protected override void Awake()
        {
            base.Awake();

            _localInputController = GetComponent<LocalInputController>();
            _cameraController = GetComponent<CameraController>();

            _localInputController.Initialize(this);
            _cameraController.Initialize(this);
        }

        private void OnEnable()
        {
            // Subscribe to input events
            _localInputController.OnMoveInputUpdated += SetMoveInput;

            _localInputController.OnAttackInputBegan += BeginAttack;
            _localInputController.OnAttackInputEnded += EndAttack;

            _localInputController.OnLookInputUpdated += SetLookInput;
            
            // Subscribe to ability events
            _localInputController.OnAbilityEquipped += AbilityEquipped;
        }

        private void OnDisable()
        {
            // Unsubscribe from input events
            _localInputController.OnMoveInputUpdated -= SetMoveInput;

            _localInputController.OnAttackInputBegan -= BeginAttack;
            _localInputController.OnAttackInputEnded -= EndAttack;

            _localInputController.OnLookInputUpdated -= SetLookInput;
            
            // Unsubscribe from ability events
            _localInputController.OnAbilityEquipped -= AbilityEquipped;
        }

        /// <summary>
        /// Update the player's look direction.
        /// </summary>
        /// <param name="lookInput"></param>
        private void SetLookInput(Vector2 lookInput)
        {
            _cameraController.LookInput = lookInput * aimSensitivity;
        }

#if UNITY_EDITOR
        //to be removed
        [ContextMenu("Take Damage")]
        public void TakeDamage()
        {
            TakeDamage(10);
        }

        [ContextMenu("Heal")]
        public void Heal()
        {
            Heal(10);
        }
#endif
    }
}