namespace _Project.Scripts.Core.Enemy.FSM.EnemyStates
{
    public class AttackState : BaseState
    {
        // Reference to the enemy's input controller
        private readonly EnemyInputController _enemyInputController;
        
        // Constructor for the AttackState, setting the state key and storing a reference to the input controller
        public AttackState(EnemyInputController enemyInputController) : base(EnemyState.Attack)
        {
            _enemyInputController = enemyInputController;
        }

        // Called when the enemy enters the AttackState
        public override void EnterState()
        {
            // Stop chasing the player when entering attack state
            _enemyInputController.StopChasing();
        }

        // Called when the enemy exits the AttackState
        public override void ExitState()
        {
            // Stop any ongoing attack actions
            _enemyInputController.StopAttack();
        }

        // Called every frame while the enemy is in the AttackState
        public override void UpdateState()
        {
            // Check if the player is still within attack range
            if (_enemyInputController.CanAttack())
            {
                // Face towards the player
                _enemyInputController.RotateTowardsPlayer();
                
                // Player is in attack range, so keep attacking
                _enemyInputController.TryAttack();
            }
            // If the player is out of attack range but within chase range, switch to ChaseState
            else if (_enemyInputController.CanChasePlayer())
            {
                _enemyInputController.StateManager.TransitionToState(EnemyState.Chase);
            }
            // If the player is neither in attack range nor chase range, switch to DetectState
            else
            {
                _enemyInputController.StateManager.TransitionToState(EnemyState.Detect);
            }
        }

        // Returns the current state key, indicating this is still AttackState
        public override EnemyState GetNextState()
        {
            // No need for transition logic here as it is handled in UpdateState
            return EnemyState.Attack;
        }
    }
}