using _Project.Scripts.Core.Enemy;
using UnityEngine;

public class AttackState : BaseState
{
    private EnemyInputController _enemyInputController;
    public AttackState(EnemyInputController enemyInputController) : base(EnemyState.Attack)
    {
        _enemyInputController = enemyInputController;
    }

    public override void EnterState()
    {
        Debug.Log("Entering sttack State");
        _enemyInputController.StopChasing(); // Stop movement when attacking
        //_enemyInputController.TryAttack();
    }

    public override void ExitState()
    {
        Debug.Log("Exiting attack State");
        _enemyInputController.StopAttack();
    }

    public override void UpdateState()
    {
        // Detect behavior here
        if (_enemyInputController.CanAttack())
        {
            _enemyInputController.RotateTowardsPlayer();
            // Player is in attack range, so keep attacking
            _enemyInputController.TryAttack();
        }
        else if (_enemyInputController.CanChasePlayer())
        {
            // Player is not in attack range but within chase range, transition to chase state
            _enemyInputController.stateManager.TransitionToState(EnemyState.Chase);
        }
        else
        {
            // Player is neither in attack range nor chase range, transition to detect state
            _enemyInputController.stateManager.TransitionToState(EnemyState.Detect);
        }
    }

    public override EnemyState GetNextState()
    {
        // Return next state based on some condition
        return EnemyState.Attack; // Example transition to Chase
    }
}