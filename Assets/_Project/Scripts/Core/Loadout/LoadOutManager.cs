using System;
using System.Collections.Generic;
using _Project.Scripts.Core.Backend;
using _Project.Scripts.Core.Weapons;
using _Project.Scripts.Core.Weapons.Melee;
using _Project.Scripts.Core.Weapons.Ranged;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _Project.Scripts.Core.Loadout
{
    public class LoadOutManager : MonoBehaviour
    {
        // Lists to store available weapon options for each category
        private List<WeaponData> _primaryRangedWeapons = new List<WeaponData>();
        private List<WeaponData> _secondaryRangedWeapons = new List<WeaponData>();
        private List<WeaponData> _meleeWeapons = new List<WeaponData>();

        // UI elements for the weapon slot holders
        [SerializeField] private GameObject primaryWeaponSlotHolder;
        [SerializeField] private GameObject secondaryWeaponSlotHolder;
        [SerializeField] private GameObject meleeWeaponSlotHolder;

        [SerializeField] private GameObject weaponSlotPrefab; // Prefab for weapon slots

        
        // List to store the selected weapon IDs for the loadout
        private readonly string[] _loadout = new string[3];
    
        // The name of the next scene to load after selection
        private string _nextSceneName = "Greybox";
        
        private void Start()
        {
            CreatePrimaryWeaponList();
            PopulateWeaponSlots(primaryWeaponSlotHolder, _primaryRangedWeapons);
            PopulateWeaponSlots(secondaryWeaponSlotHolder, _secondaryRangedWeapons);
            PopulateWeaponSlots(meleeWeaponSlotHolder, _meleeWeapons);
            
        }
        
        
        // Populates weapon slots dynamically for a given weapon category.
        public void PopulateWeaponSlots<T>(GameObject slotHolder, List<T> weapons) where T : WeaponData
        {
            foreach (var weapon in weapons)
            {
                // Instantiate the slot prefab
                GameObject slot = Instantiate(weaponSlotPrefab, slotHolder.transform);

                // Assign the weapon's icon to the slot
                var slotImage = slot.transform.GetChild(0).GetComponent<Image>();
                if (slotImage != null)
                {
                    slotImage.sprite = weapon.icon;
                    slotImage.enabled = true;
                }

                // Add weapon data to the slot
                var weaponSlot = slot.GetComponent<WeaponSlots>();
                if (weaponSlot != null)
                {
                    weaponSlot.weaponId = weapon.weaponStats[0].WeaponID; // Assign the weapon's ID
                    weaponSlot.weaponType = weapon.weaponStats[0].WeaponType; // Assign the weapon's type
                }

                // Assign the OnClick listener to the button
                var button = slot.transform.GetChild(0).GetComponent<Button>();
                if (button != null)
                {
                    string weaponID = weapon.weaponStats[0].WeaponID; // Capture the current weapon ID in a local variable
                    WeaponType weaponType = weapon.weaponStats[0].WeaponType; // Capture the current weapon type in a local variable
                    // Add the onClick listener
                    button.onClick.AddListener(() =>
                    {
                        OnWeaponSlotClicked(weaponID, weaponType);
                    });
                }
            }
        }

        private void OnWeaponSlotClicked(string weaponID, WeaponType weaponType)
        {
            if (weaponType == WeaponType.Primary)
            {
                _loadout[0] = weaponID;
            }
            else if (weaponType == WeaponType.Secondary)
            {
                _loadout[1] = weaponID;
            }
            else if (weaponType == WeaponType.Melee)
            {
                _loadout[2] = weaponID;
            }
        }
        
        private void CreatePrimaryWeaponList()
        {
            foreach (var weaponData in WeaponDataSystem.Instance.weaponDatabase) // Iterate through weapon database
            {
                foreach (var weaponStat in weaponData.weaponStats) // Iterate through stats within each weapon
                {
                    if (weaponStat.WeaponType == WeaponType.Primary) // Check the WeaponID property
                    {
                        _primaryRangedWeapons.Add(weaponData);
                    }
                    else if (weaponStat.WeaponType == WeaponType.Secondary) // Check the WeaponID property
                    {
                        _secondaryRangedWeapons.Add(weaponData);
                    }
                    else if (weaponStat.WeaponType == WeaponType.Melee) // Check the WeaponID property
                    {
                        _meleeWeapons.Add(weaponData);
                    }
                }
            }
        }
        
        // Logs the current selected loadout to the console for testing
        private void PrintLoadout()
        {
            Debug.Log("Current Loadout:");
            Debug.Log($"Primary: {_loadout[0]}");
            Debug.Log($"Secondary: {_loadout[1]}");
            Debug.Log($"Melee: {_loadout[2]}");
        }
    
        // Handles the "Continue" button click
        public void OnContinueButtonClicked()
        {
            PrintLoadout();
            if (_isLoadOutComplete())
            {
                TransferLoadoutToWeaponDataSystem(); // Save data to WeaponDataSystem
                SceneManager.LoadScene(_nextSceneName);
            }
            else
            {
                Debug.Log("please select a loadout");
            }
        }

        // Checks if the loadout is fully selected
        private bool _isLoadOutComplete()
        {
            return _loadout[0] != null && _loadout[1] != null && _loadout[2] != null;
        }
        
        // Transfers the selected loadout to the WeaponDataSystem
        public void TransferLoadoutToWeaponDataSystem()
        {
            WeaponDataSystem.Instance.SetSelectedWeapons(_loadout); // Assuming `_loadout` contains weapon IDs
        }

    }
}
