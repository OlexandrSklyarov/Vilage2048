using System;
using UnityEngine;

namespace Assets.App.Code.Runtime.Core.Input
{
    public interface IInputService
    {
        InputData InputData { get; }
        Vector2 Direction { get; }
        bool IsPressed { get; }
        bool IsPressDown { get; }
        bool IsPressUp { get; }
        bool IsPressOnUIElement { get; }
        bool IsEnabled { get; }

        event Action<InputData> InputTouchEvent;
        event Action<float> TouchScrollEvent;

        void Enable();
        void Disable();
        InputData GetInputDataOnTouch();
    }
}

