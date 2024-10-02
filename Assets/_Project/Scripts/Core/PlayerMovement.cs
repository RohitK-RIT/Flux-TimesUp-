using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : InputHandler
{
    public float moveSpeed = 5f;
    //public CharacterController CharacterController;

    public override void ProcessMovement(float horizontal, float vertical)
    {
        Vector3 movement = new Vector3(horizontal, 0, vertical);

        // Move the player
        transform.Translate(movement * moveSpeed * Time.deltaTime, Space.World);
        //CharacterController.

    }
}
