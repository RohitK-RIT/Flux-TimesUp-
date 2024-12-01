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
        public List<GameObject> corridors;
        public GameObject doorPrefab;
        private RoomManager _roomManager;
        
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
            for (var i = 0; i < path.Count; i++)
            {
                var position = _dungeonGenerator.GridSystem.GetCellWorldPosition(path[i].x, path[i].y, gridOrigin);
                corridors.Add(Instantiate(corridorPrefab, position, Quaternion.identity)); 
            }
        }
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