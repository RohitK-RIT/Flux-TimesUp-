using UnityEngine;
namespace _Project.Scripts.Core.Enemy.FSM.EnemyStates
{
    public class ChaseState : BaseState
    {
        // Reference to the enemy's input controller
        private readonly EnemyInputController _enemyInputController;

        // Constructor for the ChaseState, setting the state key and storing a reference to the input controller
        public ChaseState(EnemyInputController enemyInputController) : base(EnemyState.Chase)
        {
            _enemyInputController = enemyInputController;
        }

        // Called when the enemy enters the ChaseState
        public override void EnterState()
        {
            Debug.Log("Entering Chase State");
        }

        // Called when the enemy exits the ChaseState
        public override void ExitState()
        {
            _enemyInputController.StopChasing(); // Stop following the player when exiting this state
        }

        // Called every frame while the enemy is in the ChaseState
        // ReSharper disable Unity.PerformanceAnalysis
        public override void UpdateState()
        {
            // Step 1: Check if the player is still within chase range
            {
                // Step 2: Continue chasing the player
                // Face towards the player while chasing
                _enemyInputController.RotateTowardsPlayer();
                
                // Uses NavMeshAgent to move towards the player
                _enemyInputController.StartChasing();
            }
            
            
        }

        // Returns the current state key, indicating this is still AttackState
        public override EnemyState GetNextState()
        {
            if (!_enemyInputController.CanChasePlayer())
            {
                // If the player is out of chase range, transition to Detect state
                return EnemyState.Detect;
            }

            // Step 3: Check if the player is now within attack range
            return _enemyInputController.CanAttack() ?
                // If the player is in attack range, transition to Attack state
                EnemyState.Attack :
                // No need for transition logic here as it is handled in UpdateState
                EnemyState.Chase;
        }
    }
}