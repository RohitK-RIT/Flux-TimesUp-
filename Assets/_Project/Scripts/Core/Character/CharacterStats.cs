using UnityEngine;

namespace _Project.Scripts.Core.Character
{
    /// <summary>
    /// Class to store character's stats and handle damage, healing, and death.
    /// ScriptableObject is used to create an asset that can be reused across multiple objects.
    /// </summary>
    [CreateAssetMenu(fileName = "NewCharacterStats", menuName = "Stats/CharacterStats")]
    public class CharacterStats : ScriptableObject
    {
        [SerializeField] private int maxHealth = 100;
        [SerializeField] private float currentHealth;
        public float movementSpeed;
        public float reloadSpeed;
        public float aimAccuracy;
        public int cooldown;
        
        private void Awake()
        {
            // Initialize current health to max health when the game starts
            currentHealth = maxHealth;
        }

        /// <summary>
        /// Function to take damage by reducing the stat's value.
        /// </summary>
        /// <param name="damageAmount"></param>
        public void TakeDamage(float damageAmount)
        {
            currentHealth -= damageAmount;
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
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
            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
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
