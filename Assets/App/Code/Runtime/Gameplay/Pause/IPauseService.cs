using System;

namespace Assets.App.Code.Runtime.Gameplay.Pause
{
    public interface IPauseService
    {
        public bool IsPause { get; }

        event Action<bool> ChangePauseEvent;

        public void Enable();
        public void Disable();
    }
}
