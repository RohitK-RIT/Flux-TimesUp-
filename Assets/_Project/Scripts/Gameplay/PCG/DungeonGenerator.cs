using System.Collections.Generic;
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
                Mathf.FloorToInt(planeSizeX / 5), // Divide plane width by cell size
                Mathf.FloorToInt(planeSizeZ / 5), // Divide plane height by cell size
                5
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
            var startPosition = GridOrigin + new Vector3(startRoom.size.x / 2, 0, startRoom.size.z / 2); // Bottom-left corner
            roomManager.PlaceRoom(startRoom, startPosition);
            Debug.Log($"Placed Start Room at {startPosition}");

            // Boss room (top-right of the grid)
            var bossRoom = bossRoomPrefab.GetComponent<Room>();
            var bossPosition = GridOrigin + new Vector3(
                (GridSystem.GridWidth - Mathf.Ceil(bossRoom.size.x / GridSystem.CellSize)) * GridSystem.CellSize,
                0,
                (GridSystem.GridHeight - Mathf.Ceil(bossRoom.size.z / GridSystem.CellSize)) * GridSystem.CellSize
            );
            bossPosition += new Vector3(bossRoom.size.x / 2, 0, bossRoom.size.z / 2);
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
            ConnectAllRooms();
        }

        private void ConnectAllRooms()
        {
            foreach (var room in roomManager.rooms)
            {
                foreach(var exit in room.Exits)
                {
                    if(exit.isConnected)
                    {
                        continue;
                    }
                    var closestExit = roomManager.FindClosestUnconnectedExit(exit);
                    if(closestExit == null)
                    {
                        continue;
                    }
                    var path = GetPath(new Vector2(exit.worldPosition.x, exit.worldPosition.z), new Vector2(closestExit.worldPosition.x, closestExit.worldPosition.z));
                    if (path == null || path.Count == 0)
                    {
                        Debug.Log("Unable to find path between exits" + exit.worldPosition + " and " + closestExit.worldPosition);
                        continue;
                    }
                    Debug.Log($"Path from {exit.worldPosition} to {closestExit.worldPosition}: {string.Join(" -> ", path)}");
                    GridSystem.ResetVisitedCells();
                    exit.isConnected = true;
                    closestExit.isConnected = true;
                }
            }
        }
        
        private List<Vector2> GetPath(Vector2 start, Vector2 end)
        {
            var startX = (int)start.x; 
            var startY = (int)start.y;
            var endX = (int)end.x;
            var endY = (int)end.y;
            
            Vector2 startIndexes = GridSystem.GetGridCellPositionFromWorldPosition(startX, startY, GridOrigin);
            Vector2 endIndexes = GridSystem.GetGridCellPositionFromWorldPosition(endX, endY, GridOrigin);
            
            List<Vector2> result = GeneratePathIterative((int) startIndexes.x, (int) startIndexes.y, (int) endIndexes.x, (int) endIndexes.y);
            
            return result;
        }
        
        /*private List<Vector2> GeneratePathRecursive(int startX, int startY, int endX, int endY)
        {
            GridSystem.VisitedCells[startX, startY] = true;

            if (startX == endX && startY == endY)
            {
                return new List<Vector2> {new Vector2(startX, startY)};
            }
            GridSystem.VisitedCells[startX, startY] = true;
                var directions = new List<Vector2>
            {
                new Vector2(0, 1),
                new Vector2(1, 0),
                new Vector2(0, -1),
                new Vector2(-1, 0)
            };
            foreach (var direction in directions)
            {
                var newX = startX + (int)direction.x;
                var newY = startY + (int)direction.y;
                if (newX < 0 || newX >= GridSystem.VisitedCells.GetLength(0) || newY < 0 ||  newY >= GridSystem.VisitedCells.GetLength(1))
                {
                    continue;
                }
                if (GridSystem.VisitedCells[newX, newY])
                {
                    continue;
                }
                var path = GeneratePathRecursive(newX, newY, endX, endY);
                if (path != null)
                {
                    path.Insert(0, new Vector2(startX, startY));
                    return path;
                }
            }
            return null;
        }*/
        
        private List<Vector2> GeneratePathIterative(int startX, int startY, int endX, int endY)
        {
            int rows = GridSystem.VisitedCells.GetLength(0);
            int cols = GridSystem.VisitedCells.GetLength(1);
    
            // Direction vectors: (0, 1) = North, (1, 0) = East, (0, -1) = South, (-1, 0) = West
            Vector2[] directions = new Vector2[]
            {
                new Vector2(0, 1),
                new Vector2(1, 0),
                new Vector2(0, -1),
                new Vector2(-1, 0)
            };

            // Queue for BFS, storing the current cell and the path taken to reach it
            Queue<(Vector2 position, List<Vector2> path)> queue = new Queue<(Vector2, List<Vector2>)>();

            // Initialize BFS with the starting position
            queue.Enqueue((new Vector2(startX, startY), new List<Vector2> { new Vector2(startX, startY) }));
            GridSystem.VisitedCells[startX, startY] = true;

            while (queue.Count > 0)
            {
                var (current, path) = queue.Dequeue();
                int currentX = (int)current.x;
                int currentY = (int)current.y;

                // Check if we've reached the destination
                if (currentX == endX && currentY == endY)
                {
                    return path;
                }

                // Explore neighbors
                foreach (var direction in directions)
                {
                    int newX = currentX + (int)direction.x;
                    int newY = currentY + (int)direction.y;
                    
                    if (newX == endX && newY == endY)
                    {
                        return new List<Vector2>(path) { new Vector2(newX, newY) };
                    }
                    

                    // Check boundaries and if cell is unvisited
                    if (newX >= 0 && newX < rows && newY >= 0 && newY < cols && !GridSystem.VisitedCells[newX, newY] && !GridSystem.IsCellOccupied(newX, newY))
                    {
                        GridSystem.VisitedCells[newX, newY] = true;
                        var newPath = new List<Vector2>(path) { new Vector2(newX, newY) };
                        queue.Enqueue((new Vector2(newX, newY), newPath));
                    }
                }
            }

            // If no path is found
            return null;
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
            GridSystem.DrawOccupiedCellsGizmos(GridOrigin);
        }
    }
}
