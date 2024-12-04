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
            // Check if the player health is low
            if (_enemyInputController.EnemyHUD.enemy.CurrentHealth < 50)
            {
                // Check if the enemy has been in FleeState recently and exceeded timeout
                if (_enemyInputController.LastFleeDuration >= _enemyInputController.FleeTimeout || _enemyInputController.enemyType == EnemyType.Boss)
                {
                    // Continue attacking as timeout condition overrides health
                    AttackPlayer();
                }
                else
                {
                    //Health is low. Transitioning to Flee state.
                    _enemyInputController.StateManager.TransitionToState(EnemyState.Flee);
                }
            }
            // If health is not low check if enemy can attack
            else if (_enemyInputController.CanAttack())
            {
                AttackPlayer();
            }
        }

        private void AttackPlayer()
        {
            // Face towards the player
            _enemyInputController.RotateTowardsPlayer();

            // Player is in attack range, so keep attacking
            _enemyInputController.TryAttack();
        }
        
        public override EnemyState GetNextState()
        {
            // If a player is still in attack range, stay in attack state
            if (_enemyInputController.CanAttack())
            {
                return EnemyState.Attack;
            }
            //Check if the player is now within chase range
            return _enemyInputController.CanChasePlayer() ? 
                // If the player is in chase range, transition to Chase state
                EnemyState.Chase : 
                EnemyState.Detect;
        }
    }
}