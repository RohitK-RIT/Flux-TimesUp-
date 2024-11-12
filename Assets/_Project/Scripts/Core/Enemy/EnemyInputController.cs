using System;
using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.Core.Player_Controllers;
using _Project.Scripts.Core.Player_Controllers.Input_Controllers;
using UnityEngine;
using UnityEngine.AI;

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

        private float attackRange = 10f; // Attack range
        [SerializeField] private float attackCooldown = 5f; // Cooldown time between attacks

        private bool _isAttacking; // Tracks if an attack is in progress
        private bool _isCooldownActive; // Tracks if cooldown is active
        private Coroutine _attackCoroutine; // Holds the attack coroutine instance
        
        internal Transform _closestPlayer;
        
        [SerializeField]private Transform targetPlayerTest; // for testing purpose currently providing the transform of the player directly
        private float _updateSpeed = 0.1f; // how frequently to calculate path based on targets transform position
        private NavMeshAgent _enemy; // navmesh agent

        internal StateManager stateManager;

        private float _chaseRange = 15f;

        private void Awake()
        {
            _enemy = GetComponentInParent<NavMeshAgent>(); 
            // assigning the navmesh agent from the empty parent game object
            // empty game object is created to align the pivot of the enemy game object and the obstacle avoidance
            
            stateManager = gameObject.AddComponent<StateManager>();

            var states = new Dictionary<EnemyState, BaseState>
            {
                { EnemyState.Detect, new DetectState(this) },
                { EnemyState.Chase, new ChaseState(this) },
                { EnemyState.Attack, new AttackState(this) }
            };

            stateManager.InitializeStates(states, EnemyState.Detect);
        }

        private void Start()
        {
            //StartCoroutine(FollowPlayer());
        }

        public override void Initialize(PlayerController playerController)
        {
            base.Initialize(playerController);

            _playerDetection = GetComponent<PlayerDetection>();

            _playerDetection.Initialize(playerController);
        }

        private void Update()
        {
            // call method to find the closest player
            //FindPlayer();
        }

        public void Disable()
        {
            // Disable AI logic
            _currentTarget = null;
        }


        // Method to find the closest player and check if the closest player is in conical field of view
        // private void FindPlayer()
        // {
        //     // get the closest player
        //     _closestPlayer = _playerDetection.FindClosestPlayerInRange();
        //
        //     // check if the closest player is within the conical FOV
        //     if (_closestPlayer && _playerDetection.IsPlayerInCone(_closestPlayer))
        //     {
        //         // Rotate towards the player
        //         RotateTowardsPlayer(_closestPlayer);
        //
        //         // Try to attack the player
        //         TryAttack(_closestPlayer);
        //     }
        //     else if (_currentTarget)
        //     {
        //         // If the player is no longer in range, end the attack
        //         OnAttackInputEnded?.Invoke();
        //         _currentTarget = null;
        //     }
        // }

        internal bool FindPlayer()
        {
            _closestPlayer = _playerDetection.FindClosestPlayerInRange();
            if (_closestPlayer && IsPlayerInCone())
            {
                //RotateTowardsPlayer(_closestPlayer);
                return true;
            }
            else
            {
                return false;
            }
        }

        internal bool CanChasePlayer()
        {
            if (IsPlayerInCone())
            {
                float distance = Vector3.Distance(transform.position, _closestPlayer.position); 
                return distance <= _chaseRange; // Return true if within chase range
            }
            return false;
            
        }
        
        internal bool IsPlayerInCone()
        {
            // You need to implement this method in your PlayerDetection script to check if the player is in the cone
            return _playerDetection.IsPlayerInCone(_closestPlayer);
        }

        internal void StartChasing()
        {
            Debug.Log("inside StartChasing");
            if (IsPlayerInCone())
            {
                StartCoroutine(FollowPlayer()); // Start following the player
            }
        }
        
        internal void StopChasing()
        {
            _enemy.ResetPath(); // Stop following the player
        }

        internal bool CanAttack()
        {
            if (IsPlayerInCone())
            {
                float distanceToPlayer = Vector3.Distance(transform.position, _closestPlayer.position);
                return distanceToPlayer <= attackRange; // Check if the player is within attack range
            }
            else
            {
                return false;
            }
            
        }
        
        //method to check if the player is within attack range and cool down is not active
        internal void TryAttack()
        {
            if (IsPlayerInAttackRange(_closestPlayer) && !_isCooldownActive)
            {
                // start attacking when the player is in range and not in cooldown
                StartAttack();
                _currentTarget = _closestPlayer;
            }
            else if (_currentTarget == _closestPlayer)
            {
                // Stopping the enemy attack if the player is not in range and cooldown is active
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
        internal void StopAttack()
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
        internal void RotateTowardsPlayer()
        {
            Transform player = _closestPlayer;
            if (!player)
                PlayerController.MovementController.AimTransform.position = PlayerController.MovementController.Body.forward * 1000f;
            else
                PlayerController.MovementController.AimTransform.position = player.position;
        }

        // Method to move the enemy towards the player
        // Currently it's not having the expected movement behavior that why I have commented the setDestination which is responsible for the movement of the enemy
        // private IEnumerator FollowPlayer()
        // {
        //     Debug.Log("Moveeee");
        //     WaitForSeconds wait = new WaitForSeconds(_updateSpeed);
        //     float stoppingDistance = 1f; // Adjust this value to control how close the enemy gets to the player before stopping.
        //
        //     while (enabled)
        //     {
        //         float distanceToPlayer = Vector3.Distance(_enemy.transform.position, targetPlayerTest.position);
        //
        //         if (distanceToPlayer > stoppingDistance)
        //         {
        //             // If the enemy is farther than the stopping distance, set the destination to the player.
        //             _enemy.SetDestination(targetPlayerTest.position);
        //         }
        //         else
        //         {
        //             // If the enemy is within the stopping distance, stop moving.
        //             _enemy.ResetPath();
        //         }
        //
        //         yield return wait;
        //     }
        // }
        
        private IEnumerator FollowPlayer()
        {
            Debug.Log("FollowPlayer");
            //_enemy.SetDestination(_closestPlayer.position);
            while (CanChasePlayer())
            {
                // Move towards the player
                _enemy.SetDestination(_closestPlayer.position);
            
                // If player is in attack range, transition to attack state
                if (CanAttack())
                {
                    StopChasing(); // Stop chasing once attack range is reached
                    break;
                }
            
                yield return null; // Keep following every frame
            }
            //yield return null;
        }
        
        // to visualize the path towards the player
        // private void OnDrawGizmos()
        // {
        //     if (_enemy == null || targetPlayerTest == null) return;
        //
        //     NavMeshPath path = new NavMeshPath();
        //     _enemy.CalculatePath(targetPlayerTest.position, path);
        //
        //     // Draw the calculated path
        //     if (path.corners.Length > 1)
        //     {
        //         for (int i = 0; i < path.corners.Length - 1; i++)
        //         {
        //             Gizmos.color = Color.red;
        //             Gizmos.DrawLine(path.corners[i], path.corners[i + 1]);
        //         }
        //     }
        // }
        
        private void OnDrawGizmos()
        {
            if (_enemy == null) return;

            // Visualization of the chase range (sphere)
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(PlayerController.MovementController.Body.position, _chaseRange);

            // Visualization of the attack range (sphere)
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(PlayerController.MovementController.Body.position, attackRange);

            // Visualization of the field of view (cone)
            Gizmos.color = Color.yellow;

            // Use the current forward direction of the enemy
            Vector3 forwardDirection = PlayerController.MovementController.Body.forward * _chaseRange; // Adjust cone length with chase range
            float fovHalfAngle = _playerDetection.fieldOfViewAngle * 0.5f;

            // Calculate the boundaries of the cone
            Vector3 leftBoundary = Quaternion.Euler(0, -fovHalfAngle, 0) * forwardDirection;
            Vector3 rightBoundary = Quaternion.Euler(0, fovHalfAngle, 0) * forwardDirection;

            // Draw the cone in the updated direction
            Gizmos.DrawLine(PlayerController.MovementController.Body.position, PlayerController.MovementController.Body.position + leftBoundary); // Left boundary
            Gizmos.DrawLine(PlayerController.MovementController.Body.position, PlayerController.MovementController.Body.position + rightBoundary); // Right boundary
            Gizmos.DrawLine(PlayerController.MovementController.Body.position, PlayerController.MovementController.Body.position + forwardDirection); // Forward direction line
        }

    }
}