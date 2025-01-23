using _Project.Scripts.Core.Loadout;
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
        
        [SerializeField] public Image primaryWeaponSlotHolder;
        [SerializeField] public Image secondaryWeaponSlotHolder;
        [SerializeField] public Image meleeWeaponSlotHolder;
        //[SerializeField] public Image abilitySlotHolder;

        [SerializeField] public GameObject reloadingText;

        //[SerializeField] private TMP_Text objectiveText;
        [SerializeField] private TMP_Text coinsText;

        //private WeaponDataSystem weaponDataSystem;
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
            UpdateCoinsText();
            UpdateLoadoutInfo();
        }
        
        //Updates the current loadout of the player in real-time.
        // ReSharper disable Unity.PerformanceAnalysis
        private void UpdateLoadoutInfo()
        {
            if (player == null) return;

            primaryWeaponSlotHolder.sprite = WeaponDataSystem.Instance.GetWeaponIcon(player.WeaponController.Weapons[0].WeaponID);
            
            secondaryWeaponSlotHolder.sprite = WeaponDataSystem.Instance.GetWeaponIcon(player.WeaponController.Weapons[1].WeaponID);
            meleeWeaponSlotHolder.sprite = WeaponDataSystem.Instance.GetWeaponIcon(player.WeaponController.Weapons[2].WeaponID);
            primaryWeaponSlotHolder.enabled = true;
            secondaryWeaponSlotHolder.enabled = true;
            meleeWeaponSlotHolder.enabled = true;
        }

        // Updates the health bar based on the player's current and max health
        private void UpdateHealthBar()
        {
            healthBar.value = player.CurrentHealth;
            healthBar.maxValue = player.Stats.maxHealth;
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
            if (!currentRangedWeapon)
                return;

            reloadingText.SetActive(currentRangedWeapon.IsReloading);
        }

        /*private void UpdateObjectiveText()
        {
            objectiveText.text = LevelSceneController.Instance.NumberOfEnemies.ToString();
        }*/

        private void UpdateCoinsText()
        {
            coinsText.text = player?.GetCoins().ToString();
        }
    }
}