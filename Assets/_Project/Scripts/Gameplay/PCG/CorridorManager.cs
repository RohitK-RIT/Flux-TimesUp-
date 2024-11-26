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
        
        private void Awake() {
            _dungeonGenerator = GetComponent<DungeonGenerator>();
        }
        
        /// <summary>
        /// Instantiates corridor on the path between two points.
        /// </summary>
        public void CreatePath(List<Vector2> path, Vector3 gridOrigin)
        {                                
            for (var i = 0; i < path.Count; i++)
            {
                /*var current = path[i];
                var next = path[i + 1];*/
                
                var position = _dungeonGenerator.GridSystem.GetCellWorldPosition(path[i].x, path[i].y, gridOrigin);
                corridors.Add(Instantiate(corridorPrefab, position, Quaternion.identity));

                /*//Determine the direction of the path to close the corridor with a wall
                if (next.x > current.x)
                {
                    corridorPrefab.transform.Find("WallW").gameObject.SetActive(true);
                    corridorPrefab.transform.Find("WallE").gameObject.SetActive(true);
                    corridorPrefab.transform.Find("WallN").gameObject.SetActive(false);
                    corridorPrefab.transform.Find("WallS").gameObject.SetActive(false);
                }
                else if (next.x < current.x)
                {
                    corridorPrefab.transform.Find("WallE").gameObject.SetActive(true);
                    corridorPrefab.transform.Find("WallW").gameObject.SetActive(true);
                    corridorPrefab.transform.Find("WallN").gameObject.SetActive(false);
                    corridorPrefab.transform.Find("WallS").gameObject.SetActive(false);
                }
                else if (next.y > current.y)
                {
                    corridorPrefab.transform.Find("WallS").gameObject.SetActive(true);
                    corridorPrefab.transform.Find("WallN").gameObject.SetActive(true);
                    corridorPrefab.transform.Find("WallE").gameObject.SetActive(false);
                    corridorPrefab.transform.Find("WallW").gameObject.SetActive(false);
                }
                else if (next.y < current.y)
                {
                    corridorPrefab.transform.Find("WallN").gameObject.SetActive(true);
                    corridorPrefab.transform.Find("WallS").gameObject.SetActive(true);
                    corridorPrefab.transform.Find("WallE").gameObject.SetActive(false);
                    corridorPrefab.transform.Find("WallW").gameObject.SetActive(false);
                }
                */

            }
        }
    }
}