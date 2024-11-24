using System.Collections.Generic;
using _Project.Scripts.Core.Weapons;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _Project.Scripts.Core.Loadout
{
    public class LoadOutManager : MonoBehaviour
    {
        // Lists to store available weapon options for each category
        private readonly List<WeaponData> _primaryRangedWeapons = new List<WeaponData>();
        private readonly List<WeaponData> _secondaryRangedWeapons = new List<WeaponData>();
        private readonly List<WeaponData> _meleeWeapons = new List<WeaponData>();

        // UI elements for the weapon slot holders
        [SerializeField] private GameObject primaryWeaponSlotHolder;
        [SerializeField] private GameObject secondaryWeaponSlotHolder;
        [SerializeField] private GameObject meleeWeaponSlotHolder;

        // Prefab for weapon slots
        [SerializeField] private GameObject weaponSlotPrefab; 
        
        // List to store the selected weapon IDs for the loadout
        private readonly string[] _loadout = new string[3];
    
        // The name of the next scene to load after selection
        private readonly string _nextSceneName = "Greybox";
        
        private void Start()
        {
            CreateWeaponList();
            PopulateWeaponSlots(primaryWeaponSlotHolder, _primaryRangedWeapons);
            PopulateWeaponSlots(secondaryWeaponSlotHolder, _secondaryRangedWeapons);
            PopulateWeaponSlots(meleeWeaponSlotHolder, _meleeWeapons);
            
        }
        
        
        // Populates weapon slots dynamically for a given weapon category.
        private void PopulateWeaponSlots<T>(GameObject slotHolder, List<T> weapons) where T : WeaponData
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
                    // Capture the current weapon ID in a local variable
                    string weaponID = weapon.weaponStats[0].WeaponID;
                    
                    // Capture the current weapon type in a local variable
                    WeaponType weaponType = weapon.weaponStats[0].WeaponType; 
                    
                    // Add the onClick listener
                    button.onClick.AddListener(() =>
                    {
                        OnWeaponSlotClicked(weaponID, weaponType);
                    });
                }
            }
        }

        // Method to assign weapon Ids to the loadout list on clicking the weapon on UI
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
        
        // Method to create a weapon list based on its weapon type
        private void CreateWeaponList()
        {
            // Iterate through the weapon database
            foreach (var weaponData in WeaponDataSystem.Instance.weaponDatabase)
            {
                // Iterate through stats within each weapon
                foreach (var weaponStat in weaponData.weaponStats) 
                {
                    // Check the WeaponType property
                    if (weaponStat.WeaponType == WeaponType.Primary)
                    {
                        _primaryRangedWeapons.Add(weaponData);
                    }
                    else if (weaponStat.WeaponType == WeaponType.Secondary)
                    {
                        _secondaryRangedWeapons.Add(weaponData);
                    }
                    else if (weaponStat.WeaponType == WeaponType.Melee)
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
                // Save data to WeaponDataSystem
                TransferLoadoutToWeaponDataSystem();
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
        private void TransferLoadoutToWeaponDataSystem()
        {
            WeaponDataSystem.Instance.SetSelectedWeapons(_loadout);
        }

    }
}
