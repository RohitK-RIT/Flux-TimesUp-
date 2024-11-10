using System;
using System.Collections;
using _Project.Scripts.Core.Player_Controllers;
using UnityEngine;

namespace _Project.Scripts.Core.Weapons.Abilities
{
    public abstract class PlayerAbility : Weapon
    {
        public LocalPlayerController currentPlayerController;
        [SerializeField] private float attackCooldown = 5f; // Cooldown time between attacks
        private bool _isCooldownActive = false;
        private bool _isAbilityActive = false;
        public bool Used { get; protected set; }

        public virtual void OnEquip()
        {
            Used = false;
        }
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
        /// Coroutine to deactivate the ability after a certain time.
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public IEnumerator DeactivateAbility(float time)
        {
            yield return new WaitForSeconds(time);
            Debug.Log("Ability deactivated!!");
            StartCoroutine(StartCooldown());
        }

        protected IEnumerator StartCooldown()
        {
            _isCooldownActive = true;
            yield return new WaitForSeconds(attackCooldown); // Wait for the cooldown period
            _isCooldownActive = false;
            _isAbilityActive = false; // Allow a new attack after cooldown
            Debug.Log("Ability is on cooldown!!");
        }
    }
}