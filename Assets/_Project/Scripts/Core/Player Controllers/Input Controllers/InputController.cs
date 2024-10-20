using System;
using UnityEngine;

namespace _Project.Scripts.Core.Player_Controllers.Input_Controllers
{
    public abstract class InputController : MonoBehaviour
    {
        public abstract event Action<Vector2> OnMoveInputUpdated, OnLookInputUpdated;
        public abstract event Action OnAttackInputBegan, OnAttackInputEnded;
    }
}