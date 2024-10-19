using _Project.Scripts.Core.Character;
using _Project.Scripts.Core.Character.Weapon_Controller;
using UnityEngine;

namespace _Project.Scripts.Core.Player_Controllers
{
    [RequireComponent(typeof(MovementController), typeof(WeaponController), typeof(CharacterStats))]
    public abstract class PlayerController : MonoBehaviour
    {
        /// <summary>
        /// Player's current health.
        /// </summary>
        public float currentHealth;
        /// <summary>
        /// Component that handles movement.
        /// </summary>
        private MovementController _movementController;

        /// <summary>
        /// Property to access the weapon controller.
        /// </summary>
        public WeaponController WeaponController => _weaponController;

        /// <summary>
        /// Component that handles weapons.
        /// </summary>
        private WeaponController _weaponController;

        /// <summary>
        /// Property to access the char stats.
        /// </summary>
        public CharacterStats CharacterStats => characterStats;

        /// <summary>
        /// Component that handles Character Stats.
        /// </summary>
        [SerializeField] private CharacterStats characterStats;


        protected virtual void Awake()
        {
            // Get the CharacterMovement and CharacterWeaponController component attached to the player
            _movementController = GetComponent<MovementController>();
            _weaponController = GetComponent<WeaponController>();
            currentHealth = CharacterStats.maxHealth;
        }

        /// <summary>
        /// Update the player's movement direction.
        /// </summary>
        /// <param name="direction">direction in which player should move</param>
        protected void UpdateMoveDirection(Vector2 direction)
        {
            _movementController.moveDirection = direction;
        }

        /// <summary>
        /// Begin the player's attack.
        /// </summary>
        protected void BeginAttack()
        {
            _weaponController.BeginAttack();
        }

        /// <summary>
        /// End the player's attack.
        /// </summary>
        protected void EndAttack()
        {
            _weaponController.EndAttack();
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
        public void Heal(int healAmount)
        {
            currentHealth += healAmount;
            currentHealth = Mathf.Clamp(currentHealth, 0, CharacterStats.maxHealth);
        }
        /// <summary>
        /// Function to handle the character's death.
        /// </summary>
        private void Die()
        {
            // Handle the character's death
            Debug.Log("Character has died");
        }
    }
}