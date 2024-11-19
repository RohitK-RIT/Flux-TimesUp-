using UnityEngine;

namespace _Project.Scripts.Core.Enemy.FSM.EnemyStates
{
    public class PatrolState : BaseState
    {
        // Reference to the enemy's input controller
        private readonly EnemyInputController _enemyInputController;
    
        // Constructor for the DetectState, setting the state key and storing a reference to the input controller
        public PatrolState(EnemyInputController enemyInputController) : base(EnemyState.Patrol) 
        {
            _enemyInputController = enemyInputController;
        }
        private Vector3 _startingPosition;
        
        //internal Vector3 _roamingPositiontest;

        // Called when the enemy enters the DetectState
        public override void EnterState()
        {
            // Resets player detection status when entering the state
            Debug.Log("Patrol State Enter");
            _startingPosition = _enemyInputController._enemy.transform.position;
            _enemyInputController._roamingPosition = _enemyInputController.GetRoamingPosition(_startingPosition);
        }

        // Called when the enemy exits the DetectState
        public override void ExitState()
        {
            Debug.Log("Exiting Patrol State");
        }

        // Called every frame while the enemy is in the ChaseState
        // ReSharper disable Unity.PerformanceAnalysis
        public override void UpdateState()
        {
            Debug.Log("Patrol State Update");
            _enemyInputController.StartRoaming();
            
            if (Vector3.Distance(_enemyInputController._enemy.transform.position, _enemyInputController._roamingPosition) <= 1f)
            {
                _enemyInputController._roamingPosition = _enemyInputController.GetRoamingPosition(_startingPosition);
            }
        }

        // Returns the current state key, indicating this is still AttackState
        public override EnemyState GetNextState()
        {
            if (_enemyInputController.FindPlayer())
            {
                return EnemyState.Detect;
            }
            // No need for transition logic here as it is handled in UpdateState
            return EnemyState.Patrol;
        }
    }
}
