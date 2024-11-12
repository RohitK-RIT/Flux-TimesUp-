using _Project.Scripts.Core.Enemy;
using UnityEngine;
using UnityEngine.AI;

public class ChaseState : BaseState
{
    private EnemyInputController _enemyInputController;
    private NavMeshAgent _navMeshAgent;

    public ChaseState(EnemyInputController enemyInputController) : base(EnemyState.Chase)
    {
        _enemyInputController = enemyInputController;
    }

    public override void EnterState()
    {
        Debug.Log("Entering Chase State");
        _navMeshAgent = _enemyInputController.GetComponentInParent<NavMeshAgent>();
        _navMeshAgent.isStopped = false; // Ensure the agent is not stopped at the beginning
    }

    public override void ExitState()
    {
        Debug.Log("Exiting Chase State");
        _enemyInputController.StopChasing(); // Stop following the player when exiting this state
    }

    public override void UpdateState()
    {
        //Step 1: Check if player is within chase range
        if (!_enemyInputController.CanChasePlayer())
        {
            // Player is out of chase range, transition to Detect state
            Debug.Log("Player out of chase range, transitioning to Detect state.");
            _enemyInputController.stateManager.TransitionToState(EnemyState.Detect);
            //return;
        }
        else
        {
            Debug.Log("Chasing player");
            // Step 2: Follow player
            _enemyInputController.RotateTowardsPlayer();
            _enemyInputController.StartChasing(); // This can use NavMeshAgent to follow player without colliding
        }
        
        
        // Step 3: Check if player is in attack range
        if (_enemyInputController.CanAttack())
        {
            // Player is in attack range, transition to Attack state
            Debug.Log("Player in attack range, transitioning to Attack state.");
            _enemyInputController.stateManager.TransitionToState(EnemyState.Attack);
        }
    }

    public override EnemyState GetNextState()
    {
        // No need for transition logic here as it is handled in UpdateState
        return EnemyState.Chase;
    }
}