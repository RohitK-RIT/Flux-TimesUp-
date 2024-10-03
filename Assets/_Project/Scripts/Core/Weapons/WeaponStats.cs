using System;
using UnityEngine;

namespace _Project.Scripts.Core.Weapons
{
    [Serializable]
    public abstract class WeaponStats : ScriptableObject
    {
        public string WeaponID => weaponID;
        public string WeaponName => weaponName;
        public float Damage => damage;
        public int Range => range;
        public float AttackSpeed => attackSpeed;

        [Header("Weapon Stats")]
        [SerializeField] private string weaponID;
        [SerializeField] private string weaponName;

        [SerializeField] private float damage;
        [SerializeField] private int range;
        [SerializeField] private float attackSpeed;
    }
}