using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using _Project.Scripts.Core.Backend.Interfaces;
using _Project.Scripts.Core.Player_Controllers;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Pool;
using Random = UnityEngine.Random;

namespace _Project.Scripts.Core.Weapons.Ranged
{
    /// <summary>
    /// Ranged weapon class.
    /// </summary>
    public sealed class RangedWeapon : Weapon
    {
        /// <summary>
        /// Weapon stats.
        /// </summary>
        [SerializeField] private RangedWeaponStats stats;

        /// <summary>
        /// Muzzle of the weapon.
        /// </summary>
        [SerializeField] private Transform muzzle;

        /// <summary>
        /// Trail renderer prefab.
        /// </summary>
        [SerializeField] private TrailRenderer trailRendererPrefab;

        /// <summary>
        /// Property to access current number of bullets in the magazine.
        /// </summary>
        public int CurrentAmmo { get; private set; }

        /// <summary>
        /// Property to access the maximum number of bullets for the Gun.
        /// </summary>
        public int MaxAmmo { get; private set; }

        ///<summary>
        /// Property to check if the weapon is currently reloading.
        /// </summary>
        public bool IsReloading => _reloading;

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

        /// <summary>
        /// Object pool for trail renderers.
        /// </summary>
        private ObjectPool<TrailRenderer> _trailRendererPool;

        /// <summary>
        /// Last time the weapon was shot.
        /// </summary>
        private DateTime _lastShootTime = DateTime.MinValue;

        private void Start()
        {
            // Initialize the dictionary of fire mode strategies
            _fireModeStrategies = new Dictionary<FireModes, IFireModeStrategy>();
            foreach (var mode in stats.FireModes)
                _fireModeStrategies.TryAdd(mode, IFireModeStrategy.GetFireModeStrategy(mode));

            // Set the default fire mode and magazine count.
            _currentFireMode = _fireModeStrategies.First().Key;
            CurrentAmmo = stats.MagazineSize;
            MaxAmmo = stats.MaxBulletCount;

            // Initialize the trail renderer pool.
            _trailRendererPool = new ObjectPool<TrailRenderer>(CreateTrail);
        }

        /// <summary>
        /// Create a bullet trail renderer.
        /// </summary>
        /// <returns></returns>
        private TrailRenderer CreateTrail()
        {
            // Instantiate a trail renderer and set its properties.
            var trail = Instantiate(trailRendererPrefab);
            trail.emitting = false;
            trail.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

            return trail;
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
            if (_reloading) return;

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
            // Wait for the attack speed and then fire the bullet.
            yield return new WaitWhile(() => (DateTime.Now - _lastShootTime).Seconds < 1 / stats.AttackSpeed);

            // yield return new WaitUntil(() => (DateTime.Now - _lastShootTime).Seconds >= 1 / stats.AttackSpeed);

            // Find a fire mode strategy and wait for it to finish, else show an error.
            if (_fireModeStrategies.TryGetValue(_currentFireMode, out var strategy))
                yield return strategy.Fire(stats, FireBullet);
            else
                Debug.LogError($"No fire mode set for {stats.WeaponName}", stats);
        }

        protected override float GetDamage()
        {
            // TODO: Implement era specific damage calculation
            return stats.Damage;
        }

        /// <summary>
        /// Fire the bullet.
        /// </summary>
        private void FireBullet()
        {
            if (CurrentAmmo == 0)
                return;

            var spreadOffset = new Vector3(Random.Range(-stats.Spread, stats.Spread),
                Random.Range(-stats.Spread, stats.Spread), 0); // Add some spread to the bullet.
            var recoilOffset = new Vector3(0, Random.Range(0, stats.Recoil), 0); // Add some recoil to the bullet.
            _recoilFactor = Mathf.Clamp01(_recoilFactor + 0.1f); // Increase the recoil factor.

            // Get the direction of the bullet.
            var fireDirection = muzzle.forward;
            fireDirection = (fireDirection + spreadOffset).normalized;
            fireDirection = (fireDirection + recoilOffset * _recoilFactor).normalized;

            // Raycast to check if the bullet hits something. If it does, play the trail to that point, else play the trail to the miss distance.
            if (Physics.Raycast(muzzle.position, fireDirection, out var hit, stats.MissDistance, CurrentPlayerController.OpponentLayer))
            {
                StartCoroutine(PlayTrail(muzzle.position, hit.point));
                if(hit.transform.TryGetComponent<IDamageable>(out var damageable))
                    damageable.TakeDamage(this, GetDamage());
            }
            else
            {
                StartCoroutine(PlayTrail(muzzle.position, muzzle.position + fireDirection * stats.MissDistance));
            }

            _lastShootTime = DateTime.Now;

            // Decrease the magazine count and reload if it's empty.
            if (--CurrentAmmo != 0)
                return;

            StartCoroutine(ReloadCoroutine());
        }

        /// <summary>
        /// Reload the weapon.
        /// </summary>
        public void OnReload()
        {
            if (CurrentAmmo == stats.MagazineSize || _reloading || MaxAmmo == 0)
                return;
            StartCoroutine(ReloadCoroutine());
        }
        
        

        /// <summary>
        /// Coroutine for playing the trail.
        /// </summary>
        /// <param name="startPos">Start position of the trail</param>
        /// <param name="endPos">End position of the trail</param>
        private IEnumerator PlayTrail(Vector3 startPos, Vector3 endPos)
        {
            // Get a trail renderer from the pool and set its position.
            var trail = _trailRendererPool.Get();
            trail.transform.position = startPos;
            trail.emitting = true;
            trail.gameObject.SetActive(true);

            // Play the trail from the start position to the end position.
            var distance = Vector3.Distance(startPos, endPos);
            var remainingDistance = distance;
            while (remainingDistance > 0)
            {
                trail.transform.position = Vector3.Lerp(startPos, endPos,
                    Mathf.Clamp01((distance - remainingDistance) / distance));
                remainingDistance -= stats.TrailSpeed * Time.deltaTime;

                yield return null;
            }

            trail.transform.position = endPos;

            // Wait for the trail to finish emitting and then release it back to the pool.
            yield return new WaitForSeconds(trail.time);

            trail.emitting = false;
            trail.gameObject.SetActive(false);
            _trailRendererPool.Release(trail);
        }

        /// <summary>
        /// Coroutine for reloading.
        /// </summary>
        private IEnumerator ReloadCoroutine()
        {
            if (MaxAmmo == 0)
                yield break;

            if (AttackCoroutine != null)
                StopCoroutine(AttackCoroutine);

            // Wait for the reload time and then refill the magazine.
            _reloading = true;
            yield return new WaitForSeconds(stats.ReloadTime);

            CurrentAmmo = math.min(stats.MagazineSize, MaxAmmo);
            MaxAmmo -= CurrentAmmo;

            _reloading = false;

            if (Attacking)
                BeginAttack();
        }
    }
}