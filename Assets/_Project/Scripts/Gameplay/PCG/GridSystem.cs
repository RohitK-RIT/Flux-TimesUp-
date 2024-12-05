﻿using Unity.VisualScripting;
using UnityEngine;

namespace _Project.Scripts.Gameplay.PCG {
    /// <summary>
    /// Represents a grid system used for managing cell-based placement within the dungeon.
    /// </summary>
    public class GridSystem { 
        /// <summary>
        /// Gets the width of the grid.
        /// </summary>
        public int GridWidth { get; }
        /// <summary>
        /// Gets the height of the grid.
        /// </summary>
        public int GridHeight { get; }
        /// <summary>
        /// Gets the size of each cell in the grid.
        /// </summary>
        public float CellSize { get; }
        /// <summary>
        /// Array to track occupied cells within the grid.
        /// </summary>
        private readonly bool[,] _occupiedCells;
        /// <summary>
        /// Initializes a new instance of the <see cref="GridSystem"/> class with the specified dimensions and cell size.
        /// </summary>
        /// <param name="width">The width of the grid.</param>
        /// <param name="height">The height of the grid.</param>
        /// <param name="cellSize">The size of each cell in the grid.</param>
        public GridSystem(int width, int height, float cellSize) {
            GridWidth = width;
            GridHeight = height;
            this.CellSize = cellSize;
            _occupiedCells = new bool[width, height];
        }
        /// <summary>
        /// Marks the specified cell as occupied.
        /// </summary>
        /// <param name="x">The x-coordinate of the cell.</param>
        /// <param name="y">The y-coordinate of the cell.</param>
        public void MarkCellOccupied(int x, int y, Vector3 gridOrigin) {
            var startX = gridOrigin.x;
            var endX = gridOrigin.x + (GridWidth*CellSize);
            var startY = gridOrigin.z;
            var endY = gridOrigin.z + (GridHeight*CellSize);
            if (x < startX || x > endX || y < startY || y > endY) {
                Debug.LogWarning($"Cannot mark cell outside of grid bounds: {x}, {y}");
                return;
            }
            // Calculate the cell indices
            x = Mathf.FloorToInt((x - gridOrigin.x) / CellSize);
            y = Mathf.FloorToInt((y - gridOrigin.z) / CellSize);
            // Clamp the indices to ensure they are within bounds
            /*x = Mathf.Clamp(x, 0, GridWidth - 1);
            y = Mathf.Clamp(y, 0, GridHeight - 1);*/
            //Clamp with the list size
            x = Mathf.Clamp(x, 0, _occupiedCells.GetLength(0) - 1);
            y = Mathf.Clamp(y, 0, _occupiedCells.GetLength(1) - 1);
            
            _occupiedCells[x, y] = true;
            
            

        }
        /// <summary>
        /// Checks if the specified cell is occupied.
        /// </summary>
        /// <param name="x">The x-coordinate of the cell.</param>
        /// <param name="y">The y-coordinate of the cell.</param>
        /// <returns>True if the cell is occupied, false otherwise.</returns>
        public bool IsCellOccupied(int x, int y) {
            // Clamp the indices to ensure they are within bounds
            x = Mathf.Clamp(x, 0, GridWidth - 1);
            y = Mathf.Clamp(y, 0, GridHeight - 1);
            return _occupiedCells[x, y];
        }
        /// <summary>
        /// Gets the world position of the specified cell.
        /// </summary>
        /// <param name="x">The x-coordinate of the cell.</param>
        /// <param name="y">The y-coordinate of the cell.</param>
        /// <param name="gridOrigin">The origin point of the grid in the world.</param>
        /// <returns>The world position of the cell.</returns>
        public Vector3 GetCellWorldPosition(int x, int y, Vector3 gridOrigin) {
            return gridOrigin + new Vector3(x * CellSize, 0, y * CellSize);
        }
        public void DrawOccupiedCellsGizmos(Vector3 gridOrigin) {
            Gizmos.color = Color.red;
            for (int x = 0; x < GridWidth; x++) {
                for (int y = 0; y < GridHeight; y++) {
                    if (IsCellOccupied(x, y)) {
                        var cellPos = GetCellWorldPosition(x, y, gridOrigin);
                        Gizmos.DrawCube(cellPos, new Vector3(CellSize, 0.1f, CellSize));
                    }
                }
            }
        }
    }
}