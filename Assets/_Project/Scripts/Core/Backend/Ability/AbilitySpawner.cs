using System;
using System.Collections;
using System.Linq;
using _Project.Scripts.Core.Enemy;
using _Project.Scripts.Core.Weapons.Abilities;
using _Project.Scripts.Gameplay.PCG;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace _Project.Scripts.Core.Backend.Ability
{
    /// <summary>
    /// Spawns random abilities in the room.
    /// </summary>
    public static class AbilitySpawner
    {
        /// <summary>
        /// Layer mask for the ground.
        /// </summary>
        private static readonly LayerMask GroundLayer = LayerMask.GetMask("Ground");

        /// <summary>
        /// Spawns random abilities in the room.
        /// </summary>
        /// <param name="room">room to spawn the abilities in</param>
        public static void SpawnRandomAbilities(Room room)
        {
            room.StartCoroutine(SpawnRandomAbilitiesAsync(room));
        }

        /// <summary>
        /// Coroutine to spawn random abilities in the room.
        /// </summary>
        /// <param name="room">room to spawn the abilities in</param>
        private static IEnumerator SpawnRandomAbilitiesAsync(Room room)
        {
            // Get the room size and position
            var roomSize = room.size;
            var roomPosition = room.transform.position;
            // Randomly determine the number of abilities to spawn
            var numberOfAbilitiesToSpawn = Random.Range(2, 5);

            Debug.Log($"Spawning {numberOfAbilitiesToSpawn} abilities in room {room.name}", room);

            // Spawn the abilities
            for (var i = 0; i < numberOfAbilitiesToSpawn;)
            {
                // Randomly determine the ability type
                var abilityType = Random.Range(0, 2) == 0 ? AbilityType.Heal : AbilityType.Shield;
                // Calculate a random spawn position within the room
                var roomSpawnPosition = new Vector3(Random.Range(-roomSize.x / 2, roomSize.x / 2), 1f, Random.Range(-roomSize.z / 2, roomSize.z / 2));
                var spawnPosition = roomPosition + roomSpawnPosition;

                if (IsValidSpawnPosition(spawnPosition))
                {
                    SpawnAbility(room, abilityType, spawnPosition);
                    i++; // Increment the counter
                }
                else
                {
                    // If the spawn position is not valid, try again next frame
                    yield return null;
                }
            }

            Debug.Log($"Finished spawning abilities in room {room.name}", room);
            EditorApplication.isPaused = true;
            yield break;
        }

        /// <summary>
        /// Checks if the spawn position is valid.
        /// </summary>
        /// <param name="position">position to check if walkable</param>
        /// <returns>if the given position is over the ground layer</returns>
        private static bool IsValidSpawnPosition(Vector3 position)
        { 
            // Raycast and check if the position is walkable
            return Physics.Raycast(position, Vector3.down, out _, 5f, GroundLayer);
        }

        /// <summary>
        /// Spawns the ability in the room.
        /// </summary>
        /// <param name="room"></param>
        /// <param name="type"></param>
        /// <param name="position"></param>
        private static void SpawnAbility(Room room, AbilityType type, Vector3 position)
        {
            // Get the ability pickup prefab
            var abilityPrefab = AbilityDataSystem.Instance.GetAbilityPickupPrefab(type);
            // If the prefab is null, return
            if (!abilityPrefab)
                return;

            // Instantiate the ability pickup prefab
            GameObject.Instantiate(abilityPrefab, position, Quaternion.identity, room.transform);
        }
    }
}