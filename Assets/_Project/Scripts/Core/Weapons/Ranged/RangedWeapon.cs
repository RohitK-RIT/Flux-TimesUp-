using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace _Project.Scripts.Core.Weapons.Ranged
{
    public sealed class RangedWeapon : Weapon<RangedWeaponStats>
    {
        [SerializeField] private Transform muzzle;
        [SerializeField] private Projectile projectilePrefab;

        private int _magazineCount;
        private FireModes _currentFireMode;

        private Dictionary<FireModes, IFireModeStrategy> _fireModeStrategies;

        private void Start()
        {
            _fireModeStrategies = new Dictionary<FireModes, IFireModeStrategy>();
            foreach (var mode in stats.FireModes)
                _fireModeStrategies.TryAdd(mode, IFireModeStrategy.GetFireModeStrategy(mode));

            _currentFireMode = _fireModeStrategies.First().Key;
            _magazineCount = stats.MagazineSize;
        }

        public void SwitchFireMode()
        {
            AttackEnd();

            var indexOf = (Array.IndexOf(stats.FireModes, _currentFireMode) + 1) % stats.FireModes.Length;
            _currentFireMode = stats.FireModes[indexOf];
        }

        protected override IEnumerator AttackCoroutine()
        {
            if (_fireModeStrategies.TryGetValue(_currentFireMode, out var strategy))
                yield return strategy.Fire(stats, FireBullet);
            else
                Debug.LogError($"No fire mode set for {stats.WeaponName}", stats);
        }

        private void FireBullet()
        {
            var projectile = Instantiate(projectilePrefab, muzzle.position, muzzle.rotation);
            projectile.Init(stats.WeaponID);

            if (--_magazineCount != 0) return;

            AttackEnd();

            StartCoroutine(ReloadCoroutine());
        }

        private IEnumerator ReloadCoroutine()
        {
            yield return new WaitForSeconds(stats.ReloadTime);
            _magazineCount = stats.MagazineSize;
        }
    }
}