using _Project.Scripts.Core.Player_Controllers;
using UnityEngine;

namespace _Project.Scripts.Core.Enemy
{
    [RequireComponent(typeof(EnemyInputController))]
    public class EnemyController : PlayerController
    {
        private EnemyInputController _enemyInputController;

        protected override void Awake()
        {
            base.Awake();
            _enemyInputController = GetComponent<EnemyInputController>();
        }
    
        private void OnEnable()
        {
            // Subscribe to attack input events on enable
            _enemyInputController.OnAttackInputBegan += BeginAttack;
            _enemyInputController.OnAttackInputEnded += EndAttack;
        
            // Subscribe to look input updates on enable
            _enemyInputController.OnLookInputUpdated += LookInputUpdated;
        }

        private void OnDisable()
        {
            // Subscribe to attack input events on disable
            _enemyInputController.Disable();
            _enemyInputController.OnAttackInputBegan -= BeginAttack;
            _enemyInputController.OnAttackInputEnded -= EndAttack;
        
            // Subscribe to look input updates on disable
            _enemyInputController.OnLookInputUpdated -= LookInputUpdated;
        }

        protected override void LookInputUpdated(Vector2 lookInput)
        {
            // Update the look direction in the MovementController based on the new input.
            MovementController.LookDirection = lookInput;
        }
    }
}