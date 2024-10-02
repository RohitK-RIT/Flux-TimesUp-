using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Controller
{
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
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        if (inputHandler != null)
        {
            inputHandler.ProcessMovement(moveHorizontal, moveVertical);
            
        }
    }
}
