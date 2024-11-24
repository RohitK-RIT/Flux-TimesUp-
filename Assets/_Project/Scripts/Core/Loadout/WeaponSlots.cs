using _Project.Scripts.Core.Weapons;
using UnityEngine;

namespace _Project.Scripts.Core.Loadout
{
    public class WeaponSlots : MonoBehaviour
    {
        // The ID of the weapon that this slot represents. 
        // This ID is used to uniquely identify the weapon in the loadout system.
        public string weaponId;

        // The type of weapon associated with this slot (Primary, Secondary, or Melee).
        // This helps categorize the weapon and assign it to the correct loadout slot.
        public WeaponType weaponType;
    }
}
