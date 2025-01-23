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
        [SerializeField] private GameObject overlay;

        [SerializeField] public GameObject reloadingText;

        //[SerializeField] private TMP_Text objectiveText;
        [SerializeField] private TMP_Text coinsText;
        
        private GameObject primaryOverlay;
        private GameObject secondaryOverlay;
        private GameObject meleeOverlay;

        //private WeaponDataSystem weaponDataSystem;
        private void Start()
        {
            // Initialize the health bar and ammo display with the player's starting values
            UpdateHealthBar();
            UpdateAmmoDisplay();
            primaryOverlay = Instantiate(overlay, primaryWeaponSlotHolder.rectTransform);
            secondaryOverlay = Instantiate(overlay, secondaryWeaponSlotHolder.rectTransform);
            meleeOverlay = Instantiate(overlay, meleeWeaponSlotHolder.rectTransform);
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
            ShowActiveWeaponSlot();
        }
        
        //Shows the active weapon slot based on the player's current weapon.
        private void ShowActiveWeaponSlot()
        {
            if (player.WeaponController.CurrentWeapon == player.WeaponController.Weapons[0])
            {
                primaryOverlay.SetActive(false);
                secondaryOverlay.SetActive(true);
                meleeOverlay.SetActive(true);
            }
            else if (player.WeaponController.CurrentWeapon == player.WeaponController.Weapons[1])
            {
                primaryOverlay.SetActive(true);
                secondaryOverlay.SetActive(false);
                meleeOverlay.SetActive(true);
            }
            else if (player.WeaponController.CurrentWeapon == player.WeaponController.Weapons[2])
            {
               primaryOverlay.SetActive(true);
               secondaryOverlay.SetActive(true);
                meleeOverlay.SetActive(false);
            }
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