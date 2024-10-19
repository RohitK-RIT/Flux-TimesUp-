using System;
using System.Collections;
using _Project.Scripts.Core.Character;
using _Project.Scripts.Core.Character.Weapon_Controller;
using _Project.Scripts.Core.Player_Controllers.Input_Controllers;
using _Project.Scripts.Core.Weapons;
using UnityEngine;

namespace _Project.Scripts.Core.Enemy
{
    [RequireComponent(typeof(PlayerDetection))]
    public class EnemyInputController : InputController
    {
        public override event Action<Vector2> OnMoveInputUpdated;
        public override event Action OnAttackInputBegan;
        public override event Action OnAttackInputEnded;

        private PlayerDetection _playerDetection;
        private Transform _currentTarget;
        private MovementController _movementController;
        private WeaponController _weaponController;
        
        [SerializeField] private float attackRange = 5f; // Attack range
        [SerializeField] private float attackCooldown = 5f; // Cooldown time between attacks

     
        private bool _isAttacking;      // Tracks if an attack is in progress
        private bool _isCooldownActive; // Tracks if cooldown is active
        private Coroutine _attackCoroutine; // Holds the attack coroutine instance
        private float _lastAttackTime;

        void Awake()
        {
            _playerDetection = GetComponent<PlayerDetection>();
            _weaponController = GetComponent<WeaponController>();
            _movementController = GetComponent<MovementController>();

        }
        private void Update()
        {
            UpdateAIInput();
        }

        public void Disable()
        {
            // Disable AI logic
            _currentTarget = null;
        }
    
        public void UpdateAIInput()
        {
            Transform closestPlayer = _playerDetection.FindClosestPlayerInRange();
            if (closestPlayer && _playerDetection.IsPlayerInCone(closestPlayer))
            {
                // Try to attack the player
                TryAttack(closestPlayer);
            }
            else if (_currentTarget != null)
            {
                // If the player is no longer in range, end the attack
                OnAttackInputEnded?.Invoke();
                _currentTarget = null;
            }
        }
    
        private void TryAttack(Transform player)
        {
            if (IsPlayerInAttackRange(player) && !IsOnCooldown())
            {
                StartAttack();
                _currentTarget = player;
            }
            else if (_currentTarget == player)
            {
                StopAttack();
                _currentTarget = null;
            }
        }
        
        public bool IsPlayerInAttackRange(Transform player)
     {
         float distanceToPlayer = Vector3.Distance(transform.position, player.position);
         return distanceToPlayer <= attackRange;
     }


     // Start the attack process if not already attacking and not in cooldown
     public void StartAttack()
     {
         if (_isAttacking || _isCooldownActive) return;  // Prevent multiple attacks or attacks during cooldown
         _isAttacking = true;
         _lastAttackTime = Time.time;                   // Record the attack start time
         _attackCoroutine = StartCoroutine(AttackCoroutine());
     }
     
     // Stop the attack when the player is out of range
     public void StopAttack()
     {
         if (_isAttacking)
         {
            //_weaponController.CurrentWeapon.EndAttack();    // Stop the weapon from attacking
            OnAttackInputEnded?.Invoke();
             if (_attackCoroutine != null)
             {
                 StopCoroutine(_attackCoroutine);  // Stop the attack coroutine if it's running
             }
             _isAttacking = false;
         }
     }

     // Check if the cooldown period is active
     public bool IsOnCooldown()
     {
         return _isCooldownActive;
     }

    // Coroutine to handle the attack and initiate cooldown after finishing
     private IEnumerator AttackCoroutine()
     {
         OnAttackInputBegan?.Invoke();
         yield return new WaitForSeconds(attackCooldown);  // Wait for the attack duration or cooldown time
         StartCoroutine(StartCooldown());
     }

     // Coroutine to handle cooldown
     private IEnumerator StartCooldown()
     {
         _isCooldownActive = true;
         yield return new WaitForSeconds(attackCooldown);  // Wait for the cooldown period
         _isCooldownActive = false;
         _isAttacking = false;  // Allow a new attack after cooldown
     }
     
    
    }
}
