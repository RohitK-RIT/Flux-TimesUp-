using UnityEngine;

namespace _Project.Scripts.Core.Weapons.Abilities.Heal
{
    [CreateAssetMenu(fileName = "HAS_LevelName", menuName = "Stats/Ability/Heal", order = 0)]
    public class HealAbilityStats : AbilityStats
    {
        /// <summary>
        /// The amount of health restored by the ability.
        /// </summary>
        public float AbilityDuration => abilityDuration;

        /// <summary>
        /// The duration for which the shield ability remains active.
        /// </summary>
        [SerializeField] private float abilityDuration;
    }
}