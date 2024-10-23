using System;
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
        /// Aim transform of a character.
        /// </summary>
        public Transform AimTransform => aimTransform;

        /// <summary>
        /// Body of the player.
        /// </summary>
        public Transform Body => body;

        /// <summary>
        /// The direction the player is moving in.
        /// </summary>
        [NonSerialized] public Vector2 MoveInput;

        /// <summary>
        /// Aim transform of a character.
        /// </summary>
        [SerializeField] private Transform aimTransform;

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

        private void Awake()
        {
            // Get and store the CharacterController component attached to the player
            _characterController = GetComponent<CharacterController>();
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
        /// This method is called to rotate the character based on aim transform's position.
        /// </summary>
        private void HandleLook()
        {
            var horizontalTargetLocation = new Vector3(aimTransform.position.x, body.position.y, aimTransform.position.z);
            var horizontalDirection = (horizontalTargetLocation - transform.position).normalized;

            body.forward = Vector3.Lerp(body.forward, horizontalDirection, Time.deltaTime * 20f);

            var dy = aimTransform.position.y - weaponParent.position.y;
            var dz = aimTransform.position.z - weaponParent.position.z;
            var verticalAngle = Mathf.Asin(dy / dz) * Mathf.Rad2Deg;
            var weaponTargetRotation = Quaternion.Euler(-verticalAngle, weaponParent.localRotation.eulerAngles.y, weaponParent.localRotation.eulerAngles.z);

            weaponParent.localRotation = Quaternion.Slerp(weaponParent.localRotation, weaponTargetRotation, Time.deltaTime * 20f);
        }
    }
}