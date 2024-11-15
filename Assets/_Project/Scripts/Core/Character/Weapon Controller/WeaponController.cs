using System.Collections;
using _Project.Scripts.Core.Backend.Ability;
using _Project.Scripts.Core.Player_Controllers;
using _Project.Scripts.Core.Weapons;
using _Project.Scripts.Core.Weapons.Abilities;
using _Project.Scripts.Core.Weapons.Ranged;
using UnityEngine;

namespace _Project.Scripts.Core.Character.Weapon_Controller
{
    /// <summary>
    /// Manages the player's weapons and abilities, allowing for weapon switching and ability usage.
    /// </summary>
    public sealed class WeaponController : CharacterComponent
    {
        [SerializeField] private Transform weaponParent;

        /// <summary>
        /// The currently equipped weapon.
        /// </summary>
        [SerializeField] private Weapon currentWeapon;

        /// <summary>
        /// Array of all available weapons.
        /// </summary>
        [SerializeField] private Weapon[] weapons;

        /// <summary>
        /// Gets or sets the current weapon. Deactivates the previous weapon and activates the new one.
        /// </summary>
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

                if (currentWeapon)
                {
                    currentWeapon.OnUnequip();
                    currentWeapon.gameObject.SetActive(false);
                }

                currentWeapon = value;

                currentWeapon.gameObject.SetActive(true);
                currentWeapon.OnEquip();
            }
        }

        /// <summary>
        /// Gets the current ability.
        /// </summary>
        public PlayerAbility CurrentAbility { get; private set; }

        /// <summary>
        /// The index of the current weapon.
        /// </summary>
        private int _currentWeaponIndex;

        public override void Initialize(PlayerController playerController)
        {
            base.Initialize(playerController);

            // The player controller has picked up all the weapons
            foreach (var weapon in weapons)
                weapon?.OnPickup(PlayerController);

            // The player controller has picked up the ability
            LoadAbility(playerController.CharacterStats.playerAbilityType);

            // Load the first weapon
            CurrentWeapon = weapons[_currentWeaponIndex];
        }

        /// <summary>
        /// Loads an ability by its type.
        /// </summary>
        /// <param name="playerAbilityType">type of the ability</param>
        private void LoadAbility(PlayerAbilityType playerAbilityType)
        {
            // Check if the player has no ability
            if(playerAbilityType == PlayerAbilityType.None)
                return;
            
            // Get the ability prefab
            var abilityPrefab = AbilityDataSystem.Instance.GetAbilityPrefab(playerAbilityType);
            // Check if the ability prefab is not null
            if (abilityPrefab)
            {
                // Instantiate the ability prefab
                CurrentAbility = Instantiate(abilityPrefab, weaponParent);
                CurrentAbility.gameObject.SetActive(false);
                CurrentAbility.OnPickup(PlayerController);
            }
            else
            {
                Debug.LogError("Ability Prefab not found");
            }
        }

        /// <summary>
        /// Loads a weapon by its ID.
        /// </summary>
        /// <param name="weaponID">The ID of the weapon to load.</param>
        private void LoadWeapon(string weaponID)
        {
            // Load the weapon
        }

        /// <summary>
        /// Switches the weapon by a delta value.
        /// </summary>
        /// <param name="delta">The value with which the weapon switches</param>
        public void SwitchWeapon(int delta)
        {
            _currentWeaponIndex += delta;
            if (_currentWeaponIndex < 0)
                _currentWeaponIndex = weapons.Length - 1;
            else if (_currentWeaponIndex >= weapons.Length)
                _currentWeaponIndex = 0;

            CurrentWeapon = weapons[_currentWeaponIndex];
        }

        /// <summary>
        /// Equips the current ability and starts the weapon switch coroutine.
        /// </summary>
        public void OnAbilityEquipped()
        {
            if (!CurrentAbility)
            {
                Debug.LogError("Ability not found");
                return;
            }

            CurrentWeapon = CurrentAbility;

            StartCoroutine(HandleWeaponSwitch(CurrentAbility));
        }

        /// <summary>
        /// Reloads the current weapon.
        /// </summary>
        public void ReloadWeapon()
        {
            if (currentWeapon is RangedWeapon rangedWeapon)
                rangedWeapon.OnReload();
        }

        /// <summary>
        /// Handles the weapon switch when an ability is used.
        /// </summary>
        /// <param name="ability">The ability being used.</param>
        /// <returns>An IEnumerator for the coroutine.</returns>
        private IEnumerator HandleWeaponSwitch(PlayerAbility ability)
        {
            // Handle weapon switch
            yield return new WaitUntil(() => ability.Used);
            CurrentWeapon = weapons[_currentWeaponIndex];
        }

        /// <summary>
        /// Begins the attack with the current weapon.
        /// </summary>
        public void BeginAttack()
        {
            CurrentWeapon.BeginAttack();
        }

        /// <summary>
        /// Ends the attack with the current weapon.
        /// </summary>
        public void EndAttack()
        {
            CurrentWeapon.EndAttack();
        }
    }
}