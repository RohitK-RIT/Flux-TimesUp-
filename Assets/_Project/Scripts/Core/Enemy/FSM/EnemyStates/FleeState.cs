using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.Core.Enemy;
using _Project.Scripts.Core.Enemy.FSM;
using UnityEngine;

public class FleeState : BaseState
{
    // Reference to the enemy's input controller
    private readonly EnemyInputController _enemyInputController;
    
    // Constructor for the DetectState, setting the state key and storing a reference to the input controller
    public FleeState(EnemyInputController enemyInputController) : base(EnemyState.Flee) 
    {
        _enemyInputController = enemyInputController;
    }
   
    // Called when the enemy enters the DetectState
    public override void EnterState()
    {
        // Resets player detection status when entering the state
        Debug.Log("Flee State Enter");
    }

    // Called when the enemy exits the DetectState
    public override void ExitState()
    {
        Debug.Log("Exiting Flee State");
        // Record the time spent in the FleeState
        _enemyInputController.LastFleeDuration = _enemyInputController.FleeTimer;

        // Reset the flee timer
        //_enemyInputController.FleeTimer = 0f;
    }

    // Called every frame while the enemy is in the ChaseState
    // ReSharper disable Unity.PerformanceAnalysis
    public override void UpdateState()
    {
        Debug.Log("Flee State Update");
        Debug.Log("Health=" + _enemyInputController._enemyHUD.enemy.CurrentHealth);
        if (_enemyInputController._closestPlayer == null || _enemyInputController._enemy == null)
        {
            Debug.Log("Closest Player is null");
            return;
        }
        
        // Increment the flee timer
        _enemyInputController.FleeTimer += Time.deltaTime;

        // Calculate the direction away from the player
        Vector3 fleeDirection = (_enemyInputController._enemy.transform.position - _enemyInputController._closestPlayer.position).normalized;

        // Find a point to flee to
        Vector3 fleeTarget = _enemyInputController._enemy.transform.position + fleeDirection * _enemyInputController.SafeDistance;
        
        // Set the NavMeshAgent's destination to the flee target
        if (UnityEngine.AI.NavMesh.SamplePosition(fleeTarget, out UnityEngine.AI.NavMeshHit hit, _enemyInputController.SafeDistance, UnityEngine.AI.NavMesh.AllAreas))
        {
            _enemyInputController._enemy.SetDestination(hit.position);
        }
        
        // Check if the enemy is now at a safe distance
        if (_enemyInputController._closestPlayer == null || Vector3.Distance(_enemyInputController._enemy.transform.position, _enemyInputController._closestPlayer.position) > _enemyInputController.SafeDistance)
        {
            Debug.Log("Enemy has reached a safe distance. Transitioning to Patrol.");
            _enemyInputController.StateManager.TransitionToState(EnemyState.Patrol);
        }
    }

    // Returns the current state key, indicating this is still AttackState
    public override EnemyState GetNextState()
    {
        

        return EnemyState.Flee;
    }
}
