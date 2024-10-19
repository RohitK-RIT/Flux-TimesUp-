using _Project.Scripts.Core.Enemy;
using _Project.Scripts.Core.Player_Controllers;
using UnityEngine;

[RequireComponent(typeof(EnemyInputController))]
public class EnemyController : PlayerController
{
    private EnemyInputController _enemyInputController;

    protected override void Awake()
    {
        base.Awake();

        _enemyInputController = GetComponent<EnemyInputController>();
    }
    
    private void OnEnable()
    {
        _enemyInputController.OnAttackInputBegan += BeginAttack;
        _enemyInputController.OnAttackInputEnded += EndAttack;
        
        _enemyInputController.OnLookInputUpdated += LookInputUpdated;
    }

    private void OnDisable()
    {
        _enemyInputController.Disable();
        _enemyInputController.OnAttackInputBegan -= BeginAttack;
        _enemyInputController.OnAttackInputEnded -= EndAttack;
        
        _enemyInputController.OnLookInputUpdated -= LookInputUpdated;
    }

    protected override void LookInputUpdated(Vector2 lookInput)
    {
        MovementController.lookDirection = lookInput;
    }
}