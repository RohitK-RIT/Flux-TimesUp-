using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : Controller
{
    PlayerInput playerInput;
    Vector2 moveInput;
    
    void Awake()
    {
        playerInput = new PlayerInput();
        playerInput.PlayerControl.Move.started += onMovementInput;
        playerInput.PlayerControl.Move.canceled += onMovementInput;
    }
    void Start()
    {
        inputHandler = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        HandleInput();
    }

    public override void HandleInput()
    {
        if (inputHandler != null)
        {
            inputHandler.ProcessMovement(moveInput.x, moveInput.y);
        }
    }
    
    void onMovementInput(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
    
    void OnEnable()
    {
        playerInput.PlayerControl.Enable();
    }

    void OnDisable()
    {
        playerInput.PlayerControl.Disable();
    }
}
