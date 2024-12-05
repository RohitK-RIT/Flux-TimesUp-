using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Core.Enemy;
using _Project.Scripts.Core.Weapons.Abilities;
using _Project.Scripts.Gameplay.PCG;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Scripts.Core.Backend.Ability
{
    /// <summary>
    /// Spawns random abilities in the room.
    /// </summary>
    [RequireComponent(typeof(Room))]
    public class AbilitySpawner : MonoBehaviour
    {
        private Room _room;
        private List<EnemyController> _enemiesInRoom;
        private bool _abilitySpawned;

        private void Awake()
        {
            _room = GetComponent<Room>();
            if (_room.roomType is RoomType.Start or RoomType.Boss)
            {
                DestroyImmediate(this);
                return;
            }

            _enemiesInRoom = _room.GetComponentsInChildren<EnemyController>().ToList();
        }

        private void Update()
        {
            _enemiesInRoom = _enemiesInRoom.Where(enemy => enemy.CurrentHealth > 0f).ToList();
            if (_enemiesInRoom.Count == 0 && !_abilitySpawned)
                SpawnRandomAbility();
        }

        private void SpawnRandomAbility()
        {
            // Get the room size and position
            var roomSize = _room.size;
            var roomPosition = _room.transform.position;
            // Randomly determine the number of abilities to spawn
            var numberOfAbilities = Random.Range(2, 5);

            // Spawn the abilities
            for (var i = 0; i < numberOfAbilities; i++)
            {
                // Randomly spawn an ability in the room
                var abilityType = Random.Range(0, 2) == 0 ? AbilityType.Heal : AbilityType.Shield;
                var roomSpawnPosition = new Vector3(Random.Range(-roomSize.x / 2, roomSize.x / 2), 1f, Random.Range(-roomSize.z / 2, roomSize.z / 2));
                var spawnPosition = roomPosition + roomSpawnPosition;
                SpawnAbility(abilityType, spawnPosition);
            }

            // Set the ability spawned flag
            _abilitySpawned = true;
        }

        private void SpawnAbility(AbilityType type, Vector3 position)
        {
            // Get the ability pickup prefab
            var abilityPrefab = AbilityDataSystem.Instance.GetAbilityPickupPrefab(type);
            // If the prefab is null, return
            if (!abilityPrefab)
                return;

            // Instantiate the ability pickup prefab
            Instantiate(abilityPrefab, position, Quaternion.identity, _room.transform);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (!_room)
                _room = GetComponent<Room>();

            if (_room.roomType is not (RoomType.Start or RoomType.Boss))
                return;

            Debug.LogError("Ability Spawner cannot be attached to a Start or Boss room. Removing component.");
            Invoke(nameof(DestroyComponent), 0.1f);
        }

        private void DestroyComponent()
        {
            DestroyImmediate(this, true);
        }
#endif
    }
}