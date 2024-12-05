using _Project.Scripts.Core.Backend.Interfaces;
using UnityEngine;

namespace _Project.Scripts.Core.Weapons.Abilities
{
    public class AbilityPickup : MonoBehaviour, IPickupItem
    {
        public AbilityType AbilityType => abilityType;

        [SerializeField] private AbilityType abilityType;

        public void OnItemPickup()
        {
            Debug.Log("Picked up ability: " + AbilityType);
        }
    }
}