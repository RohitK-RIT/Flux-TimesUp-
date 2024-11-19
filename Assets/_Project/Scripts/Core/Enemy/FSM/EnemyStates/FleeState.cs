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
    }

    // Called every frame while the enemy is in the ChaseState
    // ReSharper disable Unity.PerformanceAnalysis
    public override void UpdateState()
    {
        Debug.Log("Flee State Update");
    }

    // Returns the current state key, indicating this is still AttackState
    public override EnemyState GetNextState()
    {
        // No need for transition logic here as it is handled in UpdateState
        return EnemyState.Detect;
    }
}
