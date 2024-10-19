using _Project.Scripts.Core.Player_Controllers;
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
            healthBar.value = player.CharacterStats.currentHealth;
            healthBar.maxValue = player.CharacterStats.maxHealth;
        }

        // Updates the ammo display based on the player's current and total ammo
        private void UpdateAmmoDisplay()
        {
            switch (player.WeaponController.CurrentWeapon)
            {
                case RangedWeapon currentRangedWeapon:
                    currAmmo.text = currentRangedWeapon.MagazineCount.ToString();
                    maxAmmo.text = currentRangedWeapon.Stats.MagazineSize.ToString();
                    break;
                default:
                    currAmmo.gameObject.SetActive(false);
                    maxAmmo.gameObject.SetActive(false);
                    break;
            }
        }
    }
}