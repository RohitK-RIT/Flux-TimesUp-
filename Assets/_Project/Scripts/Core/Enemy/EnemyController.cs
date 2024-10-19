using System;
using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.Core.Enemy;
using _Project.Scripts.Core.Player_Controllers;
using UnityEngine;

[RequireComponent(typeof(EnemyInputController))]
public class EnemyController : PlayerController
{
    private EnemyInputController _enemyInputController;
    private EnemyAttack _enemyAttack;

    protected override void Awake()
    {
        base.Awake();
        
        _enemyAttack = GetComponent<EnemyAttack>();
        var playerDetection = GetComponent<PlayerDetection>();

        _enemyInputController = GetComponent<EnemyInputController>();
    }
    
    private void OnEnable()
    {
        _enemyInputController.OnAttackInputBegan += BeginAttack;
        _enemyInputController.OnAttackInputEnded += EndAttack;
    }
    
    private void OnDisable()
    {
        _enemyInputController.Disable();
        _enemyInputController.OnAttackInputBegan -= BeginAttack;
        _enemyInputController.OnAttackInputEnded -= EndAttack;
    }
    
}
