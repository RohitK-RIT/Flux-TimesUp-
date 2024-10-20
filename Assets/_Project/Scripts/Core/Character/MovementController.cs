using _Project.Scripts.Core.Character.Weapon_Controller;
using UnityEngine;

namespace _Project.Scripts.Core.Character
{
    /// <summary>
    /// This class is responsible for moving the character based on the movement input.
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class MovementController : MonoBehaviour
    {
        /// <summary>
        /// The direction the player is looking in.
        /// </summary>
        public Vector2 LookDirection
        {
            get => lookDirection;
            set
            {
                // Clamp the vertical look direction to prevent the player from looking too far up or down.
                var clampedValue = new Vector2(value.x, Mathf.Clamp(value.y, -90f, 90f));
                lookDirection = clampedValue;
            }
        }

        /// <summary>
        /// The direction the player is moving in.
        /// </summary>
        [HideInInspector] public Vector2 moveDirection;

        /// <summary>
        /// Body of the player.
        /// </summary>
        [SerializeField] private Transform body;

        /// <summary>
        /// The speed at which the player moves.
        /// </summary>
        [SerializeField] private float moveSpeed = 5f;

        /// <summary>
        /// The CharacterController component attached to the character.
        /// </summary>
        private CharacterController _characterController;

        // this is a hack to get the weapon controller to move the current weapon to the correct position. To be removed.
        private WeaponController _weaponController;

        /// <summary>
        /// The direction the player is looking in.
        /// </summary>
        [SerializeField] private Vector2 lookDirection;

        private void Awake()
        {
            // Get and store the CharacterController component attached to the player
            _characterController = GetComponent<CharacterController>();
            _weaponController = GetComponent<WeaponController>();
        }

        private void Update()
        {
            // Handle movement and look.
            HandleMovement();
            HandleLook();
        }

        /// <summary>
        /// This method is called to move the character based on the movement input.
        /// </summary>
        private void HandleMovement()
        {
            // If movement input is zero then return.
            if (moveDirection == Vector2.zero) return;

            // Assign horizontal and vertical inputs to the movement vector
            var currentMovement = new Vector3(moveDirection.x, 0.0f, moveDirection.y);

            // Move the character via the character controller.
            _characterController.Move(currentMovement * (moveSpeed * Time.deltaTime));
        }

        /// <summary>
        /// This method is called to rotate the character based on the movement input.
        /// </summary>
        private void HandleLook()
        {
            // Assign Horizontal component of the look direction to the y-axis rotation of the body.
            var bodyLocalRotation = body.transform.localRotation;
            var bodyTargetRotation = Quaternion.Euler(bodyLocalRotation.eulerAngles.x, lookDirection.x, bodyLocalRotation.eulerAngles.z);
            body.transform.localRotation = Quaternion.Slerp(body.transform.localRotation, bodyTargetRotation, Time.deltaTime * 50f);

            // Assign negative of the Vertical component of the look direction to the x-axis rotation of the weapon.
            var weaponLocalRotation = _weaponController.CurrentWeapon.transform.localRotation;
            var weaponTargetRotation = Quaternion.Euler(-lookDirection.y, weaponLocalRotation.eulerAngles.y, weaponLocalRotation.eulerAngles.z);
            // Quaternion.clam
            _weaponController.CurrentWeapon.transform.localRotation = Quaternion.Slerp(weaponLocalRotation, weaponTargetRotation, Time.deltaTime * 50f);
        }
    }
}