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
        public GameObject corridorPrefab; // Prefab used for creating corridor segments.
        private DungeonGenerator _dungeonGenerator; // Reference to the dungeon generator
        public List<GameObject> corridors; // List of all corridor segments in the dungeon.
        public GameObject doorPrefab; // Prefab used for creating doors.
        private RoomManager _roomManager; // Reference to the room manager.
        
        /// <summary>
        /// Initializes the required components.
        /// </summary>
        private void Awake() {
            _dungeonGenerator = GetComponent<DungeonGenerator>();
            corridors = new List<GameObject>();
            _roomManager = GetComponent<RoomManager>();
        }
        
        /// <summary>
        /// Instantiates corridor on the path between two points.
        /// </summary>
        public void CreatePath(List<Vector2> path, Vector3 gridOrigin)
        {                                
            for (var i = 0; i < path.Count - 1; i++)
            {
                var position = _dungeonGenerator.GridSystem.GetCellWorldPosition(path[i].x, path[i].y, gridOrigin);
                corridors.Add(Instantiate(corridorPrefab, position, Quaternion.identity)); 
            }
        }
        
        /// <summary>
        /// Method to remove overlapping corridor walls.
        /// </summary>
        public void RemoveOverlappingCorridorWalls()
        {
            foreach (var room in _roomManager.rooms)
            {
                foreach (var exit in room.Exits)
                {
                    foreach (Transform child in exit.transform)
                    {
                        if (child.CompareTag($"Corridor"))
                        {
                            var corridor = child.gameObject;
                            corridors.Add(corridor);
                        }
                    }
                }
            }

            for (var i = 0; i < corridors.Count; i++)
            {
                var corridor = corridors[i];
                for (var j = 0; j < corridors.Count; j++)
                {
                    if (i == j) continue;
                    var otherCorridor = corridors[j];
                    CheckOverlappingWalls(corridor, otherCorridor);
                }
            }
        }
        
        /// <summary>
        /// Method to check if the walls of two corridors are overlapping.
        /// </summary>
        /// <param name="corridor"></param>
        /// <param name="otherCorridor"></param>
        private void CheckOverlappingWalls(GameObject corridor, GameObject otherCorridor)
        {
            foreach (Transform wallChild in corridor.transform)
            {
                foreach (Transform wallChildOfOtherCorridor in otherCorridor.transform)
                {
                    if (wallChild.CompareTag($"Wall") && wallChildOfOtherCorridor.CompareTag($"Wall"))
                    {
                        var wall = wallChild.gameObject;
                        var otherWall = wallChildOfOtherCorridor.gameObject;
                        if (IsWallsOverlapping(wall.GetComponent<BoxCollider>().bounds, otherWall))
                        {
                            wall.gameObject.SetActive(false);
                            otherWall.gameObject.SetActive(false);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Check if the corridor is overlapping with any adjacent walls.
        /// </summary>
        /// <param name="wallOneBounds"></param>
        /// <param name="wallTwo"></param>
        /// <returns></returns>
        private bool IsWallsOverlapping(Bounds wallOneBounds, GameObject wallTwo)
        {
            var wallTwoCollider = wallTwo.GetComponent<BoxCollider>();
            return wallOneBounds.Intersects(wallTwoCollider.bounds);
        }
        /// <summary>
        /// Method to close all unconnected rooms with doors.
        /// </summary>
        public void CloseUnconnectedRooms()
        {
            foreach (var room in _roomManager.rooms)
            {
                if(room.roomType == RoomType.Start)
                {
                    continue;
                }
                foreach (var exit in room.Exits)
                {
                    if (!exit.isConnected)
                    {
                        var rotation = exit.transform.localRotation.eulerAngles.y switch
                        {
                            0 => Quaternion.Euler(0, 0, 0),
                            180 => Quaternion.Euler(0, -90, 0),
                            _ => default
                        };
                        Instantiate(doorPrefab, exit.worldPosition, rotation);
                        foreach (Transform childExit in exit.transform)
                        {
                            childExit.gameObject.SetActive(false);  
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Method to clean up corridors that are inside rooms.
        /// </summary>
        public void CleanUp()
        {
            foreach (var corridor in corridors)
            {
                foreach (var room in _roomManager.rooms)
                {
                    var roomWidth = room.size.x;
                    var roomHeight = room.size.z;
                    if(corridor.transform.position.x >= room.transform.position.x - roomWidth / 2 
                       && corridor.transform.position.x <= room.transform.position.x + roomWidth / 2 
                       && corridor.transform.position.z >= room.transform.position.z - roomHeight / 2 
                       && corridor.transform.position.z <= room.transform.position.z + roomHeight / 2)
                    {
                        Destroy(corridor);
                    }
                }
            }
        }
    }
}