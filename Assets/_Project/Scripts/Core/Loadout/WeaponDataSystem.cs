using System.Collections.Generic;
using _Project.Scripts.Core.Backend;
using _Project.Scripts.Core.Weapons;
using UnityEngine;

namespace _Project.Scripts.Core.Loadout
{
    public class WeaponDataSystem : BaseSystem<WeaponDataSystem>
    {
        [SerializeField] private WeaponData[] weaponDatabase;

        /// <summary>
        /// Fetches the weapon prefab based on the provided WeaponID.
        /// </summary>
        /// <param name="weaponID">The ID of the weapon to fetch.</param>
        /// <returns>The corresponding weapon prefab, or null if not found.</returns>
        public Weapon GetWeaponPrefab(string weaponID)
        {
            foreach (var weaponData in weaponDatabase) // Iterate through weapon database
            {
                foreach (var weaponStat in weaponData.weaponStats) // Iterate through stats within each weapon
                {
                    if (weaponStat.WeaponID == weaponID) // Check the WeaponID property
                    {
                        return weaponData.weaponPrefab; // Return the prefab from the current weapon data
                    }
                }
            }

            Debug.LogWarning($"Weapon with ID {weaponID} not found in the database!");
            return null;
        }

        protected override bool IsPersistent => true;
    }
}
