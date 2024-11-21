using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Scripts.Gameplay.PCG
{
    /// <summary>
    /// Generates the dungeon layout, including rooms and corridors.
    /// </summary>
    public class DungeonGenerator : MonoBehaviour
    {
        /// <summary>
        /// Reference to the RoomManager component.
        /// </summary>
        [SerializeField] private RoomManager roomManager;
        /// <summary>
        /// Reference to the CorridorManager component.
        /// </summary> 
        [SerializeField] private CorridorManager corridorManager;
        /// <summary>
        /// Prefab for the starting room.
        /// </summary>
        public GameObject startRoomPrefab;
        /// <summary>
        /// Array of exploration room prefabs.
        /// </summary>
        public Room[] explorationRooms;
        /// <summary>
        /// Prefab for the boss room.
        /// </summary>
        public GameObject bossRoomPrefab;
        /// <summary>
        /// Number of exploration rooms to generate.
        /// </summary>
        public int explorationRoomCount;
        /// <summary>
        /// Gets the grid system used for cell-based placement.
        /// </summary>
        public GridSystem GridSystem { get; private set; }
        /// <summary>
        /// Gets the origin point of the grid in the world.
        /// </summary>
        public Vector3 GridOrigin { get; private set; }

        /// <summary>
        /// Initializes the grid system based on the plane's size.
        /// </summary>
        private void Awake()
        {
            // Assuming the plane is scaled in X and Z axes
            var planeSizeX = transform.localScale.x * 10; 
            var planeSizeZ = transform.localScale.z * 10;

            // Initialize the GridSystem
            GridSystem = new GridSystem(
                Mathf.FloorToInt(planeSizeX / 20), // Divide plane width by cell size
                Mathf.FloorToInt(planeSizeZ / 20), // Divide plane height by cell size
                20
            );
            GridOrigin = transform.position - new Vector3(planeSizeX / 2, 0, planeSizeZ / 2); // Bottom-left corner
        }
        /// <summary>
        /// Starts the dungeon generation process.
        /// </summary>
        private void Start()
        {
            GenerateDungeon();
        }
        /// <summary>
        /// Generates the dungeon layout, including placing rooms and connecting them with corridors.
        /// </summary>
        private void GenerateDungeon() {
            Debug.Log("Starting dungeon generation...");

            // Start room (bottom-left of the grid)
            var startRoom = startRoomPrefab.GetComponent<Room>();
            var startPosition = GridOrigin; // Bottom-left corner
            roomManager.PlaceRoom(startRoom, startPosition);
            Debug.Log($"Placed Start Room at {startPosition}");

            // Boss room (top-right of the grid)
            var bossRoom = bossRoomPrefab.GetComponent<Room>();
            var bossPosition = GridOrigin + new Vector3(
                (GridSystem.GridWidth - Mathf.Ceil(startRoom.size.x / GridSystem.CellSize)) * GridSystem.CellSize,
                0,
                (GridSystem.GridHeight - Mathf.Ceil(startRoom.size.z / GridSystem.CellSize)) * GridSystem.CellSize
            );
            roomManager.PlaceRoom(bossRoom, bossPosition);
            Debug.Log($"Placed Boss Room at {bossPosition}");

            // Exploration rooms
            for (var i = 0; i < explorationRoomCount; i++) {
                var explorationRoomPrefab = explorationRooms[Random.Range(0, explorationRooms.Length)];
                var explorationRoom = explorationRoomPrefab.GetComponent<Room>();

                var position = roomManager.FindValidPosition(explorationRoom);
                if (position != Vector3.zero) {
                    roomManager.PlaceRoom(explorationRoom, position);
                } else {
                    Debug.LogError($"Failed to place Exploration Room {i} - no valid position found!");
                }
            }

            // Connect rooms
            ConnectAllRooms();
        }
        /// <summary>
        /// Connects all rooms in the dungeon with corridors.
        /// </summary>
        private void ConnectAllRooms() {
            foreach (var room in roomManager.rooms) {
                var hasConnectedExit = false;

                foreach (var exit in room.Exits)
                {
                    if (!exit.isConnected) continue;
                    hasConnectedExit = true;
                    break; // At least one connection exists
                }

                // Ensure at least one exit is connected
                if (hasConnectedExit) continue;
                {
                    foreach (var exit in room.Exits) {
                        var closestExit = roomManager.FindClosestUnconnectedExit(exit);
                        if (closestExit == null) continue;
                        Debug.Log($"Connecting exit at {exit.worldPosition} to closest exit at {closestExit.worldPosition}");
                        corridorManager.ConnectExits(exit, closestExit);
                        break;
                    }
                }
            }
        }
        
        private void OnDrawGizmos() {
            if (GridSystem == null) return;
            for (var x = 0; x <= GridSystem.GridWidth; x++) {
                for (var y = 0; y <= GridSystem.GridHeight; y++) {
                    var cellPos = GridSystem.GetCellWorldPosition(x, y, GridOrigin);
                    Gizmos.color = Color.green;
                    Gizmos.DrawWireCube(cellPos, new Vector3(GridSystem.CellSize, 0.1f, GridSystem.CellSize));
                }
            }
        }
    }
}
