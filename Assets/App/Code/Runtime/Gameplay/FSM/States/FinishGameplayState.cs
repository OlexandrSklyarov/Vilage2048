using System;
using Assets.App.Code.Runtime.Core.Signals;
using Assets.App.Code.Runtime.Core.StateMachine;
using Assets.App.Code.Runtime.Gameplay.Map;
using Cysharp.Threading.Tasks;

namespace Assets.App.Code.Runtime.Gameplay.FSM.States
{
    public sealed class FinishGameplayState : IState, IDisposable
    {
        private readonly SignalBus _signalBus;
        private readonly LevelInfoService _levelInfoService;

        public FinishGameplayState(SignalBus signalBus, LevelInfoService levelInfoService)
        {
            _signalBus = signalBus;
            _levelInfoService = levelInfoService;
        }

        public async UniTask Enter()
        {
            _signalBus.Subscribe<Signal.Gameplay.NextLevel>(OnNextLevel);            
            _signalBus.Subscribe<Signal.Gameplay.ExitToMainMenu>(OnMainMenu); 
            _signalBus.Subscribe<Signal.Gameplay.Restart>(OnRestart); 

            await UniTask.CompletedTask;
        }

        private void OnRestart(Signal.Gameplay.Restart evt)
        {
            _signalBus.Fire(new Signal.App.RestartGame());
        }

        private void OnMainMenu(Signal.Gameplay.ExitToMainMenu evt)
        {
            _signalBus.Fire(new Signal.App.MainMenu());
        }

        private void OnNextLevel(Signal.Gameplay.NextLevel evt)
        {
            _levelInfoService.UpdateToNextLevelIndex();

            _signalBus.Fire(new Signal.App.RestartGame());
        }

        public async UniTask Exit()
        {
            Unsubscribe();
            await UniTask.CompletedTask;
        }      

        private void Unsubscribe()
        {
            _signalBus.UnSubscribe<Signal.Gameplay.NextLevel>(OnNextLevel);            
            _signalBus.UnSubscribe<Signal.Gameplay.ExitToMainMenu>(OnMainMenu); 
            _signalBus.UnSubscribe<Signal.Gameplay.Restart>(OnRestart); 
        }

        public void Dispose()
        {
            Unsubscribe();
        }
    }
}

