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

        // // Arrays to hold references to individual weapon slots in the UI
        // private GameObject[] _primaryweaponSlots;
        // private GameObject[] _secondaryweaponSlots;
        // private GameObject[] _meleeweaponSlots;

        [SerializeField] private GameObject weaponSlotPrefab; // Prefab for weapon slots

        
        // List to store the selected weapon IDs for the loadout
        private List<string> _loadout = new List<string> { null, null, null };
    
        // The name of the next scene to load after selection
        private string _nextSceneName = "Greybox";
    
        // private void Start()
        // {
        //     // Initialize the weapon slot arrays based on the number of child objects in the slot holders
        //     _primaryweaponSlots = new GameObject[primaryWeaponSlotHolder.transform.childCount];
        //     _secondaryweaponSlots = new GameObject[secondaryWeaponSlotHolder.transform.childCount];
        //     _meleeweaponSlots = new GameObject[meleeWeaponSlotHolder.transform.childCount];
        //
        //     // Populate the slot arrays with references to the UI elements
        //     for (int i = 0; i < _primaryweaponSlots.Length; i++)
        //     {
        //         _primaryweaponSlots[i] = primaryWeaponSlotHolder.transform.GetChild(i).gameObject;
        //     }
        //
        //     for (int i = 0; i < _secondaryweaponSlots.Length; i++)
        //     {
        //         _secondaryweaponSlots[i] = secondaryWeaponSlotHolder.transform.GetChild(i).gameObject;
        //     }
        //
        //     for (int i = 0; i < _meleeweaponSlots.Length; i++)
        //     {
        //         _meleeweaponSlots[i] = meleeWeaponSlotHolder.transform.GetChild(i).gameObject;
        //     }
        //
        //     // Populate the weapon slots with icons
        //     AddIcon();
        // }
        
        private void Start()
        {
            CreatePrimaryWeaponList();
            CreateSecondaryWeaponList();
            CreateMeeleWeaponList();
            PopulateWeaponSlots(primaryWeaponSlotHolder, _primaryRangedWeapons);
            PopulateWeaponSlots(secondaryWeaponSlotHolder, _secondaryRangedWeapons);
            PopulateWeaponSlots(meleeWeaponSlotHolder, _meleeWeapons);
            
        }
        
        /// <summary>
        /// Populates weapon slots dynamically for a given weapon category.
        /// </summary>
        /// <param name="slotHolder">Parent object to hold the slots.</param>
        /// <param name="weapons">List of weapons to populate slots for.</param>
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
                    Debug.Log("hiii");
                    string weaponID = weapon.weaponStats[0].WeaponID; // Capture the current weapon ID in a local variable
                    WeaponType weaponType = weapon.weaponStats[0].WeaponType; // Capture the current weapon type in a local variable
                    // Add the onClick listener
                    button.onClick.AddListener(() =>
                    {
                        Debug.Log($"Button clicked, Weapon ID: {weaponID}, {weaponType}");
                        OnWeaponSlotClicked(weaponID, weaponType);
                    });
                }
            }
        }

        private void OnWeaponSlotClicked(string weaponID, WeaponType weaponType)
        {
            Debug.Log($"Weapon Slot Clicked! Weapon ID: {weaponID}, {weaponType}");
            if (weaponType == WeaponType.Primary)
            {
                _loadout[0] = weaponID;
            }
            else if (weaponType == WeaponType.Secondary)
            {
                _loadout[1] = weaponID;
            }
            else if(weaponType == WeaponType.Melee)
            {
                _loadout[2] = weaponID;
            }

            PrintLoadout();
            // Perform additional logic, such as adding the weapon to the loadout
            //AddWeaponToLoadout(weaponID);
        }
        private void Update()
        {
            // Detect a left mouse button click
            // if (Input.GetMouseButtonDown(0))
            // {
            //     //getSelectedLoadout(); // Update the selected loadout
            //     PrintLoadout();
            // }
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
                }
            }
        }
        
        private void CreateSecondaryWeaponList()
        {
            foreach (var weaponData in WeaponDataSystem.Instance.weaponDatabase) // Iterate through weapon database
            {
                foreach (var weaponStat in weaponData.weaponStats) // Iterate through stats within each weapon
                {
                    if (weaponStat.WeaponType == WeaponType.Secondary) // Check the WeaponID property
                    {
                        _secondaryRangedWeapons.Add(weaponData);
                    }
                }
            }
        }
        private void CreateMeeleWeaponList()
        {
            foreach (var weaponData in WeaponDataSystem.Instance.weaponDatabase) // Iterate through weapon database
            {
                foreach (var weaponStat in weaponData.weaponStats) // Iterate through stats within each weapon
                {
                    if (weaponStat.WeaponType == WeaponType.Melee) // Check the WeaponID property
                    {
                        _meleeWeapons.Add(weaponData);
                    }
                }
            }
        }


        // // Gets the closest primary weapon slot to the mouse position
        // private RangedWeaponStats getClosestPrimaryWeapons()
        // {
        //     for (int i = 0; i < _primaryweaponSlots.Length; i++)
        //     {
        //         if (Vector2.Distance(_primaryweaponSlots[i].transform.position, Input.mousePosition) <= 68)
        //         {
        //             return primaryRangedWeapons[i];
        //         }
        //     }
        //
        //     return null;
        // }
        //
        // // Gets the closest secondary weapon slot to the mouse position
        // private RangedWeaponStats getClosestSecondaryWeapons()
        // {
        //     for (int i = 0; i < _secondaryweaponSlots.Length; i++)
        //     {
        //         if (Vector2.Distance(_secondaryweaponSlots[i].transform.position, Input.mousePosition) <= 68)
        //         {
        //             return secondaryRangedWeapons[i];
        //         }
        //     }
        //
        //     return null;
        // }
        //
        //
        // // Gets the closest melee weapon slot to the mouse position
        // private MeleeWeaponStats getClosestMeeleWeapons()
        // {
        //     for (int i = 0; i < _meleeweaponSlots.Length; i++)
        //     {
        //         if (Vector2.Distance(_meleeweaponSlots[i].transform.position, Input.mousePosition) <= 68)
        //         {
        //             return meleeWeapons[i];
        //         }
        //     }
        //
        //     return null;
        // }

        // private WeaponData getClosestPrimaryWeapons()
        // {
        //     return GetClosestWeapon(_primaryRangedWeapons, primaryWeaponSlotHolder);
        // }
        //
        // private WeaponData getClosestSecondaryWeapons()
        // {
        //     return GetClosestWeapon(_secondaryRangedWeapons, secondaryWeaponSlotHolder);
        // }
        //
        // private WeaponData getClosestMeeleWeapons()
        // {
        //     return GetClosestWeapon(_meleeWeapons, meleeWeaponSlotHolder);
        // }
        //
        // private T GetClosestWeapon<T>(List<T> weaponList, GameObject slotHolder) where T : WeaponData
        // {
        //     for (int i = 0; i < weaponList.Count; i++)
        //     {
        //         GameObject slot = slotHolder.transform.GetChild(i).gameObject;
        //         if (Vector2.Distance(slot.transform.position, Input.mousePosition) <= 68)
        //         {
        //             return weaponList[i];
        //         }
        //     }
        //
        //     return null;
        // }

        
        // // Populates weapon slots in the UI with icons
        // public void AddIcon()
        // {
        //     for (int i = 0; i < _primaryweaponSlots.Length; i++)
        //     {
        //         try
        //         {
        //             _primaryweaponSlots[i].transform.GetChild(0).GetComponent<Image>().enabled = true;
        //             _primaryweaponSlots[i].transform.GetChild(0).GetComponent<Image>().sprite = primaryRangedWeapons[i].itemIcon;
        //         }
        //         catch (Exception e)
        //         {
        //             _primaryweaponSlots[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
        //             _primaryweaponSlots[i].transform.GetChild(0).GetComponent<Image>().sprite = null;
        //         }
        //     
        //     }
        //
        //     for (int i = 0; i < _secondaryweaponSlots.Length; i++)
        //     {
        //         try
        //         {
        //             _secondaryweaponSlots[i].transform.GetChild(0).GetComponent<Image>().enabled = true;
        //             _secondaryweaponSlots[i].transform.GetChild(0).GetComponent<Image>().sprite = secondaryRangedWeapons[i].itemIcon;
        //         }
        //         catch (Exception e)
        //         {
        //             _secondaryweaponSlots[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
        //             _secondaryweaponSlots[i].transform.GetChild(0).GetComponent<Image>().sprite = null;
        //         }
        //     
        //     }
        //
        //     for (int i = 0; i < _meleeweaponSlots.Length; i++)
        //     {
        //         try
        //         {
        //             _meleeweaponSlots[i].transform.GetChild(0).GetComponent<Image>().enabled = true;
        //             _meleeweaponSlots[i].transform.GetChild(0).GetComponent<Image>().sprite = meleeWeapons[i].itemIcon;
        //         }
        //         catch (Exception e)
        //         {
        //             _meleeweaponSlots[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
        //             _meleeweaponSlots[i].transform.GetChild(0).GetComponent<Image>().sprite = null;
        //         }
        //     
        //     }
        // }

        // Gets the selected loadout based on mouse proximity
        // internal List<string> getSelectedLoadout()
        // {
        //     var primaryWeapon = getClosestPrimaryWeapons();
        //     if (primaryWeapon != null)
        //     {
        //         _loadout[0] = primaryWeapon.weaponStats;
        //         Debug.Log($"Primary Weapon Selected: {primaryWeapon.WeaponID}");
        //     }
        //
        //     var secondaryWeapon = getClosestSecondaryWeapons();
        //     if (secondaryWeapon != null)
        //     {
        //         _loadout[1] = secondaryWeapon.WeaponID;
        //         Debug.Log($"Secondary Weapon Selected: {secondaryWeapon.WeaponID}");
        //     }
        //
        //     var meleeWeapon = getClosestMeeleWeapons();
        //     if (meleeWeapon != null)
        //     {
        //         _loadout[2] = meleeWeapon.WeaponID;
        //         Debug.Log($"Melee Weapon Selected: {meleeWeapon.WeaponID}");
        //     }
        //
        //     return _loadout;
        // }
        
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
