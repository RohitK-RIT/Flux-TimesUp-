using _Project.Scripts.Core.Backend.Currency;
using _Project.Scripts.Core.Player_Controllers.Input_Controllers;
using _Project.Scripts.Core.Weapons.Abilities.Shield;
using UnityEngine;

namespace _Project.Scripts.Core.Player_Controllers
{
    /// <summary>
    /// This class is responsible for handling the player's input.
    /// </summary>
    [RequireComponent(typeof(LocalInputController), typeof(CameraController))]
    public class LocalPlayerController : PlayerController
    {
        /// <summary>
        /// Component that handles player input.
        /// </summary>
        private LocalInputController _localInputController;

        /// <summary>
        /// Component that handles the player's camera.
        /// </summary>
        private CameraController _cameraController;

        // This will go in player info eventually.
        [SerializeField] private float aimSensitivity = 1f;

        private string _walletID;

        protected override void Awake()
        {
            base.Awake();

            _localInputController = GetComponent<LocalInputController>();
            _cameraController = GetComponent<CameraController>();

            _localInputController.Initialize(this);
            _cameraController.Initialize(this);

            _walletID = CurrencySystem.Instance.CreateWallet();
        }

        private void OnEnable()
        {
            // Subscribe to input events
            _localInputController.OnMoveInputUpdated += SetMoveInput;

            _localInputController.OnAttackInputBegan += BeginAttack;
            _localInputController.OnAttackInputEnded += EndAttack;

            _localInputController.OnLookInputUpdated += SetLookInput;

            _localInputController.OnAbilityEquipped += AbilityEquipped;

            _localInputController.OnSwitchWeaponInput += SwitchWeapon;
            _localInputController.OnReloadInput += Reload;
            
            WeaponController.OnKillEnemy += OnEnemyKilled;
            WeaponController.OnHitEnemy += OnEnemyHit;
        }

        private void OnDisable()
        {
            // Unsubscribe from input events
            _localInputController.OnMoveInputUpdated -= SetMoveInput;

            _localInputController.OnAttackInputBegan -= BeginAttack;
            _localInputController.OnAttackInputEnded -= EndAttack;

            _localInputController.OnLookInputUpdated -= SetLookInput;

            _localInputController.OnAbilityEquipped -= AbilityEquipped;

            _localInputController.OnSwitchWeaponInput -= SwitchWeapon;
            _localInputController.OnReloadInput -= Reload;
            
            WeaponController.OnKillEnemy -= OnEnemyKilled;
            WeaponController.OnHitEnemy -= OnEnemyHit;
        }

        /// <summary>
        /// Update the player's look direction.
        /// </summary>
        /// <param name="lookInput">look input to the player</param>
        private void SetLookInput(Vector2 lookInput)
        {
            _cameraController.LookInput = lookInput * aimSensitivity;
        }

        /// <summary>
        /// Function to equip the player's ability.
        /// </summary>
        private void AbilityEquipped()
        {
            WeaponController.OnAbilityEquipped();
        }

        /// <summary>
        /// Overrides the TakeDamage method to include shield ability check.
        /// </summary>
        /// <param name="damageAmount">The amount of damage to be taken.</param>
        /// <returns>if the player is dead</returns>
        public override bool TakeDamage(float damageAmount)
        {
            // Check if the shield ability is active, if so, return false
            var shield = WeaponController.CurrentAbility as ShieldAbility;
            if (shield && shield.IsActive)
                return false;

            // If the shield ability is not active, take damage
            return base.TakeDamage(damageAmount);
        }
        
        /// <summary>
        /// Called when an enemy is killed.
        /// </summary>
        private void OnEnemyKilled()
        {
            CurrencySystem.Instance.AddCoins(_walletID, 10);
        }

        /// <summary>
        /// Called when an enemy is hit.
        /// </summary>
        private void OnEnemyHit()
        {
            // Empty for now
        }
        
        public int GetCoins()
        {
            return CurrencySystem.Instance.GetCoins(_walletID);
        }

#if UNITY_EDITOR
        //to be removed
        [ContextMenu("Take Damage")]
        public void TakeDamage()
        {
            TakeDamage(10);
        }

        [ContextMenu("Heal")]
        public void Heal()
        {
            Heal(10);
        }
#endif
    }
}