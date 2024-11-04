using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Project.Scripts.Gameplay.PCG
{
    public class DungeonGenerator : MonoBehaviour
    {
        [SerializeField] private GameObject[] roomPrefabs; // Array of room prefabs
        [SerializeField] private GameObject bossRoomPrefab;
        [SerializeField] private int playerCount; // Number of players

        private List<Vector3> _occupiedPositions = new List<Vector3>();
        private Vector2 _planeDimensions; // Calculated dimensions based on the plane's scale

        private void Start()
        {
            SetPlaneDimensions();
            GenerateDungeon();
        }

        private void SetPlaneDimensions()
        {
            GameObject planeObject = GameObject.Find("Plane");
            if (planeObject != null)
            {
                Vector3 scale = planeObject.transform.localScale;
                // Assuming the plane size is 10x10 units at scale 1, adjust accordingly if different
                _planeDimensions = new Vector2(10 * scale.x, 10 * scale.z);
            }
            else
            {
                Debug.LogWarning("Plane object not found. Using default dimensions.");
                _planeDimensions = new Vector2(100, 100); // Default size if "Plane" is missing
            }
        }

        private void GenerateDungeon()
        {
            PlaceBossRoom();
            var roomCount = GetRoomCount();
        
            // Create a list of possible grid positions based on the calculated plane dimensions
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
                // Set parent as plane
                var instantiatedRoom = Instantiate(roomPrefab, position, Quaternion.identity);
                instantiatedRoom.transform.parent = GameObject.Find("Plane").transform;
            }
        }

        private void PlaceBossRoom()
        {
            var bossRoomPosition = Vector3.zero; // Place at the center or a specific offset
            var bossRoomSize = CalculateRoomSize(bossRoomPrefab);

            if (!IsOverlapping(bossRoomPosition, bossRoomSize))
            {
                _occupiedPositions.Add(bossRoomPosition);
                var instantiatedBossRoom = Instantiate(bossRoomPrefab, bossRoomPosition, Quaternion.identity);
                instantiatedBossRoom.transform.parent = GameObject.Find("Plane").transform;
            }
            else
            {
                Debug.LogWarning("Could not place boss room at the desired position due to overlap.");
            }
        }

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

        private List<Vector3> GenerateGridPositions()
        {
            var positions = new List<Vector3>();
        
            var xCount = Mathf.FloorToInt(_planeDimensions.x / 20); // Approximate grid spacing
            var zCount = Mathf.FloorToInt(_planeDimensions.y / 20); // Adjust as needed

            for (var x = 0; x < xCount; x++)
            {
                for (var z = 0; z < zCount; z++)
                {
                    var posX = x * 20 - _planeDimensions.x / 2 + 10; // Adjust spacing
                    var posZ = z * 20 - _planeDimensions.y / 2 + 10;
                    positions.Add(new Vector3(posX, 0, posZ));
                }
            }

            return positions;
        }

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

        private bool IsOverlapping(Vector3 position, Vector3 roomSize)
        {
            return _occupiedPositions.Any(occupiedPosition => Vector3.Distance(occupiedPosition, position) < Mathf.Max(roomSize.x, roomSize.z));
        }
    }
}