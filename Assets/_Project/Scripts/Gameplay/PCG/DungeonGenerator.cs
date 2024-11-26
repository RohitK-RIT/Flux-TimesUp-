using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace _Project.Scripts.Gameplay.PCG
{
    /// <summary>
    /// Generates the dungeon layout, including rooms and corridors.
    /// </summary>
    public class DungeonGenerator : MonoBehaviour
    {
       
        // Reference to the RoomManager component.
        [SerializeField] private RoomManager roomManager;
     
        /// Reference to the CorridorManager component.
        [SerializeField] private CorridorManager corridorManager;
        
        // Prefab for the starting room.
        public GameObject startRoomPrefab;
        
        // Array of exploration room prefabs.
        public Room[] explorationRooms;
        
        // Prefab for the boss room.
        public GameObject bossRoomPrefab;
        
        // Prefab for the door.
        public GameObject doorPrefab;
        
        
        // Number of exploration rooms to generate.
        public int explorationRoomCount;

        // Gets the grid system used for cell-based placement.
        public GridSystem GridSystem { get; private set; }
        // Gets the origin point of the grid in the world.
        
        public Vector3 GridOrigin { get; private set; }

        private List<Room> _unconnectedRooms = new List<Room>();
        private List<Room> _connectedRooms = new List<Room>();

        /// <summary>
        /// Initializes the grid system based on the plane's size.
        /// </summary>
        private void Awake()
        {
            InitializeGrid();
            
        }

        private void InitializeGrid()
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

            // Start room (bottom-left of the grid)
            var startRoom = startRoomPrefab.GetComponent<Room>();
            var startPosition = GridOrigin + new Vector3(startRoom.size.x / 2, 0, startRoom.size.z / 2); // Bottom-left corner
            roomManager.PlaceRoom(startRoom, startPosition);

            // Boss room (top-right of the grid)
            var bossRoom = bossRoomPrefab.GetComponent<Room>();
            var bossPosition = GridOrigin + new Vector3(
                (GridSystem.GridWidth - Mathf.Ceil(bossRoom.size.x / GridSystem.CellSize)) * GridSystem.CellSize,
                bossRoom.transform.position.y,
                (GridSystem.GridHeight - Mathf.Ceil(bossRoom.size.z / GridSystem.CellSize)) * GridSystem.CellSize
            );
            bossPosition += new Vector3(bossRoom.size.x / 2, 0, bossRoom.size.z / 2);
            roomManager.PlaceRoom(bossRoom, bossPosition);

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
            _connectedRooms.Add(startRoom);
            foreach (var t in roomManager.rooms)
            {
                if(t!=startRoom)
                    _unconnectedRooms.Add(t);
            }
            ConnectAllRooms();
            CloseUnconnectedRooms();
        }

        private void DestroyDungeon()
        {
            foreach (var room in roomManager.rooms)
            {
                Destroy(room.gameObject);
                
            }

            roomManager.rooms.Clear();

            foreach (var corridor in corridorManager.corridors)
            {
                Destroy(corridor);
            }
            corridorManager.corridors.Clear();
        }

        #region AStarSearch
        private List<Vector2> AStarSearch(Vector2 startPos, Vector2 goalPos)
        {
            //Create a list to store the open cells
            var open = new List<Vector2>();

            //Create a list to store the closed cells
            var closed = new List<Vector2>();
            
            //Add the start position to the open list
            open.Add(startPos);
            
            //Dictionary to store the parent of each cell
            var parent = new Dictionary<Vector2,Vector2>();
            
            //Dictionary to store the cost of each cell
            var cost = new Dictionary<Vector2,int>();
            
            //Dictionary to store the g value of each cell
            var g = new Dictionary<Vector2,int>
            {
                [startPos] = 0
            };

            cost[startPos]  = g[startPos] + ManhattanDistance((int)startPos.x, (int)startPos.y, (int)goalPos.x, (int)goalPos.y);
            
            //While the open list is not empty
            while (open.Count != 0)
            {
                //Get the cell with the lowest cost
                var current = open[0];
                foreach (var cell in open)
                {
                    if (cost[cell] < cost[current])
                    {
                        current = cell;
                    }
                }
                //Remove the current cell from the open list
                open.Remove(current);
                
                //Add the current cell to the closed list
                closed.Add(current);
                
                //If the current cell is the goal cell
                if (current == goalPos)
                {
                    //Return the path
                    var path = new List<Vector2>();
                    while (current != startPos)
                    {
                        path.Add(current);
                        current = parent[current];
                    }
                    path.Add(startPos);
                    path.Reverse();
                    return path;
                }
                // Get the neighbours of the current cell
                var neighbours = new List<Vector2>
                {
                    new Vector2(current.x + 1, current.y),
                    new Vector2(current.x - 1, current.y),
                    new Vector2(current.x, current.y+1),
                    new Vector2(current.x, current.y-1)
                };

                foreach (var neighbour in neighbours)
                {
                    if (neighbour == goalPos)
                    {
                        var path = new List<Vector2> { goalPos };
                        var cur = current;
                        while (cur != startPos)
                        {
                            path.Add(cur);
                            cur = parent[cur];
                        }
                        path.Add(startPos);
                        path.Reverse();
                        return path;
                        
                    }
                    //If the neighbour is not walkable or is in the closed list, skip it
                    if (neighbour.x < 0 || neighbour.x >= GridSystem.VisitedCells.GetLength(0) || neighbour.y < 0 || neighbour.y >= GridSystem.VisitedCells.GetLength(1))
                    {
                        continue;
                    }
                    if (GridSystem.IsCellOccupied((int)neighbour.x,(int)neighbour.y) || closed.Contains(neighbour))
                    {
                        continue;
                    }
                    
                    //Calculate the cost of the neighbour
                    var newCost = g[current] + 1;
                    
                    //If the neighbour is not in the open list, add it
                    if (!open.Contains(neighbour))
                    {
                        open.Add(neighbour);
                    }
                    //If the new cost is greater than the cost of the neighbour, skip it
                    if (g.ContainsKey(neighbour) && newCost >= g[neighbour])
                    {
                        continue;
                    }
                    
                    //Set the parent of the neighbour to the current cell
                    parent[neighbour] = current;
                    
                    //Set the cost of the neighbour to the new cost
                    g[neighbour] = newCost;
                    
                    //Set the cost of the neighbour to the new cost + the heuristic
                    cost[neighbour] = g[neighbour] + ManhattanDistance((int)neighbour.x,(int)neighbour.y,(int)goalPos.x,(int)goalPos.y);
                }
            }
            Debug.Log("No path found "+startPos+" to "+goalPos+"!"+closed.Count);
            return null;
        }
        #endregion

        private static int ManhattanDistance(int x1,int y1,int x2,int y2)
        {
            return Math.Abs(x1-x2) + Math.Abs(y1-y2);
        }

        private void ConnectAllRooms()
        {
            var counter = 0;
            while(_unconnectedRooms.Count > 0 && counter<100)
            { 
                foreach (var room in _connectedRooms)
                {
                    var connected = false;
                    foreach (var exit in room.Exits)
                    {
                        if (exit.isConnected)
                        {
                            continue;
                        }
                        var (closestExit, closestRoom) = roomManager.FindClosestUnconnectedExitFromUnconnectedRooms(_unconnectedRooms, exit).First();
                        if (closestExit == null)
                        {
                            continue;
                        }
                        var path = GetPath(new Vector2(exit.worldPosition.x, exit.worldPosition.z), new Vector2(closestExit.worldPosition.x, closestExit.worldPosition.z));
                        if (path == null || path.Count == 0)
                        {
                            continue;
                        }
                        corridorManager.CreatePath(path, GridOrigin);
                        GridSystem.ResetVisitedCells();
                        exit.isConnected = true;
                        closestExit.isConnected = true;
                        _connectedRooms.Add(closestRoom);
                        _unconnectedRooms.Remove(closestRoom);
                        connected = true;
                        break;
                    }
                    if (connected)
                    {
                        break;
                    }
                }
                counter += 1;
            }

            if (counter == 100)
            {
                Debug.Log("Triggerred loop break");
                //Should Retry
                DestroyDungeon();
                InitializeGrid();
                GenerateDungeon();
                return;
            }

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
                    foreach (var cell in path)
                    {
                        GridSystem.GetCellWorldPosition(cell.x, cell.y, GridOrigin);
                        GridSystem.MarkCellOccupied((int)cell.x, (int)cell.y, GridOrigin);
                    }
                    corridorManager.CreatePath(path, GridOrigin);
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
            
            var startIndexes = GridSystem.GetGridCellPositionFromWorldPosition(startX, startY, GridOrigin);
            var endIndexes = GridSystem.GetGridCellPositionFromWorldPosition(endX, endY, GridOrigin);
            
            var result = AStarSearch(startIndexes, endIndexes);
            
            return result;
        }

        private void CloseUnconnectedRooms()
        {
            foreach (var room in roomManager.rooms)
            {
                if(room.roomType == RoomType.Start)
                {
                    continue;
                }
                foreach (var exit in room.Exits)
                {
                    if (!exit.isConnected)
                    {
                        var rotation = exit.transform.rotation.y switch
                        {
                            0 => Quaternion.Euler(0, 0, 0),
                            180 => Quaternion.Euler(0, -90, 0),
                            _ => default
                        };
                        var door = Instantiate(doorPrefab, exit.worldPosition, Quaternion.identity);
                        door.transform.rotation = rotation;
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
            GridSystem.DrawOccupiedCellsGizmos(GridOrigin);
        }
    }
}
