using _Project.Scripts.Core.PlayerMovement;
using UnityEngine;

namespace _Project.Scripts.Core
{
    public abstract class Controller : MonoBehaviour
    {
        // Reference to the InputHandler which will manage and process the player's input.
        protected PlayerInputHandler InputHandler;
        
    }
}
