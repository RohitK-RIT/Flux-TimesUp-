using System;
using System.Collections;
using UnityEngine;

namespace _Project.Scripts.Core.Weapons.Ranged
{
    public interface IFireModeStrategy
    {
        public FireModes FireMode { get; }
        public IEnumerator Fire(RangedWeaponStats stats, Action fireAction);

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

    public class SingleFireMode : IFireModeStrategy
    {
        public FireModes FireMode => FireModes.Single;

        public IEnumerator Fire(RangedWeaponStats stats, Action fireAction)
        {
            fireAction?.Invoke();
            yield break;
        }
    }

    public class AutoFireMode : IFireModeStrategy
    {
        public virtual FireModes FireMode => FireModes.Auto;

        public IEnumerator Fire(RangedWeaponStats stats, Action fireAction)
        {
            while (true)
            {
                yield return FireCoroutine(stats, fireAction);
                yield return new WaitForSeconds(1 / stats.AttackSpeed);
            }
        }

        protected virtual IEnumerator FireCoroutine(RangedWeaponStats stats, Action fireAction)
        {
            fireAction?.Invoke();
            yield return new WaitForSeconds(1 / stats.AttackSpeed);
        }
    }
    
    public class BurstFireMode : AutoFireMode
    {
        public override FireModes FireMode => FireModes.Burst;

        protected override IEnumerator FireCoroutine(RangedWeaponStats stats, Action fireAction)
        {
            for (var burstNumber = 0; burstNumber < stats.BurstAmount; burstNumber++)
            {
                fireAction?.Invoke();
                yield return new WaitForSeconds(stats.BurstDuration / stats.BurstAmount);
            }
        }
    }
}