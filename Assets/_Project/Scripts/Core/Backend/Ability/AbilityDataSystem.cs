﻿using System.Linq;
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

        /// <summary>
        /// The ability data.
        /// </summary>
        [SerializeField] private AbilityData[] abilityData;

        /// <summary>
        /// Gets the ability prefab.
        /// </summary>
        /// <param name="type">type of ability</param>
        /// <returns>player ability prefab</returns>
        public PlayerAbility GetAbilityPrefab(PlayerAbilityType type)
        {
            // Get the ability data
            var data = GetAbilityData(type);
            // If the data is null, return null
            if (data.Equals(null))
                return null;

            // Get the ability prefab
            var prefab = data.GetAbilityPrefab();
            // If the prefab is not null, return the prefab
            if (prefab)
                return prefab;

            // Log an error if the prefab is not found
            Debug.LogError($"Ability prefab for {type} not found.");
            return null;
        }
        
        /// <summary>
        /// Gets the ability stats.
        /// </summary>
        /// <param name="type">type of the ability</param>
        /// <param name="level">level of the ability stat you require</param>
        /// <returns>ability stats so of the specified type and level</returns>
        public AbilityStats GetAbilityStats(PlayerAbilityType type, int level)
        {
            // Get the ability data
            var data = GetAbilityData(type);
            // If the data is null, return null
            if (data.Equals(null))
                return null;

            // Get the ability stats
            var stats = data.GetAbilityStats(level);
            // If the stats are not null, return the stats
            if (stats)
                return stats;

            // Log an error if the stats are not found
            Debug.LogError($"Ability stats for {type} at level {level} not found.");
            return null;
        }

        /// <summary>
        /// Gets the ability data.
        /// </summary>
        /// <param name="type">type of the ability</param>
        /// <returns>ability data of the specified type</returns>
        private AbilityData GetAbilityData(PlayerAbilityType type)
        {
            // Get the ability data
            var data = abilityData.FirstOrDefault(data => data.Type == type);
            // If the data is not null, return the data
            if (data != null)
                return data;

            // Log an error if the data is not found
            Debug.LogError($"AbilityData object for {type} not found.");
            return null;
        }
    }
}