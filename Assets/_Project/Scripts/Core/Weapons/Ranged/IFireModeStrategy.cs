using System;
using System.Collections;
using UnityEngine;

namespace _Project.Scripts.Core.Weapons.Ranged
{
    /// <summary>
    /// Interface for fire mode strategies.
    /// </summary>
    public interface IFireModeStrategy
    {
        /// <summary>
        /// Fire the weapon.
        /// </summary>
        /// <param name="stats">stats of the weapon</param>
        /// <param name="fireAction">the function that fires the bullet</param>
        public IEnumerator Fire(RangedWeaponStats stats, Action fireAction);

        /// <summary>
        /// Get the fire mode strategy based on the fire mode.
        /// </summary>
        /// <param name="mode">fire mode</param>
        /// <returns>strategy based on the fire mode</returns>
        public static IFireModeStrategy GetFireModeStrategy(FireModes mode)
        {
            return mode switch
            {
                FireModes.Single => new SingleFireMode(),
                FireModes.Burst => new BurstFireMode(),
                FireModes.Auto => new AutoFireMode(),
                _ => new SingleFireMode()
            };
        }
    }

    /// <summary>
    /// Single fire mode strategy.
    /// </summary>
    public class SingleFireMode : IFireModeStrategy
    {
        /// <summary>
        /// Fire the weapon.
        /// </summary>
        /// <param name="stats">stats of the weapon</param>
        /// <param name="fireAction">the function that fires the bullet</param>
        public IEnumerator Fire(RangedWeaponStats stats, Action fireAction)
        {
            fireAction?.Invoke();
            yield break;
        }
    }

    /// <summary>
    /// Auto fire mode strategy.
    /// </summary>
    public class AutoFireMode : IFireModeStrategy
    {
        /// <summary>
        /// Fire the weapon.
        /// </summary>
        /// <param name="stats">stats of the weapon</param>
        /// <param name="fireAction">the function that fires the bullet</param>
        public IEnumerator Fire(RangedWeaponStats stats, Action fireAction)
        {
            // Keep on firing until the coroutine is stopped.
            while (true)
            {
                yield return FireCoroutine(stats, fireAction);
                yield return new WaitForSeconds(1 / stats.AttackSpeed);
            }
        }

        /// <summary>
        /// What happens when the weapon is fired.
        /// </summary>
        /// <param name="stats">stats of the weapon</param>
        /// <param name="fireAction">the function that fires the bullet</param>
        protected virtual IEnumerator FireCoroutine(RangedWeaponStats stats, Action fireAction)
        {
            fireAction?.Invoke();
            yield break;
        }
    }
    
    /// <summary>
    /// Burst fire mode strategy.
    /// </summary>
    public class BurstFireMode : AutoFireMode
    {
        /// <summary>
        /// What happens when the weapon is fired.
        /// </summary>
        /// <param name="stats">stats of the weapon</param>
        /// <param name="fireAction">the function that fires the bullet</param>
        protected override IEnumerator FireCoroutine(RangedWeaponStats stats, Action fireAction)
        {
            // Fire the weapon for the amount of bursts.
            for (var burstNumber = 0; burstNumber < stats.BurstAmount; burstNumber++)
            {
                fireAction?.Invoke();
                yield return new WaitForSeconds(stats.BurstDuration / stats.BurstAmount);
            }
        }
    }
}