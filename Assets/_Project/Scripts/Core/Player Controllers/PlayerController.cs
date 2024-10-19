using _Project.Scripts.Core.Character;
using _Project.Scripts.Core.Character.Weapon_Controller;
using UnityEngine;

namespace _Project.Scripts.Core.Player_Controllers
{
    [RequireComponent(typeof(MovementController), typeof(WeaponController))]
    public abstract class PlayerController : MonoBehaviour
    {
        /// <summary>
        /// Component that handles movement.
        /// </summary>
        protected MovementController MovementController { get; private set; }

        /// <summary>
        /// Component that handles weapons.
        /// </summary>
        protected WeaponController WeaponController { get; private set; }

        protected virtual void Awake()
        {
            // Get the CharacterMovement and CharacterWeaponController component attached to the player
            MovementController = GetComponent<MovementController>();
            WeaponController = GetComponent<WeaponController>();
        }

        /// <summary>
        /// Update the player's movement direction.
        /// </summary>
        /// <param name="direction">direction in which player should move</param>
        protected void UpdateMoveDirection(Vector2 direction)
        {
            MovementController.moveDirection = direction;
        }

        /// <summary>
        /// Begin the player's attack.
        /// </summary>
        protected void BeginAttack()
        {
            WeaponController.BeginAttack();
        }

        /// <summary>
        /// End the player's attack.
        /// </summary>
        protected void EndAttack()
        {
            WeaponController.EndAttack();
        }

        /// <summary>
        /// Update the player's look direction.
        /// </summary>
        /// <param name="lookInput">Look input</param>
        protected abstract void LookInputUpdated(Vector2 lookInput);
    }
}