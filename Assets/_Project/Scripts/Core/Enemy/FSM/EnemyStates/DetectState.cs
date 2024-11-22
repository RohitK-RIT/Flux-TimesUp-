using UnityEngine;

namespace _Project.Scripts.Core.Enemy.FSM.EnemyStates
{
    public class DetectState : BaseState
    {
        // Tracks if the player is within detection range
        private bool _isPlayerInRange; 
        
        // Reference to the enemy's input controller
        private readonly EnemyInputController _enemyInputController;
    
        // Constructor for the DetectState, setting the state key and storing a reference to the input controller
        public DetectState(EnemyInputController enemyInputController) : base(EnemyState.Detect) 
        {
            _enemyInputController = enemyInputController;
        }
    
        // Called when the enemy enters the DetectState
        public override void EnterState()
        {
            // Resets player detection status when entering the state
            _isPlayerInRange = false;
        }

        // Called when the enemy exits the DetectState
        public override void ExitState()
        {
            Debug.Log("Exiting Detect State");
        }

        // Called every frame while the enemy is in the ChaseState
        // ReSharper disable Unity.PerformanceAnalysis
        public override void UpdateState()
        {
            // Continuously checks if the player is in detection range
            _isPlayerInRange = _enemyInputController.FindPlayer();

            if (_isPlayerInRange)
            {
                // If the player is detected, rotate towards them and transition to Chase state
                _enemyInputController.RotateTowardsPlayer();
            }
        }

        // Returns the current state key, indicating this is still AttackState
        public override EnemyState GetNextState()
        {
            if (_isPlayerInRange)
            {
                return EnemyState.Chase;
            }
            return _enemyInputController.isPlayerinDetectionRange() ? EnemyState.Detect :
                // No need for transition logic here as it is handled in UpdateState
                EnemyState.Patrol;
        }
    }
}