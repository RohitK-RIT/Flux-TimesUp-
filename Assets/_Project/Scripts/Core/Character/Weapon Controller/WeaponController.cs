using System.Linq;
using _Project.Scripts.Core.Weapons;
using _Project.Scripts.Core.Weapons.Abilities;
using UnityEngine;

namespace _Project.Scripts.Core.Character.Weapon_Controller
{
    public sealed class WeaponController : CharacterComponent
    {
        [SerializeField] private Weapon currentWeapon;
        [SerializeField] private Weapon[] weapons;
        // [SerializeField] private LoadoutConfig loadoutConfig;
        public Weapon CurrentWeapon => currentWeapon;

        private int _currentWeaponIndex = 0;

        private void Start()
        {
            // LoadWeapon(loadoutConfig.PrimaryWeaponID);
        }

        private void LoadWeapon(string weaponID)
        {
            // Load the weapon
        }

        public void OnAbilityEquiped()
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