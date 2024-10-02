using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Controller : MonoBehaviour
{
    protected InputHandler inputHandler;

    public abstract void HandleInput();

    protected void SetInputHandler(InputHandler handler)
    {
        inputHandler = handler;
    }
    
}
