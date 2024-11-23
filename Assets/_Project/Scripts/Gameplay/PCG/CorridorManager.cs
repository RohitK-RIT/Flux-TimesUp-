using System.Collections.Generic;
using UnityEngine;

namespace _Project.Scripts.Gameplay.PCG {
    /// <summary>
    /// Manages the creation and connection of corridors between rooms in the dungeon.
    /// </summary>
    public class CorridorManager : MonoBehaviour {
        /// <summary>
        /// Prefab used for creating corridor segments.
        /// </summary>
        public GameObject corridorPrefab;
        private DungeonGenerator _dungeonGenerator; // Reference to the dungeon generator
        
        private void Awake() {
            _dungeonGenerator = GetComponent<DungeonGenerator>();
        }
        /// <summary>
        /// Connects two exits with a corridor, ensuring no overlap with existing rooms.
        /// </summary>
        /// <param name="startExit">The starting exit.</param>
        /// <param name="endExit">The ending exit.</param>
        public void ConnectExits(Exit startExit, Exit endExit) {
            if (startExit.transform.parent == endExit.transform.parent) {
                Debug.LogWarning("Cannot connect exits within the same room.");
                return;
            }

            var startPos = startExit.worldPosition;
            var endPos = endExit.worldPosition;

            // Check for overlap before creating the corridor
            if (IsPathOverlappingRooms(startPos, endPos)) {
                Debug.LogWarning($"Cannot connect exits due to overlap: {startPos} to {endPos}");
                PlaceDoor(startExit);
                PlaceDoor(endExit);
                return;
            }

            // Create L-shaped connection
            var midpoint = startPos + endPos / 2;
            //var midpoint = new Vector3(startPos.x, startPos.y, endPos.z);
            CreateCorridor(startPos, midpoint);
            CreateCorridor(midpoint, endPos);

            // Mark exits as connected
            startExit.isConnected = true;
            endExit.isConnected = true;

            Debug.Log($"Connected exits: {startPos} -> {endPos}");
        }

        /// <summary>
        /// Instantiates corridor on the path between two points.
        /// </summary>
        public void CreatePath(List<Vector2> path, Vector3 gridOrigin)
        {
            for (var i = 0; i < path.Count; i++)
            {
                var position = _dungeonGenerator.GridSystem.GetCellWorldPosition(path[i].x, path[i].y, gridOrigin);
                Instantiate(corridorPrefab, position, Quaternion.identity);
            }
        }
        /// <summary>
        /// Creates a corridor segment between two points.
        /// </summary>
        /// <param name="start">The starting point of the corridor.</param>
        /// <param name="end">The ending point of the corridor.</param>
        private void CreateCorridor(Vector3 start, Vector3 end) {
            var direction = (end - start).normalized;
            var distance = Vector3.Distance(start, end);

            // Place corridor segments
            var segmentCount = Mathf.CeilToInt(distance / corridorPrefab.transform.localScale.z); // Adjust for prefab length
            for (var i = 0; i < segmentCount; i++) {
                var position = start + direction * (i * corridorPrefab.transform.localScale.z);

                // Correctly align the corridor segment
                var rotation = Quaternion.LookRotation(direction);
                Instantiate(corridorPrefab, position, rotation);
            }
        } 
        /// <summary>
        /// Checks if a path between two points overlaps with any existing rooms.
        /// </summary>
        /// <param name="start">The starting point of the path.</param>
        /// <param name="end">The ending point of the path.</param>
        /// <returns>True if the path overlaps with any rooms, false otherwise.</returns>
        public bool IsPathOverlappingRooms(Vector3 start, Vector3 end) {
            foreach (var room in FindObjectsOfType<Room>()) {
                var roomBounds = room.GetComponent<Collider>().bounds;
                var pathBounds = new Bounds(
                    (start + end) / 2, 
                    new Vector3(Mathf.Abs(end.x - start.x), 1, Mathf.Abs(end.z - start.z))
                );

                if (roomBounds.Intersects(pathBounds)) {
                    return true; // Path overlaps a room
                }
            }
            return false;
        }
        /// <summary>
        /// Places a door at the specified exit.
        /// </summary>
        /// <param name="exit">The exit where the door will be placed.</param>
        private void PlaceDoor(Exit exit) {
            Instantiate(corridorPrefab, exit.worldPosition, Quaternion.LookRotation(exit.direction));
        }
    }
}