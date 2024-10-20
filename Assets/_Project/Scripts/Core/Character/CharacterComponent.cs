using _Project.Scripts.Core.Player_Controllers;
using UnityEngine;

namespace _Project.Scripts.Core.Character
{
    public class CharacterComponent : MonoBehaviour
    {
        public PlayerController PlayerController { get; private set; }

        public void Initialize(PlayerController playerController)
        {
            PlayerController = playerController;
        }
    }
}