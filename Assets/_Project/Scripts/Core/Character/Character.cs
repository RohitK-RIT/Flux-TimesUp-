using UnityEngine;

namespace _Project.Scripts.Core.Character
{
    public class Character : MonoBehaviour
    {
        private CharacterStatsObjects stats;

        private int currentHealth;

        private void Awake()
        {
            currentHealth = stats.maxHealth;
        }

        // Same damage, heal, and die methods here
        private void TakeDamage(int damage)
        {
            damage = Mathf.Clamp(damage, 0, int.MaxValue);

            currentHealth -= damage;
            currentHealth = Mathf.Clamp(currentHealth, 0, stats.maxHealth);
            Debug.Log(transform.name + " takes " + damage + " damage.");

            if (currentHealth <= 0)
            {
                Die();
            }
        }

        private void Heal(int amount)
        {
            currentHealth += amount;
            currentHealth = Mathf.Clamp(currentHealth, 0, stats.maxHealth);
            Debug.Log(transform.name + " heals for " + amount + " health.");
        }

        private void Die()
        {
            Debug.Log(transform.name + " died.");
        }
    }
}
