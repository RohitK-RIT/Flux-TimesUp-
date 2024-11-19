using System;
using System.Collections;
using System.Collections.Generic;
using _Project.Scripts.Core.Enemy.FSM;
using _Project.Scripts.Core.Enemy.FSM.EnemyStates;
using _Project.Scripts.Core.Player_Controllers;
using _Project.Scripts.Core.Player_Controllers.Input_Controllers;
using UnityEngine;
using UnityEngine.AI;
using Random = Unity.Mathematics.Random;

namespace _Project.Scripts.Core.Enemy
{
    [RequireComponent(typeof(PlayerDetection))]
    public class EnemyInputController : InputController
    {
        public override event Action<Vector2> OnMoveInputUpdated;
        public override event Action OnAttackInputBegan;
        public override event Action OnAttackInputEnded;

        private PlayerDetection _playerDetection;
        
        private Transform _currentTarget; // current target to assign

        private readonly float _attackRange = 10f; // Attack range
        
        [SerializeField] private float attackCooldown = 5f; // Cooldown time between attacks

        private bool _isAttacking; // Tracks if an attack is in progress
        
        private bool _isCooldownActive; // Tracks if cooldown is active
        
        private Coroutine _attackCoroutine; // Holds the attack coroutine instance
        
        private Transform _closestPlayer; // closest player to the enemy which is the actual target

        internal NavMeshAgent _enemy; // navmesh agent

        internal StateManager StateManager; // refrence to state manager

        private readonly float _chaseRange = 15f; // chase range


        internal Vector3 _roamingPosition;
        
        
        private bool isRoaming = false; 

        private void Start()
        {
            //_startingPosition = transform.position;
            //Debug.Log("start pos"+_startingPosition);
            
            

        }

        private void Update()
        {
            //Debug.Log("roam pos"+_roamingPosition);
            // Only start the coroutine if it's not already running
            
            //Debug.Log("distance ="+Vector3.Distance(_enemy.transform.position, _roamingPosition));
            
        }
        private void Awake()
        {
            _enemy = GetComponent<NavMeshAgent>(); 
            // assigning the navmesh agent from the empty parent game object
            // empty game object is created to align the pivot of the enemy game object and the obstacle avoidance
            
            StateManager = gameObject.AddComponent<StateManager>();

            // Initializing the dictionary for states
            var states = new Dictionary<EnemyState, BaseState>
            {
                { EnemyState.Patrol, new PatrolState(this) },
                { EnemyState.Detect, new DetectState(this) },
                { EnemyState.Chase, new ChaseState(this) },
                { EnemyState.Attack, new AttackState(this) },
                { EnemyState.Flee, new FleeState(this) }
            };

            StateManager.InitializeStates(states, EnemyState.Patrol);
        }

        public override void Initialize(PlayerController playerController)
        {
            base.Initialize(playerController);

            _playerDetection = GetComponent<PlayerDetection>();

            _playerDetection.Initialize(playerController);
        }

        public void Disable()
        {
            // Disable AI logic
            _currentTarget = null;
        }

        // Method to check if the player is in detection range and in conical field of view
        internal bool FindPlayer()
        {
            _closestPlayer = _playerDetection.FindClosestPlayerInRange();
            return _closestPlayer && IsPlayerInCone();
        }

        internal bool isPlayerinDetectionRange()
        {
            return _playerDetection._playersInRange.Count>0;
        }

        // Method to check if player is in chase range and conical field of view
        internal bool CanChasePlayer()
        {
            if (!IsPlayerInCone()) return false;
            var distance = Vector3.Distance(transform.position, _closestPlayer.position); 
            return distance <= _chaseRange; // Return true if within chase range

        }
        
        // Method to check if player is in conical field of view
        private bool IsPlayerInCone()
        {
            // You need to implement this method in your PlayerDetection script to check if the player is in the cone
            return _playerDetection.IsPlayerInCone(_closestPlayer);
        }

        // Method to start chasing the player if player is in conical field of view
        // ReSharper disable Unity.PerformanceAnalysis
        internal void StartChasing()
        {
            if (IsPlayerInCone())
            {
                StartCoroutine(FollowPlayer()); // Start following the player
            }
        }
        
        // Method to stop chasing the player i.e resetting the navmesh agent path
        internal void StopChasing()
        {
            _enemy.ResetPath(); // Stop following the player
        }

        // Method to check if the player is in attack range and conical field of view
        internal bool CanAttack()
        {
            if (IsPlayerInCone())
            {
                var distanceToPlayer = Vector3.Distance(transform.position, _closestPlayer.position);
                return distanceToPlayer <= _attackRange; // Check if the player is within attack range
            }
            else
            {
                return false;
            }
            
        }
        
        //method to check if the player is within attack range and cool down is not active
        // ReSharper disable Unity.PerformanceAnalysis
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
            var distanceToPlayer = Vector3.Distance(transform.position, player.position);
            return distanceToPlayer <= _attackRange;
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
            var player = _closestPlayer;
            if (!player)
                PlayerController.MovementController.AimTransform.position = PlayerController.MovementController.Body.forward * 1000f;
            else
                PlayerController.MovementController.AimTransform.position = player.position;
        }
        
        // Coroutine to follow player
        private IEnumerator FollowPlayer()
        {
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
        }

        private IEnumerator MoveToRoamPoition()
        {
            isRoaming = true; // Set flag to prevent multiple coroutines
            _enemy.SetDestination(_roamingPosition);
            while (_enemy.remainingDistance > 0.5f)
            {
                yield return null; // Wait for the next frame
            }

            // Once close enough, set roaming position to a new location
            _roamingPosition = GetRoamingPosition(_enemy.transform.position);
            isRoaming = false;

        }

        internal Vector3 GetRoamingPosition(Vector3 startPosition)
        {
            return startPosition + GetRandomDirection() * UnityEngine.Random.Range(5, 15);
        }

        private static Vector3 GetRandomDirection()
        {
            return new Vector3(UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f), UnityEngine.Random.Range(-1f, 1f)).normalized;
        }

        internal void StartRoaming()
        {
            if (!isRoaming)
            {
                StartCoroutine(MoveToRoamPoition());
            }
        }
        
        // Method to visualize the detect, chase, attack and conical FOV for testing purpose
        private void OnDrawGizmos()
        {
            if (_enemy == null) return;

            // Visualization of the chase range (sphere)
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(PlayerController.MovementController.Body.position, _chaseRange);

            // Visualization of the attack range (sphere)
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(PlayerController.MovementController.Body.position, _attackRange);

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