using UnityEngine;

namespace _Project.Scripts.Core.Weapons.Abilities
{
    public class HealAbility: PlayerAbility
    {
        [SerializeField] private float abilityDuration;
        private bool _isAbilityActive = false;
        private bool _isCooldownActive = false;

        public override void OnEquip()
        {
            base.OnEquip();
            Used = true;
        }
        /// <summary>
        /// Function to use the heal ability.
        /// </summary>
        public void UseHeal()
        {
            if (_isAbilityActive || _isCooldownActive)
            {
                Debug.Log("Ability is on cooldown or already active.");
                return;
            }
            _isAbilityActive = true;
            //Heal the player
            abilityDuration = 5f;
            Debug.Log("Player is using the heal ability!!");
            StartCoroutine(DeactivateAbility(abilityDuration)); 
        }
    }
}