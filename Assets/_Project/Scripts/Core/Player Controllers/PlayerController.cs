﻿using System;
using _Project.Scripts.Core.Character;
using _Project.Scripts.Core.Character.Weapon_Controller;
using _Project.Scripts.Core.Weapons;
using UnityEngine;

namespace _Project.Scripts.Core.Player_Controllers
{
    /// <summary>
    /// Base class for player controllers.
    /// </summary>
    [RequireComponent(typeof(MovementController), typeof(WeaponController), typeof(AnimationController) )]
    public abstract class PlayerController : MonoBehaviour
    {
        /// <summary>
        /// Component that handles movement.
        /// </summary>
        public MovementController MovementController { get; private set; }

        /// <summary>
        /// Property to access the weapon controller.
        /// </summary>
        public WeaponController WeaponController { get; private set; }
        
        public AnimationController AnimationController { get; private set; }

        /// <summary>
        /// Property to access the char stats.
        /// </summary>
        public CharacterStats CharacterStats => characterStats;

        /// <summary>
        /// Property to access the player's current health.
        /// </summary>
        public float CurrentHealth => currentHealth;

        /// <summary>
        /// Property to access the friendly layer.
        /// </summary>
        public LayerMask FriendlyLayer => friendlyLayer;

        /// <summary>
        /// Property to access the enemy layer.
        /// </summary>
        public LayerMask OpponentLayer => opponentLayer;

        /// <summary>
        /// Component that handles Character Stats.
        /// </summary>
        [SerializeField] private CharacterStats characterStats;

        /// <summary>
        /// Layer mask for the friendly.
        /// </summary>
        [SerializeField] private LayerMask friendlyLayer;

        /// <summary>
        /// Layer mask for the enemy.
        /// </summary>
        [SerializeField] private LayerMask opponentLayer;

        /// <summary>
        /// Player's current health.
        /// </summary>
        [SerializeField] private float currentHealth;

        protected virtual void Awake()
        {
            // Get the CharacterMovement and CharacterWeaponController component attached to the player
            MovementController = GetComponent<MovementController>();
            WeaponController = GetComponent<WeaponController>();
            AnimationController = GetComponent<AnimationController>();
        }

        protected virtual void Start()
        {
            // Initialize the player's movement and weapon controller
            MovementController.Initialize(this);
            WeaponController.Initialize(this);
            AnimationController.Initialize(this);

            // Initialize the player's health
            currentHealth = CharacterStats.maxHealth;
        }

        /// <summary>
        /// Update the player's movement direction.
        /// </summary>
        /// <param name="direction">direction in which player should move</param>
        protected void SetMoveInput(Vector2 direction)
        {
            MovementController.MoveInput = direction;
        }

        /// <summary>
        /// Switch the player's weapon.
        /// </summary>
        /// <param name="direction">the number by which the weapon is supposed to switch</param>
        protected virtual void SwitchWeapon(int direction)
        {
            WeaponController.SwitchWeapon(direction);
        }

        protected virtual void Reload()
        {
            WeaponController.ReloadWeapon();
        }

        /// <summary>
        /// Begin the player's attack.
        /// </summary>
        protected void BeginAttack()
        {
            WeaponController.BeginAttack();
        }

        /// <summary>
        /// End the player's attack.
        /// </summary>
        protected void EndAttack()
        {
            WeaponController.EndAttack();
        }

        /// <summary>
        /// Function to take damage by reducing the stat's value.
        /// </summary>
        /// <param name="weapon">weapon that is dealing the damage.</param>
        /// <param name="damageDealt">damage dealt by the weapon</param>
        public virtual void TakeDamage(Weapon weapon, float damageDealt)
        {
            currentHealth -= damageDealt;
            currentHealth = Mathf.Clamp(currentHealth, 0f, CharacterStats.maxHealth);
            OnHitConfirmed(weapon.CurrentPlayerController);

            if (currentHealth <= 0)
                Die(weapon.CurrentPlayerController);
        }

        /// <summary>
        /// Function to heal character's health by increasing the stat's value.
        /// </summary>
        /// <param name="healAmount"></param>
        protected void Heal(int healAmount)
        {
            currentHealth += healAmount;
            currentHealth = Mathf.Clamp(currentHealth, 0, CharacterStats.maxHealth);
        }

        /// <summary>
        /// Function to handle the character's death.
        /// </summary>
        /// <param name="enemyPlayer"></param>
        protected virtual void Die(PlayerController enemyPlayer)
        {
            // Handle the character's death
            enemyPlayer.OnKillConfirmed(this);
        }

        protected virtual void OnKillConfirmed(PlayerController enemyPlayer)
        {
            // Handle the kill confirmation
        }

        protected virtual void OnHitConfirmed(PlayerController enemyPlayer)
        {
            // Handle the hit confirmation
        }
    }
}