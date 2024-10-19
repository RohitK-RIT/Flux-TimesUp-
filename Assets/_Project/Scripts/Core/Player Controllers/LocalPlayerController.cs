using _Project.Scripts.Core.Player_Controllers.Input_Controllers;
using UnityEngine;

namespace _Project.Scripts.Core.Player_Controllers
{
    /// <summary>
    /// This class is responsible for handling the player's input.
    /// </summary>
    public class LocalPlayerController : PlayerController
    {
        /// <summary>
        /// Component that handles player input.
        /// </summary>
        private LocalInputController _localInputController;

        protected override void Awake()
        {
            base.Awake();
            _localInputController = new LocalInputController();
        }

        private void OnEnable()
        {
            // Enable the LocalInputController
            _localInputController.Enable();

            // Subscribe to input events
            _localInputController.OnMoveInputUpdated += UpdateMoveDirection;
            _localInputController.OnAttackInputBegan += BeginAttack;
            _localInputController.OnAttackInputEnded += EndAttack;
            
        }

        private void OnDisable()
        {
            // Unsubscribe from input events
            _localInputController.OnMoveInputUpdated -= UpdateMoveDirection;
            _localInputController.OnAttackInputBegan -= BeginAttack;
            _localInputController.OnAttackInputEnded -= EndAttack;

            // Disable the LocalInputController
            _localInputController.Disable();
        }

#if UNITY_EDITOR
        //to be removed
        [ContextMenu("Take Damage")]
        public void TakeDamage()
        {
            CharacterStats.TakeDamage(10);
        }  
#endif
    }
}