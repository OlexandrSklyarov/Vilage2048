using System;
using Assets.App.Code.Runtime.Core.Time;

namespace Assets.App.Code.Runtime.Gameplay.Pause
{
    public sealed class GamePauseService : IPauseService, IDisposable
    {
        public bool IsPause { get; private set; }
        private readonly ITimeService _timeService;

        public event Action<bool> ChangePauseEvent = delegate { };

        public GamePauseService(ITimeService timeService)
        {
            _timeService = timeService;
        }

        public void Enable()
        {
            _timeService.SetTimeScale(0f);
            IsPause = true;

            ChangePauseEvent.Invoke(IsPause);
        }

        public void Disable()
        {
            _timeService.SetTimeScale(1f);
            IsPause = false;
            ChangePauseEvent.Invoke(IsPause);            
        }

        public void Dispose()
        {
            Disable();
        }
    }
}

