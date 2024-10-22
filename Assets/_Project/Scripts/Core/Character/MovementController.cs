using System;
using _Project.Scripts.Core.Character.Weapon_Controller;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Project.Scripts.Core.Character
{
    /// <summary>
    /// This class is responsible for moving the character based on the movement input.
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
    public class MovementController : CharacterComponent
    {
        /// <summary>
        /// The direction the player is looking in.
        /// </summary>
        public Vector2 LookDirection
        {
            get => _lookDirection;
            set
            {
                // Clamp the vertical look direction to prevent the player from looking too far up or down.
                var clampedValue = new Vector2(value.x, Mathf.Clamp(value.y, -90f, 90f));
                _lookDirection = clampedValue;
            }
        }

        /// <summary>
        /// The direction the player is moving in.
        /// </summary>
        [NonSerialized] public Vector2 MoveInput;

        /// <summary>
        /// Body of the player.
        /// </summary>
        [SerializeField] private Transform body;

        /// <summary>
        /// Weapon parent of the character.
        /// </summary>
        [SerializeField] private Transform weaponParent;

        /// <summary>
        /// The speed at which the player moves.
        /// </summary>
        [SerializeField] private float moveSpeed = 5f;

        /// <summary>
        /// The CharacterController component attached to the character.
        /// </summary>
        private CharacterController _characterController;

        /// <summary>
        /// The direction the player is moving in.
        /// </summary>
        private Vector2 _moveDirection;

        /// <summary>
        /// The direction the player is looking in.
        /// </summary>
        private Vector2 _lookDirection;

        private void Awake()
        {
            // Get and store the CharacterController component attached to the player
            _characterController = GetComponent<CharacterController>();
        }

        private void Start()
        {
            // Set the look direction to the forward direction of the body.
            LookDirection = new Vector2(body.forward.x, body.forward.z);
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
            if (MoveInput == Vector2.zero) return;

            // Assign horizontal and vertical inputs to the movement vector
            var currentMovement = (body.right * MoveInput.x + body.forward * MoveInput.y) * PlayerController.CharacterStats.movementSpeed;
            
            // Can add jump here if needed by modifying the y component of the movement vector. 

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
            var bodyTargetRotation = Quaternion.Euler(bodyLocalRotation.eulerAngles.x, _lookDirection.x, bodyLocalRotation.eulerAngles.z);
            body.transform.localRotation = Quaternion.Slerp(body.transform.localRotation, bodyTargetRotation, Time.deltaTime * 50f);

            // Assign negative of the Vertical component of the look direction to the x-axis rotation of the weapon.
            var weaponLocalRotation = weaponParent.localRotation;
            var weaponTargetRotation = Quaternion.Euler(-_lookDirection.y, weaponLocalRotation.eulerAngles.y, weaponLocalRotation.eulerAngles.z);
            weaponParent.localRotation = Quaternion.Slerp(weaponLocalRotation, weaponTargetRotation, Time.deltaTime * 50f);
        }
    }
}