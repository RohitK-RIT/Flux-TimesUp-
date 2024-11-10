using UnityEngine;

namespace _Project.Scripts.Core.Weapons.Abilities
{
    public class ShieldAbility: PlayerAbility
    {
        public static bool HasShield { get; private set; }
        [SerializeField] private float abilityDuration;
        private bool _isAbilityActive = false;
        private bool _isCooldownActive = false;

        public override void OnEquip()
        {
            base.OnEquip();
            Used = true;
        }
        /// <summary>
        /// Function to use the shield ability.
        /// </summary>
        public void UseShield()
        {
            if (_isAbilityActive || _isCooldownActive)
            {
                Debug.Log("Ability is on cooldown or already active.");
                return;
            }
            HasShield = true;
            _isAbilityActive = true;
            //No damage is taken when shield is active
            //currentPlayerController.TakeDamage(0);
            abilityDuration = 7f;
            Debug.Log("Player is using the shield ability!!");
            StartCoroutine(DeactivateAbility(abilityDuration));
        }
    }
}