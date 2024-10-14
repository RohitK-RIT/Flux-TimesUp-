using UnityEngine;
using UnityEngine.Serialization;

namespace _Project.Scripts.Core.Character
{
    public sealed class CharacterStats : MonoBehaviour
    {
        [FormerlySerializedAs("_maxHealth")] public int maxHealth = 100;
        private int CurrentHealth { get; set; }
        [FormerlySerializedAs("_movementSpeed")] public int movementSpeed;
        [FormerlySerializedAs("_reloadSpeed")] public int reloadSpeed;
        [FormerlySerializedAs("_aimAccuracy")] public int aimAccuracy;
        [FormerlySerializedAs("_cooldown")] public int cooldown;

        private void Awake()
        {
            // Initialize current health to max health when the game starts
            CurrentHealth = maxHealth;
        }

        public void TakeDamage(int damage)
        {
            //damage -= armor.GetValue();
            damage = Mathf.Clamp(damage, 0, int.MaxValue);

            CurrentHealth -= damage;
            CurrentHealth = Mathf.Clamp(CurrentHealth, 0, maxHealth);
            Debug.Log(transform.name + " takes " + damage + " damage.");

            if (CurrentHealth <= 0)
            {
                Die();
            }
        }
        public void Heal(int amount)
        {
            CurrentHealth += amount;
            CurrentHealth = Mathf.Clamp(CurrentHealth, 0, maxHealth);
            Debug.Log(transform.name + " heals for " + amount + " health.");
        }

        private void Die()
        {
            // Die in some way
            // This method is meant to be overwritten
            Debug.Log(transform.name + " died.");
        }
    }
}
