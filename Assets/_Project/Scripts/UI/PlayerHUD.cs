using _Project.Scripts.Core.Backend.Scene_Control;
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
        [SerializeField] public LocalPlayerController player;
        [SerializeField] public GameObject reloadingText;
        //[SerializeField] private TMP_Text objectiveText;

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
            UpdateReloadingText();
            //UpdateObjectiveText();
        }

        // Updates the health bar based on the player's current and max health
        private void UpdateHealthBar()
        {
            healthBar.value = player.CurrentHealth;
            healthBar.maxValue = player.CharacterStats.maxHealth;
        }

        // Updates the ammo display based on the player's current and total ammo
        private void UpdateAmmoDisplay()
        {
            var currentRangedWeapon = player.WeaponController.CurrentWeapon as RangedWeapon;
            if (!currentRangedWeapon) return;
            currAmmo.text = currentRangedWeapon.CurrentAmmo.ToString();
            maxAmmo.text = currentRangedWeapon.MaxAmmo.ToString();
        }
        // Updates the reloading text based on the player's current weapon state
        private void UpdateReloadingText()
        {
            var currentRangedWeapon = player.WeaponController.CurrentWeapon as RangedWeapon;
            if (!currentRangedWeapon) return;
            if (currentRangedWeapon.IsReloading)
            {
                reloadingText.SetActive(true);
            }

            if (!currentRangedWeapon.IsReloading)
            {
                reloadingText.SetActive(false);
            }
        }

        /*private void UpdateObjectiveText()
        {
            objectiveText.text = LevelSceneController.Instance.NumberOfEnemies.ToString();
        }*/
    }
}