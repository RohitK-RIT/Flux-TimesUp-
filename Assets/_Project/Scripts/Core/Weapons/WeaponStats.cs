using System;
using UnityEngine;

namespace _Project.Scripts.Core.Weapons
{
    /// <summary>
    /// Base scriptable object for weapon stats.
    /// </summary>
    [Serializable]
    public abstract class WeaponStats : ScriptableObject
    {
        /// <summary>
        /// Weapon ID.
        /// </summary>
        public string WeaponID => weaponID;

        /// <summary>
        /// Weapon name.
        /// </summary>
        public string WeaponName => weaponName;

        /// <summary>
        /// Damage of the weapon.
        /// </summary>
        public float Damage => damage;

        /// <summary>
        /// Range of the weapon.
        /// </summary>
        public int Range => range;

        /// <summary>
        /// Attack speed of the weapon.
        /// </summary>
        public float AttackSpeed => attackSpeed;

        [Header("Weapon Stats")] [SerializeField]
        private string weaponID;

        [SerializeField] private string weaponName;
        [SerializeField] private float damage;
        [SerializeField] private int range;
        [SerializeField] private float attackSpeed;
    }
}