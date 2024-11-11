using System.Collections;
using _Project.Scripts.Core.Player_Controllers;
using UnityEngine;

namespace _Project.Scripts.Core.Weapons.Abilities
{
    /// <summary>
    /// Abstract base class for player abilities, inheriting from Weapon.
    /// </summary>
    public abstract class PlayerAbility : Weapon
    {
        /// <summary>
        /// The current player controller using this ability.
        /// </summary>
        public LocalPlayerController currentPlayerController;

        /// <summary>
        /// Cooldown time between attacks.
        /// </summary>
        [SerializeField] private float attackCooldown = 5f;

        /// <summary>
        /// Indicates if the cooldown is active.
        /// </summary>
        protected bool _isCooldownActive = false;

        /// <summary>
        /// Indicates if the ability is active.
        /// </summary>
        protected bool _isAbilityActive = false;

        /// <summary>
        /// Indicates if the ability has been used.
        /// </summary>
        public bool Used { get; protected set; }

        /// <summary>
        /// Called when the ability is equipped.
        /// </summary>
        public virtual void OnEquip()
        {
            Used = false;
        }

        /// <summary>
        /// Starts the cooldown period for the ability.
        /// </summary>
        /// <returns>An IEnumerator for the coroutine.</returns>
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