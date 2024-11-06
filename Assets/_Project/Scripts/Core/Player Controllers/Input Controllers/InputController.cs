﻿using System;
using _Project.Scripts.Core.Character;
using UnityEngine;

namespace _Project.Scripts.Core.Player_Controllers.Input_Controllers
{
    public abstract class InputController : CharacterComponent
    {
        public abstract event Action<Vector2> OnMoveInputUpdated;
        public abstract event Action OnAttackInputBegan, OnAttackInputEnded;
    }
}