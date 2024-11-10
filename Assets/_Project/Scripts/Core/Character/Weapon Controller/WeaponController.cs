using System.Collections;
using System.Linq;
using _Project.Scripts.Core.Player_Controllers;
using _Project.Scripts.Core.Weapons;
using _Project.Scripts.Core.Weapons.Abilities;
using UnityEngine;

namespace _Project.Scripts.Core.Character.Weapon_Controller
{
    public sealed class WeaponController : CharacterComponent
    {
        [SerializeField] private Weapon currentWeapon;
        [SerializeField] private PlayerAbility currentAbility;
        [SerializeField] private Weapon[] weapons;

        public Weapon CurrentWeapon
        {
            get => currentWeapon;
            private set
            {
                if (!value)
                {
                    Debug.LogError("Weapon not found");
                    return;
                }

                currentWeapon.gameObject.SetActive(false);
                currentWeapon = value;
                currentWeapon.gameObject.SetActive(true);
            }
        }

        public PlayerAbility CurrentAbility => currentAbility;

        private int _currentWeaponIndex = 0;

        private void Start()
        {
            // Set its currentPlayerController to the PlayerController casted as LocalPlayerController
            if (CurrentAbility)
            {
                CurrentAbility.currentPlayerController = PlayerController as LocalPlayerController;
            }
        }

        private void LoadWeapon(string weaponID)
        {
            // Load the weapon
        }

        public void OnAbilityEquipped()
        {
            if (!CurrentAbility)
            {
                Debug.LogError("Ability not found");
                return;
            }

            CurrentWeapon = CurrentAbility;
            CurrentAbility?.OnEquip();

            StartCoroutine(HandleWeaponSwitch(CurrentAbility));
        }

        private IEnumerator HandleWeaponSwitch(PlayerAbility ability)
        {
            // Handle weapon switch
            yield return new WaitUntil(() => ability.Used);
            CurrentWeapon = weapons[_currentWeaponIndex];
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