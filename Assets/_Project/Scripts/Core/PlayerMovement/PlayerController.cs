using UnityEngine;
using UnityEngine.InputSystem;

namespace _Project.Scripts.Core.PlayerMovement
{
    public class PlayerController : Controller
    {
        void Start()
        {
            // Get the input handler which now includes all input handling
            InputHandler = GetComponent<CharacterMovement>();
        }

        void Update()
        {
            // Delegate input handling to InputHandler
            if (InputHandler)
            {
                InputHandler.HandleMovementInput();
            }
        }
    }
}