using System;
using Assets.App.Code.Runtime.Core.Signals;
using Assets.App.Code.Runtime.Data.Configs;
using Assets.App.Code.Runtime.Gameplay.Map;
using R3;

namespace Assets.App.Code.Runtime.Gameplay.Process
{
    public sealed class CheckGameResultService : IDisposable
    {
        public ReactiveProperty<int> AliveBoxCount { get; private set; } = new(0);

        private readonly AppConfig _appConfig;
        private readonly SignalBus _signalBus;
        private readonly LevelInfoService _levelInfoService;
        private bool _isGameEnd;

        public CheckGameResultService(
            AppConfig appConfig,
            SignalBus signalBus,
            LevelInfoService levelInfoService)
        {
            _appConfig = appConfig;
            _signalBus = signalBus;
            _levelInfoService = levelInfoService;

            _signalBus.Subscribe<Signal.GameEvent.CreateBox>(OnCreateBox);
            _signalBus.Subscribe<Signal.GameEvent.ReleaseBox>(OnReleaseBox);
        }

        private void UnSubscribe()
        {
            _signalBus.UnSubscribe<Signal.GameEvent.CreateBox>(OnCreateBox);
            _signalBus.UnSubscribe<Signal.GameEvent.ReleaseBox>(OnReleaseBox);
        }

        public void Dispose() => UnSubscribe();

        private void OnReleaseBox(Signal.GameEvent.ReleaseBox box) => AliveBoxCount.Value--;

        private void OnCreateBox(Signal.GameEvent.CreateBox box)
        {
            AliveBoxCount.Value++;

            if (_isGameEnd) return;
           
            if (box.Number >= GetMaxNumber())
            {
                UnSubscribe();
                _isGameEnd = true;
                _signalBus.Fire(new Signal.Gameplay.Win());
            }
            else if (AliveBoxCount.Value > GetMaxBoxOnMap())
            {
                UnSubscribe();
                _isGameEnd = true;
                _signalBus.Fire(new Signal.Gameplay.Lose());
            }
        }

        private int GetMaxBoxOnMap()
        {
            var indexLvl = _levelInfoService.GetCurrentLevelIndex();
            return _appConfig.Maps[indexLvl].MaxBoxOnMap;
        }

        private int GetMaxNumber()
        {
            var indexLvl = _levelInfoService.GetCurrentLevelIndex();
            return _appConfig.Maps[indexLvl].MaxNumberToWin;
        }
    }
}

