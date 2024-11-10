using System;
using System.Collections;
using System.Linq;
using _Project.Scripts.Core.Player_Controllers;
using _Project.Scripts.Core.Weapons;
using _Project.Scripts.Core.Weapons.Abilities;
using _Project.Scripts.Core.Weapons.Ranged;
using UnityEngine;

namespace _Project.Scripts.Core.Character.Weapon_Controller
{
    public sealed class WeaponController : CharacterComponent
    {
        [SerializeField] private Weapon currentWeapon;
        [SerializeField] private Weapon[] weapons;
        public Weapon CurrentWeapon => currentWeapon;

        private int _currentWeaponIndex = 0;

        private void Start()
        {
            // Find the first weapon in the weapons array that is of type PlayerAbility and cast it to PlayerAbility
            var playerAbility = weapons.First(weapon => weapon is PlayerAbility) as PlayerAbility;
            // If a PlayerAbility was found, set its currentPlayerController to the PlayerController casted as LocalPlayerController
            if(playerAbility)
            {
                playerAbility.currentPlayerController = PlayerController as LocalPlayerController;
            }
        }

        private void LoadWeapon(string weaponID)
        {
            // Load the weapon
        }

        public void OnAbilityEquipped()
        {
            currentWeapon.gameObject.SetActive(false);
            var playerAbility = weapons.First(weapon => weapon is PlayerAbility);
            if(!playerAbility)
            {
                Debug.LogError("Ability not found");
                return;
            }
            currentWeapon = playerAbility;
            currentWeapon.gameObject.SetActive(true);
            
            switch (PlayerController.CharacterStats.playerAbilityType)
            {
                case PlayerAbilityType.Shield:
                {
                    var shieldAbility = playerAbility as ShieldAbility;
                    shieldAbility?.OnEquip();
                    StartCoroutine(HandleWeaponSwitch(shieldAbility));
                    shieldAbility?.UseShield();
                    break;
                }
                case PlayerAbilityType.Heal:
                    var healAbility = playerAbility as HealAbility;
                    healAbility?.UseHeal();
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        
        private IEnumerator HandleWeaponSwitch(PlayerAbility ability)
        {
            Debug.Log("Switched back to weapon");
            // Handle weapon switch
            yield return new WaitUntil(() => ability.Used);
            currentWeapon.gameObject.SetActive(false);
            currentWeapon = weapons[_currentWeaponIndex];
            currentWeapon.gameObject.SetActive(true);
            Debug.Log("madarchod");
        }

        public void BeginAttack()
        {
            CurrentWeapon.BeginAttack();
        }

        public void EndAttack()
        {
            CurrentWeapon.EndAttack();
        }
    }
}