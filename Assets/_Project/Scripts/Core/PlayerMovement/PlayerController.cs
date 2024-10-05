using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : Controller
{
    PlayerInput playerInput;
    Vector2 moveInput;
    private bool _moving;


    void Start()
    {
        playerInput = new PlayerInput();
        inputHandler = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (_moving)
            HandleMovementInput();
    }

    public override void HandleMovementInput()
    {
        if (inputHandler != null)
        {
            inputHandler.ProcessMovement(moveInput.x, moveInput.y);
        }
    }

    void onMovementInputStarted(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        _moving = true;
    }
    
    void onMovementInput(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }

    void onMovementInputCancelled(InputAction.CallbackContext context)
    {
        _moving = false;
        moveInput = Vector2.zero;
    }

    void OnEnable()
    {
        playerInput.PlayerControl.Enable();

        playerInput.PlayerControl.Move.started += onMovementInputStarted;
        playerInput.PlayerControl.Move.performed += onMovementInput;
        playerInput.PlayerControl.Move.canceled += onMovementInputCancelled;
    }

    void OnDisable()
    {
        playerInput.PlayerControl.Disable();

        playerInput.PlayerControl.Move.started -= onMovementInputStarted;
        playerInput.PlayerControl.Move.performed -= onMovementInput;
        playerInput.PlayerControl.Move.canceled -= onMovementInputCancelled;
    }
}