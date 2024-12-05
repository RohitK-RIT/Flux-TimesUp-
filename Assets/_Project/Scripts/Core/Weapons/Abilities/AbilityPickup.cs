using _Project.Scripts.Core.Backend.Interfaces;
using _Project.Scripts.Core.Backend.Scene_Control;
using UnityEngine;

namespace _Project.Scripts.Core.Weapons.Abilities
{
    public class AbilityPickup : MonoBehaviour, IPickupItem
    {
        public AbilityType AbilityType => abilityType;

        [SerializeField] private AbilityType abilityType;

        public void OnItemPickup()
        {
            // Switch the ability equipped to the new ability
            LevelSceneController.Instance.Player.WeaponController.SwitchAbility(AbilityType);
            Destroy(gameObject);
        }
    }
}