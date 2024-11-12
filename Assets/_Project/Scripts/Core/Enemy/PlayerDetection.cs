using System.Collections.Generic;
using _Project.Scripts.Core.Character;
using UnityEngine;

namespace _Project.Scripts.Core.Enemy
{
    public class PlayerDetection : CharacterComponent
    {
        private float detectionRange = 20f;  // The distance at which the enemy detects the player
        public float fieldOfViewAngle = 60f; // The conical angle at which the enemy detects the player
        
        public LayerMask layerMask; // A LayerMask to specify which layers the detection should interact with
        
        private readonly List<Transform> _playersInRange = new List<Transform>();  // List of players currently in range
        
        void Update()
        {
            // Find all players in range
            FindPlayersInRange();

            // Debugging: Print the list of players in range --> for testing purpose will be removed later
            DebugPlayersInRange();
        }
       
        // Find all players currently within the detection range.
        private void FindPlayersInRange()
        {
            // Get all colliders within the detection range
            Collider[] hitColliders = Physics.OverlapSphere(transform.position, detectionRange, layerMask);

            // Create a temporary list to track the players in this frame
            List<Transform> currentPlayers = new List<Transform>();

            foreach (Collider hitCollider in hitColliders)
            {
                if (hitCollider.CompareTag("Player"))
                {
                    currentPlayers.Add(hitCollider.transform);

                    // If player is not already in the list, add them
                    if (!_playersInRange.Contains(hitCollider.transform))
                    {
                        _playersInRange.Add(hitCollider.transform);
                        Debug.Log("Player entered range: " + hitCollider.transform.name); // will be removed later
                    }
                }
            }

            // Remove players who have left the detection range
            for (int i = _playersInRange.Count - 1; i >= 0; i--)
            {
                if (!currentPlayers.Contains(_playersInRange[i]))
                {
                    Debug.Log("Player left range: " + _playersInRange[i].name); // will be removed later
                    _playersInRange.RemoveAt(i);  // Remove the player reference
                }
            }
        }
        
        // Print the names of the players currently in range for debugging --> for testing will be removed later
        private void DebugPlayersInRange()
        {
            if (_playersInRange.Count == 0)
            {
                Debug.Log("No players in range.");
            }
            else
            {
                Debug.Log("Players in range:"+_playersInRange.Count);
                foreach (Transform player in _playersInRange)
                {
                    Debug.Log(player.name);
                }
            }
        }
        
        // Find the closest player within the playersInRange list.
        internal Transform FindClosestPlayerInRange()
        {
            // Variable to store the closest player's Transform
            Transform closest = null;
            
            // Initialize the closest distance as the detection range
            float closestDistance = detectionRange;
            foreach (Transform player in _playersInRange)
            {
                // Calculate the distance between the enemy and the current player
                float distanceToPlayer = Vector3.Distance(transform.position, player.position);
                
                // Check if this player is closer than the previously found closest player
                if (distanceToPlayer < closestDistance)
                {
                    closestDistance = distanceToPlayer; // Update the closest distance
                    closest = player; // Update the closest player reference
                }
            }
            
            // Return the Transform of the closest player, or null if no player was found
            return closest;
        }

        internal bool IsPlayerInCone(Transform player)
        {
            if (!player)
                return false; 
            Vector3 directionToPlayer = player.position - PlayerController.MovementController.Body.position;
            directionToPlayer.y = 0;  // Only rotate on the horizontal plane

            // Angle between enemy's forward direction and the direction to the player
            float angleToPlayer = Vector3.Angle(PlayerController.MovementController.Body.forward, directionToPlayer.normalized);

            // Check if the player is within the field of view angle
            return angleToPlayer <= fieldOfViewAngle;
        }
        
        
        private void OnDrawGizmos()
        {
            // Visualization of the detection range (sphere)
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(PlayerController.MovementController.Body.position, detectionRange);

            // Visualization of the chase range (sphere)
            Gizmos.color = Color.blue;
            //Gizmos.DrawWireSphere(PlayerController.MovementController.Body.position, 15f); // Chase range (you can also add this to your EnemyInputController)

            // Visualization of the field of view (cone)
            Gizmos.color = Color.yellow;
            Vector3 forwardDirection = PlayerController.MovementController.Body.forward * detectionRange;
            float fovHalfAngle = fieldOfViewAngle * 0.5f;
            float coneAngle = Mathf.Deg2Rad * fovHalfAngle; // Convert to radians

            // Draw the sides of the cone using lines
            Vector3 leftBoundary = Quaternion.Euler(0, -fovHalfAngle, 0) * forwardDirection;
            Vector3 rightBoundary = Quaternion.Euler(0, fovHalfAngle, 0) * forwardDirection;
            Gizmos.DrawLine(PlayerController.MovementController.Body.position, PlayerController.MovementController.Body.position + leftBoundary); // Left boundary
            Gizmos.DrawLine(PlayerController.MovementController.Body.position, PlayerController.MovementController.Body.position + rightBoundary); // Right boundary

            // Draw the front line of the cone
            Gizmos.DrawLine(PlayerController.MovementController.Body.position, PlayerController.MovementController.Body.position + forwardDirection); // Forward direction line
        }

    }
}
