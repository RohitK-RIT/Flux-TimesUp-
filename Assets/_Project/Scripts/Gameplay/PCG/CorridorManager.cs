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
    }
}