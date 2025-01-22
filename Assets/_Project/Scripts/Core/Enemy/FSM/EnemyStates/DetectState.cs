using UnityEngine;

namespace _Project.Scripts.Core.Enemy.FSM.EnemyStates
{
    public class DetectState : BaseState
    {
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
            // Resets player movement when entering the state
            _enemyInputController.StopChasing(); 
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
            _enemyInputController.StartChasing();
            // If the player is detected, rotate towards them
            _enemyInputController.RotateTowardsPlayer();
        }
        
        public override EnemyState GetNextState()
        {
            // check if the closest player in range
            if (_enemyInputController.CanChasePlayer())
            {
                return EnemyState.Chase;
            }
            
            // check if any player is in the player detection range
            return _enemyInputController.FindPlayer()? 
                // If player is in Detect range, stay in detected state
                EnemyState.Detect :
                EnemyState.Patrol;
        }
    }
}