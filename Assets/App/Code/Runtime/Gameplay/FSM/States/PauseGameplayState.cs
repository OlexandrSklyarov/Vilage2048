using System;
using Assets.App.Code.Runtime.Core.Signals;
using Assets.App.Code.Runtime.Core.StateMachine;
using Assets.App.Code.Runtime.Gameplay.Pause;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.App.Code.Runtime.Gameplay.FSM.States
{
    public sealed class PauseGameplayState : IState, IDisposable
    {
        private readonly GameplayFSM _stateMachine;
        private readonly SignalBus _signalBus;
        private readonly GamePauseService _gamePauseService;

        public PauseGameplayState(GameplayFSM stateMachine,
            SignalBus signalBus,
            GamePauseService gamePauseService)
        {
            _stateMachine = stateMachine;
            _signalBus = signalBus;
            _gamePauseService = gamePauseService;
        }

        public async UniTask Enter()
        {
            _gamePauseService.EnablePause();

            _signalBus.Subscribe<Signal.Gameplay.ResumePause>(PauseResume);
            _signalBus.Subscribe<Signal.Gameplay.ExitToMainMenu>(ExitToMainMenu);

            _signalBus.Fire(new Signal.ShowGameplayScreen.PauseMenu());

            await UniTask.CompletedTask;
        }

        private void UnSubscribe()
        {
            _signalBus.UnSubscribe<Signal.Gameplay.ResumePause>(PauseResume);
            _signalBus.UnSubscribe<Signal.Gameplay.ExitToMainMenu>(ExitToMainMenu);
        }

        private void ExitToMainMenu(Signal.Gameplay.ExitToMainMenu evt)
        {
            _signalBus.Fire(new Signal.App.MainMenu());
        }

        private void PauseResume(Signal.Gameplay.ResumePause evt)
        {            
            _gamePauseService.Disable();
            _stateMachine.Enter<GameProcessState>().Forget();
        } 

        public async UniTask Exit()
        {            
            UnSubscribe();
            await UniTask.CompletedTask;
        }

        public void Dispose()
        {
            UnSubscribe();
        }
    }
}
