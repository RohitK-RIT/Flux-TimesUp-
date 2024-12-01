using UnityEngine;

namespace _Project.Scripts.Gameplay.PCG
{
    /// <summary>
    /// Defines the types of rooms available in the dungeon.
    /// </summary>
    public enum RoomType {
        Start,
        Exploration,
        Boss
    }
    /// <summary>
    /// Represents a room within the dungeon, containing exits and other properties.
    /// </summary>
    public class Room : MonoBehaviour
    {
        public Exit[] Exits => exits;
        [SerializeField] public Vector3 size;
        [SerializeField] public RoomType roomType;
        [SerializeField] private Exit[] exits;
        /// <summary>
        /// Initializes the room by mapping local exit positions to world positions.
        /// </summary>
        private void Awake()
        {
            foreach (var exit in Exits) {
                exit.worldPosition = transform.position + exit.localPosition;
                Debug.Log($"Exit at {exit.localPosition} mapped to world position {exit.worldPosition}");
            }
        }
    }
}