using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Project.Scripts.Gameplay.PCG
{
    /// <summary>
    /// Generates a dungeon layout by placing rooms and a boss room on a plane.
    /// </summary>
    public class DungeonGenerator : MonoBehaviour
    {
        /// <summary>
        /// Array of room prefabs to be used in the dungeon.
        /// </summary>
        [SerializeField] private GameObject[] roomPrefabs;
        /// <summary>
        /// Prefab for the boss room.
        /// </summary>
        [SerializeField] private GameObject bossRoomPrefab;
        /// <summary>
        /// Number of players, which influences the number of rooms generated.
        /// </summary>
        [SerializeField] private int playerCount;
        /// <summary>
        /// List of positions that are already occupied by rooms.
        /// </summary>
        private List<Vector3> _occupiedPositions = new List<Vector3>();
        /// <summary>
        /// Dimensions of the plane on which the dungeon is generated.
        /// </summary>
        private Vector2 _planeDimensions; 
        
        private void Start()
        {
            SetDungeonDimensions();
            GenerateDungeon();
        }
        
        /// <summary>
        /// Sets the dimensions of the dungeon based on its scale.
        /// This is will modified in the future dynamically based on the number of players connected in the game.
        /// </summary>
        private void SetDungeonDimensions()
        {
            var planeObject = GameObject.Find("Dungeon");
            if (planeObject != null)
            {
                var scale = planeObject.transform.localScale;
                _planeDimensions = new Vector2(10 * scale.x, 10 * scale.z);
            }
            else
            {
                Debug.LogWarning("Plane object not found. Using default dimensions.");
                _planeDimensions = new Vector2(100, 100); // Default size if "Plane" is missing
            }
        }
        /// <summary>
        /// Generates the dungeon by placing rooms and the boss room.
        /// </summary>
        private void GenerateDungeon()
        {
            PlaceBossRoom();
            var roomCount = GetRoomCount();
        
            // Create a list of possible grid positions based on the calculated dungeon dimensions
            var availablePositions = GenerateGridPositions();

            // Randomly place composite rooms without overlap
            for (var i = 0; i < roomCount; i++)
            {
                if (availablePositions.Count == 0)
                {
                    Debug.LogWarning("No more available positions for rooms.");
                    break;
                }

                // Choose a random position from the available grid positions
                var randomIndex = Random.Range(0, availablePositions.Count);
                var position = availablePositions[randomIndex];
                availablePositions.RemoveAt(randomIndex); // Remove position to avoid reuse

                // Randomly select a room prefab from the array
                var roomPrefab = roomPrefabs[Random.Range(0, roomPrefabs.Length)];
            
                // Calculate the room size based on the number of tiles under the "Ground" parent
                var roomSize = CalculateRoomSize(roomPrefab);
            
                // Check for overlap and instantiate if there's no overlap
                if (IsOverlapping(position, roomSize)) continue;
                _occupiedPositions.Add(position);
                // Set parent as dungeon
                var instantiatedRoom = Instantiate(roomPrefab, position, Quaternion.identity);
                instantiatedRoom.transform.parent = GameObject.Find("Dungeon").transform;
            }
        }
        /// <summary>
        /// Places the boss room at a specific position.
        /// </summary>
        private void PlaceBossRoom()
        {
            var bossRoomPosition = Vector3.zero; // Place at the center or a specific offset
            var bossRoomSize = CalculateRoomSize(bossRoomPrefab);

            if (!IsOverlapping(bossRoomPosition, bossRoomSize))
            {
                _occupiedPositions.Add(bossRoomPosition);
                var instantiatedBossRoom = Instantiate(bossRoomPrefab, bossRoomPosition, Quaternion.identity);
                instantiatedBossRoom.transform.parent = GameObject.Find("Dungeon").transform;
            }
            else
            {
                Debug.LogWarning("Could not place boss room at the desired position due to overlap.");
            }
        }
        /// <summary>
        /// Determines the number of rooms to generate based on the player count.
        /// </summary>
        /// <returns>The number of rooms to generate.</returns>
        private int GetRoomCount()
        {
            return playerCount switch
            {
                1 => 7,
                2 => 13,
                3 => 15,
                _ => 7
            };
        }
        /// <summary>
        /// Generates a list of possible grid positions for room placement.
        /// </summary>
        /// <returns>A list of possible grid positions.</returns>
        private List<Vector3> GenerateGridPositions()
        {
            var positions = new List<Vector3>();
        
            var xCount = Mathf.FloorToInt(_planeDimensions.x / 20); // Approximate grid spacing
            var zCount = Mathf.FloorToInt(_planeDimensions.y / 20); // to be adjusted based on level designs.

            for (var x = 0; x < xCount; x++)
            {
                for (var z = 0; z < zCount; z++)
                {
                    var posX = x * 20 - _planeDimensions.x / 2 + 10; // to be adjusted based on level designs.
                    var posZ = z * 20 - _planeDimensions.y / 2 + 10;
                    positions.Add(new Vector3(posX, 0, posZ));
                }
            }

            return positions;
        }
        /// <summary>
        /// Calculates the size of a room based on its prefab.
        /// </summary>
        /// <param name="roomPrefab">The room prefab.</param>
        /// <returns>The size of the room.</returns>
        private static Vector3 CalculateRoomSize(GameObject roomPrefab)
        {
            var ground = roomPrefab.transform.Find("Ground");
            if (ground == null) return new Vector3(5f, 1, 4f); // Default size if "Ground" is missing
            var tileCount = ground.childCount;
            var tilesPerRow = Mathf.CeilToInt(Mathf.Sqrt(tileCount));
            var roomWidth = tilesPerRow * 5f;
            var roomDepth = Mathf.Ceil(tileCount / (float)tilesPerRow) * 4f;
            return new Vector3(roomWidth, 1, roomDepth);
        }
        /// <summary>
        /// Checks if a room overlaps with any existing rooms.
        /// </summary>
        /// <param name="position">The position to check.</param>
        /// <param name="roomSize">The size of the room.</param>
        /// <returns>True if there is an overlap, false otherwise.</returns>
        private bool IsOverlapping(Vector3 position, Vector3 roomSize)
        {
            return _occupiedPositions.Any(occupiedPosition => Vector3.Distance(occupiedPosition, position) < Mathf.Max(roomSize.x, roomSize.z));
        }
    }
}