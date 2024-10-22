using _Project.Scripts.Core.Character;
using _Project.Scripts.Core.Character.Weapon_Controller;
using UnityEngine;

namespace _Project.Scripts.Core.Player_Controllers
{
    /// <summary>
    /// Base class for player controllers.
    /// </summary>
    [RequireComponent(typeof(MovementController), typeof(WeaponController), typeof(CharacterStats))]
    public abstract class PlayerController : MonoBehaviour
    {
        /// <summary>
        /// Component that handles movement.
        /// </summary>
        protected MovementController MovementController { get; private set; }

        /// <summary>
        /// Property to access the weapon controller.
        /// </summary>
        public WeaponController WeaponController { get; private set; }

        /// <summary>
        /// Property to access the char stats.
        /// </summary>
        public CharacterStats CharacterStats => characterStats;

        /// <summary>
        /// Property to access the player's current health.
        /// </summary>
        public float CurrentHealth => currentHealth;

        /// <summary>
        /// Component that handles Character Stats.
        /// </summary>
        [SerializeField] private CharacterStats characterStats;

        /// <summary>
        /// Player's current health.
        /// </summary>
        [SerializeField] private float currentHealth;


        protected virtual void Awake()
        {
            // Get the CharacterMovement and CharacterWeaponController component attached to the player
            MovementController = GetComponent<MovementController>();
            WeaponController = GetComponent<WeaponController>();

            MovementController.Initialize(this);
            WeaponController.Initialize(this);

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
        /// <param name="damageAmount"></param>
        public void TakeDamage(float damageAmount)
        {
            currentHealth -= damageAmount;
            currentHealth = Mathf.Clamp(currentHealth, 0, CharacterStats.maxHealth);
            if (currentHealth <= 0)
            {
                Die();
            }
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
        protected virtual void Die()
        {
            // Handle the character's death
            Debug.Log("Character has died");
        }

        /// <summary>
        /// Update the player's look direction.
        /// </summary>
        /// <param name="lookInput">Look input</param>
        protected abstract void SetLookInput(Vector2 lookInput);
    }
}