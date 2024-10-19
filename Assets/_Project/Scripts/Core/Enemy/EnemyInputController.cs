using System;
using System.Collections;
using _Project.Scripts.Core.Character;
using _Project.Scripts.Core.Character.Weapon_Controller;
using _Project.Scripts.Core.Player_Controllers.Input_Controllers;
using UnityEngine;

namespace _Project.Scripts.Core.Enemy
{
    [RequireComponent(typeof(PlayerDetection))]
    public class EnemyInputController : InputController
    {
        public override event Action<Vector2> OnMoveInputUpdated;
        public override event Action<Vector2> OnLookInputUpdated;
        public override event Action OnAttackInputBegan;
        public override event Action OnAttackInputEnded;

        private PlayerDetection _playerDetection;
        private Transform _currentTarget;

        [SerializeField] private float attackRange = 5f; // Attack range
        [SerializeField] private float attackCooldown = 5f; // Cooldown time between attacks

        private bool _isAttacking; // Tracks if an attack is in progress
        private bool _isCooldownActive; // Tracks if cooldown is active
        private Coroutine _attackCoroutine; // Holds the attack coroutine instance

        // This is hack, remove later.
        private WeaponController _weaponController;

        private void Awake()
        {
            _playerDetection = GetComponent<PlayerDetection>();
            _weaponController = GetComponent<WeaponController>();
        }

        private void Update()
        {
            // call method to finf closest player
            FindPlayer();
        }

        public void Disable()
        {
            // Disable AI logic
            _currentTarget = null;
        }

        
        // Method to find closest player and check if the closest player is in conical field of view
        private void FindPlayer()
        {
            // get the closest player
            Transform closestPlayer = _playerDetection.FindClosestPlayerInRange();
            
            Debug.Log("chk to rotate player:" + _playerDetection.IsPlayerInCone(closestPlayer));
            // check if the closest player is within the conical FOV
            if (closestPlayer && _playerDetection.IsPlayerInCone(closestPlayer))
            {
                Debug.Log("rotate player:" + closestPlayer.gameObject.name);
                // Rotate towards the player
                RotateTowardsPlayer(closestPlayer);
                
                // Try to attack the player
                TryAttack(closestPlayer);
            }
            else if (_currentTarget)
            {
                // If the player is no longer in range, end the attack
                OnAttackInputEnded?.Invoke();
                _currentTarget = null;
            }
        }

        //method to check if the player is within attack range and cool down is not active
        private void TryAttack(Transform player)
        {
            if (IsPlayerInAttackRange(player) && !_isCooldownActive)
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

        // method to check if player is in attack range
        private bool IsPlayerInAttackRange(Transform player)
        {
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);
            return distanceToPlayer <= attackRange;
        }


        // Start the attack process if not already attacking and not in cooldown
        private void StartAttack()
        {
            if (_isAttacking || _isCooldownActive) return; // Prevent multiple attacks or attacks during cooldown
            _isAttacking = true;
            _attackCoroutine = StartCoroutine(AttackCoroutine());
        }

        // Stop the attack when the player is out of range
        private void StopAttack()
        {
            if (!_isAttacking) return;
            
            // Invoke an event to notify end attack
            OnAttackInputEnded?.Invoke();
            if (_attackCoroutine != null)
            {
                StopCoroutine(_attackCoroutine); // Stop the attack coroutine if it's running
            }

            _isAttacking = false;
        }

        // Coroutine to handle the attack and initiate cooldown after finishing
        private IEnumerator AttackCoroutine()
        {
            // Invoke an event to notify start attack
            OnAttackInputBegan?.Invoke();
            yield return new WaitForSeconds(attackCooldown); // Wait for the attack duration or cooldown time
            StartCoroutine(StartCooldown());
        }

        // Coroutine to handle cooldown
        private IEnumerator StartCooldown()
        {
            _isCooldownActive = true;
            yield return new WaitForSeconds(attackCooldown); // Wait for the cooldown period
            _isCooldownActive = false;
            _isAttacking = false; // Allow a new attack after cooldown
        }

        //Method to rotate the enemy towards the player
        private void RotateTowardsPlayer(Transform player)
        {
            if (!player)
                return;
            
            // Calculate the direction from the enemy to the player
            Vector3 directionToPlayer = (player.position - transform.position).normalized;
            
            // Extract horizontal direction (Y-axis) from the directionToPlayer
            Vector3 horizontalDirection = new Vector3(directionToPlayer.x, 0, directionToPlayer.z);
            
            // Calculate the vertical angle (X-axis rotation)
            float verticalAngle = Mathf.Atan2(directionToPlayer.y, horizontalDirection.magnitude) * Mathf.Rad2Deg;
            
            // Calculate the final rotation needed to align the weapon's forward direction with the player's position
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);
            
            // Update the enemy's body rotation (horizontal aiming)
            //transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
            
            // Rotate the weapon towards the player (vertical aiming)
            if (_weaponController && _weaponController.CurrentWeapon)
            {
                // Get the current local rotation of the weapon and update the pitch (X-axis) based on the calculated vertical angle
                var weaponLocalRotation = _weaponController.CurrentWeapon.transform.localRotation;
                Quaternion weaponTargetRotation = Quaternion.Euler(-verticalAngle, weaponLocalRotation.eulerAngles.y, weaponLocalRotation.eulerAngles.z);
                _weaponController.CurrentWeapon.transform.localRotation = Quaternion.Slerp(weaponLocalRotation, weaponTargetRotation, Time.deltaTime * 5f);
            }
            
            // Invoke an event to notify that the look direction (rotation) has been updated
            OnLookInputUpdated?.Invoke(new Vector2(targetRotation.eulerAngles.y, verticalAngle));
            
            
            
        }
    }
}