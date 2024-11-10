using System;
using System.Collections;
using _Project.Scripts.Core.Player_Controllers;
using UnityEngine;

namespace _Project.Scripts.Core.Weapons.Abilities
{
    public sealed class PlayerAbility : Weapon
    {
        [SerializeField] private float abilityDuration;
        public LocalPlayerController currentPlayerController;
        [SerializeField] private float attackCooldown = 5f; // Cooldown time between attacks
        private bool _isCooldownActive = false;
        private bool _isAbilityActive = false;

        protected override IEnumerator OnAttack()
        {
            switch (currentPlayerController.CharacterStats.playerAbilityType)
            {
                case PlayerAbilityType.Attack:
                    //UseAttack();
                    break;
                case PlayerAbilityType.Teleport:
                    //UseTeleport();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            Debug.Log("Player is using the ability!!");
            yield return null;
        }
        /// <summary>
        /// Function to use the shield ability.
        /// </summary>
        public void UseShield()
        {
            if (_isAbilityActive || _isCooldownActive)
            {
                Debug.Log("Ability is on cooldown or already active.");
                return;
            }
            _isAbilityActive = true;
            //No damage is taken when shield is active
            currentPlayerController.TakeDamage(0);
            abilityDuration = 7f;
            Debug.Log("Player is using the shield ability!!");
            StartCoroutine(DeactivateAbility(abilityDuration));
            StartCoroutine(StartCooldown());
        }
        /// <summary>
        /// Function to use the heal ability.
        /// </summary>
        public void UseHeal()
        {
            if (_isAbilityActive || _isCooldownActive)
            {
                Debug.Log("Ability is on cooldown or already active.");
                return;
            }
            abilityDuration = 5f;
            Debug.Log("Player is using the heal ability!!");
            StartCoroutine(DeactivateAbility(abilityDuration));
            StartCoroutine(StartCooldown());
        }
        /// <summary>
        /// Coroutine to deactivate the ability after a certain time.
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        private static IEnumerator DeactivateAbility(float time)
        {
            yield return new WaitForSeconds(time);
            Debug.Log("Ability deactivated!!");
        }
        private IEnumerator StartCooldown()
        {
            _isCooldownActive = true;
            yield return new WaitForSeconds(attackCooldown); // Wait for the cooldown period
            _isCooldownActive = false;
            _isAbilityActive = false; // Allow a new attack after cooldown
            Debug.Log("Ability is on cooldown!!");
        }
    }
}