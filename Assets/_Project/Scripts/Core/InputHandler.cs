using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InputHandler : MonoBehaviour
{
    public abstract void ProcessMovement(float horizontal, float vertical);
}
