using System;
using UnityEngine;

namespace Assets.App.Code.Runtime.Core.Input
{
    public interface IInputService
    {
        InputData Data { get; }
        Vector2 PointerPosition { get; }
        bool IsPressDown { get; }
        bool IsPressed { get; }
        bool IsPressUp { get; }

        event Action<InputData> InputUpdateEvent;
        event Action<Vector2> SwipeEvent;

        void EnableGameplay();
        void EnableUI();
    }
}

