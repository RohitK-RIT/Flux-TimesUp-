using _Project.Scripts.Core.Enemy;
using UnityEngine;

public class DetectState : BaseState
{
    public DetectState() : base(EnemyState.Detect) { }

    private bool _isPlayerInRange;
    private EnemyInputController _enemyInputController;
    
    public DetectState(EnemyInputController enemyInputController) : base(EnemyState.Detect) 
    {
        _enemyInputController = enemyInputController;
    }
    
    public override void EnterState()
    {
        Debug.Log("Entering Detect State");
        _isPlayerInRange = false; // Reset detection state
    }

    public override void ExitState()
    {
        Debug.Log("Exiting Detect State");
    }

    public override void UpdateState()
    {
        // Repeatedly check if the player is in range
        _isPlayerInRange = _enemyInputController.FindPlayer();

        if (_isPlayerInRange)
        {
            _enemyInputController.RotateTowardsPlayer();
            Debug.Log("Player detected, transitioning to Chase state.");
            // State transition is handled by the StateManager, no need for return here.
        }
        else
        {
            Debug.Log("Still detecting...");
        }
    }

    public override EnemyState GetNextState()
    {
        // Return next state based on some condition
        Debug.Log("is player in range?:"+ _isPlayerInRange);
        return _isPlayerInRange ? EnemyState.Chase : EnemyState.Detect;
    }
}