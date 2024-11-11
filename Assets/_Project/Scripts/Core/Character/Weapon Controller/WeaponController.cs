using System.Collections;
using _Project.Scripts.Core.Player_Controllers;
using _Project.Scripts.Core.Weapons;
using _Project.Scripts.Core.Weapons.Abilities;
using UnityEngine;

namespace _Project.Scripts.Core.Character.Weapon_Controller
{
    /// <summary>
    /// Manages the player's weapons and abilities, allowing for weapon switching and ability usage.
    /// </summary>
    public sealed class WeaponController : CharacterComponent
    {
        /// <summary>
        /// The currently equipped weapon.
        /// </summary>
        [SerializeField] private Weapon currentWeapon;

        /// <summary>
        /// The currently equipped ability.
        /// </summary>
        [SerializeField] private PlayerAbility currentAbility;

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

                currentWeapon?.gameObject.SetActive(false);
                currentWeapon = value;
                currentWeapon?.gameObject.SetActive(true);
                currentWeapon?.OnEquip(PlayerController);
            }
        }

        /// <summary>
        /// Gets the current ability.
        /// </summary>
        public PlayerAbility CurrentAbility => currentAbility;

        private int _currentWeaponIndex = 0;

        /// <summary>
        /// Initializes the weapon controller, setting the player controller for the current ability.
        /// </summary>
        private void Start()
        {
            CurrentWeapon = weapons[_currentWeaponIndex];
        }

        /// <summary>
        /// Loads a weapon by its ID.
        /// </summary>
        /// <param name="weaponID">The ID of the weapon to load.</param>
        private void LoadWeapon(string weaponID)
        {
            // Load the weapon
        }
        
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