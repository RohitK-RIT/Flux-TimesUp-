﻿using System;
using UnityEngine;
using _Project.Scripts.Core.Weapons.Abilities;

namespace _Project.Scripts.Core.Backend.Ability
{
    [Serializable]
    public class AbilityData
    {
        /// <summary>
        /// The name of the ability.
        /// </summary>
        public string Name => name;

        /// <summary>
        /// The type of the ability.
        /// </summary>
        public PlayerAbilityType Type => type;

        /// <summary>
        /// The description of the ability.
        /// </summary>
        public string Description => description;

        /// <summary>
        /// The icon of the ability.
        /// </summary>
        public Sprite Icon => icon;

        [SerializeField] private string name;
        [SerializeField] private PlayerAbilityType type;
        [SerializeField, Multiline] private string description;
        [SerializeField] private Sprite icon;

        [Space] [SerializeField] private PlayerAbility abilityPrefab;

        [Header("Ability Stats (follow the order of levels)")] [SerializeField]
        private AbilityStats[] abilityStats;

        internal AbilityStats GetAbilityStats(int level)
        {
            return abilityStats[level - 1];
        }

        internal PlayerAbility GetAbilityPrefab()
        {
            return abilityPrefab;
        }
    }
}