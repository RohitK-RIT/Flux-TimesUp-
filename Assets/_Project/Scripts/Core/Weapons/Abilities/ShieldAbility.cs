using System.Collections;
using UnityEngine;

namespace _Project.Scripts.Core.Weapons.Abilities
{
    /// <summary>
    /// Represents the shield ability for the player.
    /// </summary>
    public class ShieldAbility : PlayerAbility
    {
        /// <summary>
        /// Gets a value indicating whether the shield is active.
        /// </summary>
        public bool IsActive { get; private set; }

        /// <summary>
        /// The duration for which the shield ability remains active.
        /// </summary>
        [SerializeField] private float abilityDuration;

        /// <summary>
        /// The visual representation of the shield.
        /// </summary>
        [SerializeField] private GameObject shieldVisual;

        /// <summary>
        /// Called when the ability is equipped.
        /// </summary>
        public override void OnEquip()
        {
            base.OnEquip();
            UseShield();
            Used = true;
        }

        /// <summary>
        /// Activates the shield ability.
        /// </summary>
        private void UseShield()
        {
            if (_isAbilityActive || _isCooldownActive)
            {
                Debug.Log("Ability is on cooldown or already active.");
                return;
            }
            SetShieldVisual(true);
            IsActive = true;
            _isAbilityActive = true;
            abilityDuration = 7f;
            currentPlayerController.StartCoroutine(DeactivateAbility(abilityDuration));
        }

        /// <summary>
        /// Coroutine to deactivate the ability after a certain time.
        /// </summary>
        /// <param name="time">The duration after which the ability should be deactivated.</param>
        /// <returns>An IEnumerator for the coroutine.</returns>
        private IEnumerator DeactivateAbility(float time)
        {
            yield return new WaitForSeconds(time);
            Debug.Log("Ability deactivated!!");
            IsActive = false;
            SetShieldVisual(false);
            currentPlayerController.StartCoroutine(StartCooldown());
        }

        /// <summary>
        /// Overrides the OnAttack method to provide custom attack behavior for the shield ability.
        /// </summary>
        /// <returns>An IEnumerator for the coroutine.</returns>
        protected override IEnumerator OnAttack()
        {
            yield break;
        }

        /// <summary>
        /// Sets the visual representation of the shield.
        /// </summary>
        /// <param name="value">A boolean value indicating whether to activate or deactivate the shield visual.</param>
        private void SetShieldVisual(bool value)
        {
            shieldVisual.SetActive(value);
        }
    }
}