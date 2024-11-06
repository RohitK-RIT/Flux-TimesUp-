﻿using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace _Project.Scripts.Core.Player_Controllers.Input_Controllers
{
    /// <summary>
    /// This class is responsible for handling the player's input.
    /// </summary>
    public class LocalInputController : InputController
    {
        /// <summary>
        /// Event that is called when the player moves the character.
        /// </summary>
        public override event Action<Vector2> OnMoveInputUpdated;

        /// <summary>
        /// Event that is called when the player looks around.
        /// </summary>
        public event Action<Vector2> OnLookInputUpdated;

        /// <summary>
        /// Event that is called when the player starts attacking.
        /// </summary>
        public override event Action OnAttackInputBegan;

        /// <summary>
        /// Event that is called when the player stops attacking.
        /// </summary>
        public override event Action OnAttackInputEnded;

        /// <summary>
        /// Component that handles player input Unity API calls.
        /// </summary>
        private PlayerInput _playerInput;

        private void Awake()
        {
            _playerInput = new PlayerInput();
        }

        public void OnEnable()
        {
            // Subscribe to input events
            // Move Input Events
            _playerInput.PlayerControl.Move.started += OnMoveInputReceived;
            _playerInput.PlayerControl.Move.performed += OnMoveInputReceived;
            _playerInput.PlayerControl.Move.canceled += OnMoveInputReceived;

            // Attack Input Events
            _playerInput.PlayerControl.Attack.started += OnAttackInputStarted;
            _playerInput.PlayerControl.Attack.canceled += OnAttackInputCancelled;

            // Look Input Events
            _playerInput.PlayerControl.Look.performed += OnLookInputReceived;
            _playerInput.PlayerControl.Look.canceled += OnLookInputReceived;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            // Enable the PlayerInput component
            _playerInput.PlayerControl.Enable();
        }

        public void OnDisable()
        {
            // Unsubscribe from input events
            // Move Input Events
            _playerInput.PlayerControl.Move.started -= OnMoveInputReceived;
            _playerInput.PlayerControl.Move.performed -= OnMoveInputReceived;
            _playerInput.PlayerControl.Move.canceled -= OnMoveInputReceived;

            // Attack Input Events
            _playerInput.PlayerControl.Attack.started -= OnAttackInputStarted;
            _playerInput.PlayerControl.Attack.canceled -= OnAttackInputCancelled;

            // Look Input Events
            _playerInput.PlayerControl.Look.performed -= OnLookInputReceived;
            _playerInput.PlayerControl.Look.canceled -= OnLookInputReceived;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            // Disable the PlayerInput component
            _playerInput.PlayerControl.Disable();
        }

        private void OnDestroy()
        {
            _playerInput.Dispose();
        }

        // Input Event Handlers

        #region Movement Input

        /// <summary>
        /// This method is called when the player is moving the character.
        /// </summary>
        /// <param name="context">struct that holds the movement input</param>
        private void OnMoveInputReceived(InputAction.CallbackContext context)
        {
            // Invoke the OnMoveInputUpdated event with the input value.
            OnMoveInputUpdated?.Invoke(context.ReadValue<Vector2>());
        }

        #endregion

        #region Attack Input

        /// <summary>
        /// This method is called when the player starts attacking.
        /// </summary>
        /// <param name="context">struct that hold the action context</param>
        private void OnAttackInputStarted(InputAction.CallbackContext context)
        {
            // Invoke the OnAttackInputBegan event.
            OnAttackInputBegan?.Invoke();
        }

        /// <summary>
        /// This method is called when the player stops attacking.
        /// </summary>
        /// <param name="context">struct that hold the action context</param>
        private void OnAttackInputCancelled(InputAction.CallbackContext context)
        {
            // Invoke the OnAttackInputEnded event.
            OnAttackInputEnded?.Invoke();
        }

        #endregion

        #region Look Input

        /// <summary>
        /// This method is called when the player is looking around.
        /// </summary>
        /// <param name="context">struct that hold the action context</param>
        private void OnLookInputReceived(InputAction.CallbackContext context)
        {
            // Invoke the OnLookInputUpdated event with the input value.
            OnLookInputUpdated?.Invoke(context.ReadValue<Vector2>());
        }

        #endregion
    }
}