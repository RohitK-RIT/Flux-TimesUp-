﻿using System.Collections;
using _Project.Scripts.Core.Player_Controllers;
using UnityEngine;

namespace _Project.Scripts.Core.Weapons.Melee
{
    /// <summary>
    /// Melee weapon class.
    /// </summary>
    public class MeleeWeapon : Weapon
    {
        /// <summary>
        /// Melee weapon stats.
        /// </summary>
        [SerializeField] private MeleeWeaponStats stats;

        /// <summary>
        /// Coroutine for attacking.
        /// </summary>
        protected override IEnumerator OnAttack()
        {
            // Attack until the attack ends
            while (true)
            {
                Slash();

                yield return new WaitForSeconds(1 / stats.AttackSpeed);
            }
        }

        /// <summary>
        /// Do the attack.
        /// </summary>
        private void Slash()
        {
            // Check for enemies in the attack range
            var enemiesColliders = new Collider[20];
            var count = Physics.OverlapSphereNonAlloc(CurrentPlayerController.transform.position, stats.Range, enemiesColliders, CurrentPlayerController.OpponentLayer);

            // Remove the enemies that are out of attack FOV
            for (var i = 0; i < count; i++)
            {
                if (!enemiesColliders[i])
                    continue;

                var direction = enemiesColliders[i].transform.position - CurrentPlayerController.transform.position;
                var angle = Vector3.Angle(transform.forward, direction);

                // Deal damage to the enemies in the attack FOV
                if (angle > stats.AttackFOV)
                    continue;

                var playerController = enemiesColliders[i].gameObject.GetComponent<PlayerController>();
                playerController?.TakeDamage(stats.Damage);
            }
        }
    }
}