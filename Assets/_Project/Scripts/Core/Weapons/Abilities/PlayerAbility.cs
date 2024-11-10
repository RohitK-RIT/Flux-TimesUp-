using System.Collections;
using _Project.Scripts.Core.Player_Controllers;
using UnityEngine;

namespace _Project.Scripts.Core.Weapons.Abilities
{
    public abstract class PlayerAbility : Weapon
    {
        public LocalPlayerController currentPlayerController;
        [SerializeField] private float attackCooldown = 5f; // Cooldown time between attacks
        protected bool _isCooldownActive = false;
        protected bool _isAbilityActive = false;
        public bool Used { get; protected set; }

        public virtual void OnEquip()
        {
            Used = false;
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