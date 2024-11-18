using System;
using System.Collections.Generic;
using _Project.Scripts.Core.Backend;
using _Project.Scripts.Core.Weapons.Melee;
using _Project.Scripts.Core.Weapons.Ranged;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace _Project.Scripts.Core.Loadout
{
    public class LoadOutManager : MonoBehaviour
    {
        public List<RangedWeaponStats> primaryRangedWeapons = new List<RangedWeaponStats>();
        public List<RangedWeaponStats> secondaryRangedWeapons = new List<RangedWeaponStats>();
        public List<MeleeWeaponStats> meleeWeapons = new List<MeleeWeaponStats>();

        [SerializeField] private GameObject primaryWeaponSlotHolder;
        [SerializeField] private GameObject secondaryWeaponSlotHolder;
        [SerializeField] private GameObject meleeWeaponSlotHolder;

        private GameObject[] _primaryweaponSlots;
        private GameObject[] _secondaryweaponSlots;
        private GameObject[] _meleeweaponSlots;

        private List<string> _loadout = new List<string> { null, null, null };
    
        private string _nextSceneName = "Greybox";
    
        //[SerializeField] private WeaponController weaponController; // Reference to the WeaponController

    
        private void Start()
        {
            _primaryweaponSlots = new GameObject[primaryWeaponSlotHolder.transform.childCount];
            _secondaryweaponSlots = new GameObject[secondaryWeaponSlotHolder.transform.childCount];
            _meleeweaponSlots = new GameObject[meleeWeaponSlotHolder.transform.childCount];

            for (int i = 0; i < _primaryweaponSlots.Length; i++)
            {
                _primaryweaponSlots[i] = primaryWeaponSlotHolder.transform.GetChild(i).gameObject;
            }
        
            for (int i = 0; i < _secondaryweaponSlots.Length; i++)
            {
                _secondaryweaponSlots[i] = secondaryWeaponSlotHolder.transform.GetChild(i).gameObject;
            }
        
            for (int i = 0; i < _meleeweaponSlots.Length; i++)
            {
                _meleeweaponSlots[i] = meleeWeaponSlotHolder.transform.GetChild(i).gameObject;
            }
        
            AddIcon();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("Click");
                getSelectedLoadout();
                PrintLoadout();
            
                // Pass the selected loadout to WeaponController
                //WeaponController.LoadWeapon(_loadout); // Convert object list to List<int>
            }
        }

        private RangedWeaponStats getClosestPrimaryWeapons()
        {
            for (int i = 0; i < _primaryweaponSlots.Length; i++)
            {
                if (Vector2.Distance(_primaryweaponSlots[i].transform.position, Input.mousePosition) <= 68)
                {
                    return primaryRangedWeapons[i];
                }
            }

            return null;
        }
        private RangedWeaponStats getClosestSecondaryWeapons()
        {
            for (int i = 0; i < _secondaryweaponSlots.Length; i++)
            {
                if (Vector2.Distance(_secondaryweaponSlots[i].transform.position, Input.mousePosition) <= 68)
                {
                    return secondaryRangedWeapons[i];
                }
            }

            return null;
        }
    
        private MeleeWeaponStats getClosestMeeleWeapons()
        {
            for (int i = 0; i < _meleeweaponSlots.Length; i++)
            {
                if (Vector2.Distance(_meleeweaponSlots[i].transform.position, Input.mousePosition) <= 68)
                {
                    return meleeWeapons[i];
                }
            }

            return null;
        }

        public void AddIcon()
        {
            for (int i = 0; i < _primaryweaponSlots.Length; i++)
            {
                try
                {
                    _primaryweaponSlots[i].transform.GetChild(0).GetComponent<Image>().enabled = true;
                    _primaryweaponSlots[i].transform.GetChild(0).GetComponent<Image>().sprite = primaryRangedWeapons[i].itemIcon;
                }
                catch (Exception e)
                {
                    _primaryweaponSlots[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
                    _primaryweaponSlots[i].transform.GetChild(0).GetComponent<Image>().sprite = null;
                }
            
            }
        
            for (int i = 0; i < _secondaryweaponSlots.Length; i++)
            {
                try
                {
                    _secondaryweaponSlots[i].transform.GetChild(0).GetComponent<Image>().enabled = true;
                    _secondaryweaponSlots[i].transform.GetChild(0).GetComponent<Image>().sprite = secondaryRangedWeapons[i].itemIcon;
                }
                catch (Exception e)
                {
                    _secondaryweaponSlots[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
                    _secondaryweaponSlots[i].transform.GetChild(0).GetComponent<Image>().sprite = null;
                }
            
            }
        
            for (int i = 0; i < _meleeweaponSlots.Length; i++)
            {
                try
                {
                    _meleeweaponSlots[i].transform.GetChild(0).GetComponent<Image>().enabled = true;
                    _meleeweaponSlots[i].transform.GetChild(0).GetComponent<Image>().sprite = meleeWeapons[i].itemIcon;
                }
                catch (Exception e)
                {
                    _meleeweaponSlots[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
                    _meleeweaponSlots[i].transform.GetChild(0).GetComponent<Image>().sprite = null;
                }
            
            }
        }

        internal List<string> getSelectedLoadout()
        {
            var primaryWeapon = getClosestPrimaryWeapons();
            if (primaryWeapon)
            {
                _loadout[0] = primaryWeapon.WeaponID;
                Debug.Log($"Primary Weapon Selected: {primaryWeapon.WeaponID}");
            }

            var secondaryWeapon = getClosestSecondaryWeapons();
            if (secondaryWeapon)
            {
                _loadout[1] = secondaryWeapon.WeaponID;
                Debug.Log($"Secondary Weapon Selected: {secondaryWeapon.WeaponID}");
            }

            var meleeWeapon = getClosestMeeleWeapons();
            if (meleeWeapon)
            {
                _loadout[2] = meleeWeapon.WeaponID;
                Debug.Log($"Melee Weapon Selected: {meleeWeapon.WeaponID}");
            }

            return _loadout;
        }
        private void PrintLoadout()
        {
            Debug.Log("Current Loadout:");
            Debug.Log($"Primary: {_loadout[0]}");
            Debug.Log($"Secondary: {_loadout[1]}");
            Debug.Log($"Melee: {_loadout[2]}");
        }
    
        public void OnContinueButtonClicked()
        {
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

        private bool _isLoadOutComplete()
        {
            return _loadout[0] != null && _loadout[1] != null && _loadout[2] != null;
        }
        
        public void TransferLoadoutToWeaponDataSystem()
        {
            WeaponDataSystem.Instance.SetSelectedWeapons(_loadout); // Assuming `_loadout` contains weapon IDs
        }

    }
}
