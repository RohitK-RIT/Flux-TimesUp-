using _Project.Scripts.Core.Player_Controllers;
using UnityEngine;

namespace _Project.Scripts.Core.Character
{
    public abstract class CharacterComponent : MonoBehaviour
    {
        public PlayerController PlayerController { get; private set; }

        public virtual void Initialize(PlayerController playerController)
        {
            PlayerController = playerController;
        }
    }
}