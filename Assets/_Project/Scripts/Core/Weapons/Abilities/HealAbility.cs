﻿using System.Collections;
using UnityEngine;

namespace _Project.Scripts.Core.Weapons.Abilities
{
    public class HealAbility : PlayerAbility
    {
        [SerializeField] private float abilityDuration;

        public override void OnEquip()
        {
            base.OnEquip();
            UseHeal();
            Used = true;
        }

        /// <summary>
        /// Function to use the heal ability.
        /// </summary>
        private void UseHeal()
        {
            if (_isAbilityActive || _isCooldownActive)
            {
                Debug.Log("Ability is on cooldown or already active.");
                return;
            }

            _isAbilityActive = true;
            //Heal the player
            abilityDuration = 5f;
            Debug.Log("Player is using the heal ability!!");
            StartCoroutine(DeactivateAbility(abilityDuration));
        }

        /// <summary>
        /// Coroutine to deactivate the ability after a certain time.
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        private IEnumerator DeactivateAbility(float time)
        {
            yield return new WaitForSeconds(time);
            Debug.Log("Ability deactivated!!");
            StartCoroutine(StartCooldown());
        }

        protected override IEnumerator OnAttack()
        {
            yield break;
        }
    }
}