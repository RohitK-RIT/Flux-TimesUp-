using System.Linq;
using UnityEngine;
using _Project.Scripts.Core.Weapons.Abilities;

namespace _Project.Scripts.Core.Backend.Ability
{
    /// <summary>
    /// System for managing ability data.
    /// </summary>
    public class AbilityDataSystem : BaseSystem<AbilityDataSystem>
    {
        protected override bool IsPersistent => true;

        [SerializeField] private AbilityData[] abilityData;

        public PlayerAbility GetAbilityPrefab(PlayerAbilityType type)
        {
            var data = GetAbilityData(type);
            if (data.Equals(null))
                return null;

            var prefab = data.GetAbilityPrefab();
            if (prefab)
                return prefab;

            Debug.LogError($"Ability prefab for {type} not found.");
            return null;
        }
        
        public AbilityStats GetAbilityStats(PlayerAbilityType type, int level)
        {
            var data = GetAbilityData(type);
            if (data.Equals(null))
                return null;

            var stats = data.GetAbilityStats(level);
            if (stats)
                return stats;

            Debug.LogError($"Ability stats for {type} at level {level} not found.");
            return null;
        }

        private AbilityData GetAbilityData(PlayerAbilityType type)
        {
            var data = abilityData.FirstOrDefault(data => data.Type == type);
            if (data != null)
                return data;

            Debug.LogError($"AbilityData object for {type} not found.");
            return null;
        }
    }
}