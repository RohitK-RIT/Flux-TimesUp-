using System.Collections;
using UnityEngine;

namespace _Project.Scripts.Core.Weapons.Abilities
{
    public class ShieldAbility : PlayerAbility
    {
        public bool IsActive { get; private set; }
        [SerializeField] private float abilityDuration;

        public override void OnEquip()
        {
            base.OnEquip();
            UseShield();
            Used = true;
        }

        /// <summary>
        /// Function to use the shield ability.
        /// </summary>
        private void UseShield()
        {
            if (_isAbilityActive || _isCooldownActive)
            {
                Debug.Log("Ability is on cooldown or already active.");
                return;
            }

            IsActive = true;
            _isAbilityActive = true;
            abilityDuration = 7f;
            currentPlayerController.StartCoroutine(DeactivateAbility(abilityDuration));
        }

        /// <summary>
        /// Coroutine to deactivate the ability after a certain time.
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        private IEnumerator DeactivateAbility(float time)
        {
            yield return new WaitForSeconds(time);
            Debug.Log("Ability deactivated!!");
            IsActive = false;
            currentPlayerController.StartCoroutine(StartCooldown());
        }

        protected override IEnumerator OnAttack()
        {
            yield break;
        }
    }
}