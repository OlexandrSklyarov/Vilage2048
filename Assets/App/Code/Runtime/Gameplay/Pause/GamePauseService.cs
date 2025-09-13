using System;
using Assets.App.Code.Runtime.Core.Time;

namespace Assets.App.Code.Runtime.Gameplay.Pause
{
    public sealed class GamePauseService : IDisposable
    {
        public bool IsPause { get; private set; }
        private readonly ITimeService _timeService;


        public GamePauseService(ITimeService timeService)
        {
            _timeService = timeService;
        }

        public void EnablePause()
        {
            _timeService.SetTimeScale(0f);
            IsPause = true;
        }
    
        public void Disable()
        {
            _timeService.SetTimeScale(1f);
            IsPause = false;
        }

        public void Dispose()
        {
            Disable();
        }
    }
}

