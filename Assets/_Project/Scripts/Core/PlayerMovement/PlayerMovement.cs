using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : InputHandler
{
    public float moveSpeed = 5f;
    CharacterController characterController;
    
    Vector2 currentMovementInput;
    Vector3 currentMovement;
    bool isMovementPressed;

    float rotationFactorPerFrame = 10.0f;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    void handleRotation()
    {
        Vector3 positionToLookAt;
        positionToLookAt.x = currentMovement.x;
        positionToLookAt.y = 0.0f;
        positionToLookAt.z = currentMovement.z;
        Quaternion currentRotation = transform.rotation;
        if (isMovementPressed)
        {
            Quaternion targetRotation = Quaternion.LookRotation(positionToLookAt);
            transform.rotation = Quaternion.Slerp(currentRotation, targetRotation, rotationFactorPerFrame * Time.deltaTime);
        }
        
    }

    public override void ProcessMovement(float horizontal, float vertical)
    {
        currentMovement.x = horizontal;
        currentMovement.z = vertical;
        isMovementPressed = currentMovement != Vector3.zero;

        if (isMovementPressed)
        {
            characterController.Move(currentMovement * moveSpeed * Time.deltaTime);
        }
        
    }

    void Update()
    {
        handleRotation();
    }

}
