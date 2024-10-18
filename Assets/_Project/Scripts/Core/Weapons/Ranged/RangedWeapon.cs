using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.Scripts.Core.Weapons.Ranged
{
    /// <summary>
    /// Ranged weapon class.
    /// </summary>
    public sealed class RangedWeapon : Weapon<RangedWeaponStats>
    {
        /// <summary>
        /// Muzzle of the weapon.
        /// </summary>
        [SerializeField] private Transform muzzle;

        /// <summary>
        /// Projectile prefab.
        /// </summary>
        [SerializeField] private Projectile projectilePrefab;
        /// <summary>
        /// Current Magazine size of the weapon.
        /// </summary>
        public int MagazineCount => _magazineCount;
        /// <summary>
        /// Number of bullets in the magazine.
        /// </summary>
        private int _magazineCount;
        
        /// <summary>
        /// Is the weapon currently reloading.
        /// </summary>
        private bool _reloading;

        /// <summary>
        /// Recoil factor.
        /// </summary>
        private float _recoilFactor;

        /// <summary>
        /// Current fire mode.
        /// </summary>
        private FireModes _currentFireMode;

        /// <summary>
        /// Dictionary of fire mode strategies.
        /// </summary>
        private Dictionary<FireModes, IFireModeStrategy> _fireModeStrategies;

        private void Start()
        {
            // Initialize the dictionary of fire mode strategies
            _fireModeStrategies = new Dictionary<FireModes, IFireModeStrategy>();
            foreach (var mode in stats.FireModes)
                _fireModeStrategies.TryAdd(mode, IFireModeStrategy.GetFireModeStrategy(mode));

            // Set the default fire mode and magazine count.
            _currentFireMode = _fireModeStrategies.First().Key;
            _magazineCount = stats.MagazineSize;
        }

        /// <summary>
        /// Cycle through the allowed fire modes.
        /// </summary>
        public void SwitchFireMode()
        {
            // End the previous attack if it's still running
            EndAttack();

            // Set the new fire mode
            var indexOf = (Array.IndexOf(stats.FireModes, _currentFireMode) + 1) % stats.FireModes.Length;
            _currentFireMode = stats.FireModes[indexOf];
        }

        /// <summary>
        /// Start attacking.
        /// </summary>
        public override void BeginAttack()
        {
            // If the weapon is reloading, don't start attacking.
            if(_reloading) return;
            
            base.BeginAttack();

            // Reset the recoil factor.
            _recoilFactor = 0;
        }

        /// <summary>
        /// Coroutine for attacking.
        /// </summary>
        /// <returns></returns>
        protected override IEnumerator OnAttack()
        {
            // Find a fire mode strategy and wait for it to finish, else show an error.
            if (_fireModeStrategies.TryGetValue(_currentFireMode, out var strategy))
                yield return strategy.Fire(stats, FireBullet);
            else
                Debug.LogError($"No fire mode set for {stats.WeaponName}", stats);
        }

        /// <summary>
        /// End attacking.
        /// </summary>
        public override void EndAttack()
        {
            base.EndAttack();

            // Reset the recoil factor.
            _recoilFactor = 0;
        }

        /// <summary>
        /// Fire the bullet.
        /// </summary>
        private void FireBullet()
        {
            // Add some spread to the bullet.
            var spreadOffset = new Vector3(Random.Range(-stats.Spread, stats.Spread), Random.Range(-stats.Spread, stats.Spread), 0);
            // Add some recoil to the bullet.
            var recoilOffset = new Vector3(0, Random.Range(0, stats.Recoil), 0);
            // Increase the recoil factor.
            _recoilFactor = Mathf.Clamp01(_recoilFactor + 0.1f);

            // Instantiate the projectile and set its stats.
            var projectile = Instantiate(projectilePrefab, muzzle.position, muzzle.rotation);
            projectile.transform.Rotate(spreadOffset); // Add spread to the bullet.
            projectile.transform.Rotate(recoilOffset * _recoilFactor); // Add recoil to the bullet.
            projectile.Init(stats.WeaponID);

            // Decrease the magazine count and reload if it's empty.
            if (--_magazineCount != 0) return;

            if (AttackCoroutine != null)
                StopCoroutine(AttackCoroutine);
            StartCoroutine(ReloadCoroutine());
        }

        /// <summary>
        /// Coroutine for reloading.
        /// </summary>
        private IEnumerator ReloadCoroutine()
        {
            _reloading = true;
            yield return new WaitForSeconds(stats.ReloadTime);
            _magazineCount = stats.MagazineSize;
            _reloading = false;

            if (Attacking)
                BeginAttack();
        }
    }
}