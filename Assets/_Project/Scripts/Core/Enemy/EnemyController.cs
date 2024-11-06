using _Project.Scripts.Core.Player_Controllers;
using UnityEngine;

namespace _Project.Scripts.Core.Enemy
{
    [RequireComponent(typeof(EnemyInputController))]
    public class EnemyController : PlayerController
    {
        private EnemyInputController _enemyInputController;

        protected override void Awake()
        {
            base.Awake();
            
            _enemyInputController = GetComponent<EnemyInputController>();
            
            _enemyInputController.Initialize(this);
        }

        private void OnEnable()
        {
            // Subscribe to attack input events on enable
            _enemyInputController.OnAttackInputBegan += BeginAttack;
            _enemyInputController.OnAttackInputEnded += EndAttack;
        }

        private void OnDisable()
        {
            // Subscribe to attack input events on disable
            _enemyInputController.Disable();
            _enemyInputController.OnAttackInputBegan -= BeginAttack;
            _enemyInputController.OnAttackInputEnded -= EndAttack;
        }

        protected override void Die()
        {
            gameObject.SetActive(false);
        }
    }
}