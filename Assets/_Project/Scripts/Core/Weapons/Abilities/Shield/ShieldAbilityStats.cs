using UnityEngine;

namespace _Project.Scripts.Core.Weapons.Abilities.Shield
{
    [CreateAssetMenu(fileName = "SAS_LevelName", menuName = "Stats/Ability/Shield", order = 0)]
    public class ShieldAbilityStats : AbilityStats
    {
        public float Duration => duration;

        /// <summary>
        /// The duration for which the shield ability remains active.
        /// </summary>
        [SerializeField] private float duration = 7f;
    }
}