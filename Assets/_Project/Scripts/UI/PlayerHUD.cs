using _Project.Scripts.Core.Character;
using _Project.Scripts.Core.Weapons.Melee;
using _Project.Scripts.Core.Weapons.Ranged;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace _Project.Scripts.UI
{
    /// <summary>
    /// Player HUD class.
    /// </summary>
    public class PlayerHUD : MonoBehaviour
    {
        // References to the UI components
        [SerializeField] public Slider healthBar;
        [SerializeField] public TMP_Text currAmmo;
        [SerializeField] public TMP_Text maxAmmo;
        [SerializeField] public PlayerController player;
        
        private void Start()
        {
            // Initialize the health bar and ammo display with the player's starting values
            UpdateHealthBar();
            UpdateAmmoDisplay();
        }

        private void Update()
        {
            // Update the health bar and ammo display in real-time
            UpdateHealthBar();
            UpdateAmmoDisplay();
        }

        // Updates the health bar based on the player's current and max health
        private void UpdateHealthBar()
        {
            healthBar.value = player.characterStats.currentHealth;
            healthBar.maxValue = player.characterStats.maxHealth;
        }

        // Updates the ammo display based on the player's current and total ammo
        private void UpdateAmmoDisplay()
        {
            if (player._weaponController = typeof(MeleeWeapon))
            {
                currAmmo.gameObject.SetActive(false); 
                maxAmmo.gameObject.SetActive(false);
            }
            if (player._weaponController = typeof(RangedWeapon))
            {
                currAmmo.text = player._weaponController.MagazineCount.ToString();
                maxAmmo.text = player._weaponController.stats.MagazineSize.ToString();
            }
        }
    }
}
