using System;
using App.Code.Runtime.Gameplay.Process;
using Assets.App.Code.Runtime.Core.Input;
using Assets.App.Code.Runtime.Core.Signals;
using Assets.App.Code.Runtime.Core.StateMachine;
using Cysharp.Threading.Tasks;

namespace Assets.App.Code.Runtime.Gameplay.FSM.States
{
    public sealed class GameProcessState : IState, IDisposable
    {
        private readonly GameplayFSM _stateMachine;
        private readonly SignalBus _signalBus;
        private readonly GameProcessService _gameProcessService;
        private readonly IInputService _inputService;

        public GameProcessState(
            GameplayFSM stateMachine,
            SignalBus signalBus,
            GameProcessService gameProcessService,
            IInputService inputService)
        {
            _stateMachine = stateMachine;
            _signalBus = signalBus;
            _gameProcessService = gameProcessService;
            _inputService = inputService;
        }

        public async UniTask Enter()
        {                    
            _signalBus.Subscribe<Signal.Gameplay.ActivePause>(Pause);
            _signalBus.Subscribe<Signal.Gameplay.Win>(Win);
            _signalBus.Subscribe<Signal.Gameplay.Lose>(Loss);

            _signalBus.Fire(new Signal.ShowGameplayScreen.HUD());

            _inputService.EnableGameplay();

            await UniTask.CompletedTask;
        }

        private void UnSubscribe()
        {
            _signalBus.UnSubscribe<Signal.Gameplay.ActivePause>(Pause);
            _signalBus.UnSubscribe<Signal.Gameplay.Win>(Win);
            _signalBus.UnSubscribe<Signal.Gameplay.Lose>(Loss);           
        }             

        private void Pause(Signal.Gameplay.ActivePause evt)
        {
            _stateMachine.Enter<PauseGameplayState>().Forget();
        }

        private void Loss(Signal.Gameplay.Lose evt)
        {
            _gameProcessService.Stop();
            _stateMachine.Enter<LossGameplayState>().Forget();
        }

        private void Win(Signal.Gameplay.Win evt)
        {           
            _gameProcessService.Stop();
            _stateMachine.Enter<WinGameplayState>().Forget();
        }

        public async UniTask Exit()
        {
            _inputService.EnableUI();
            UnSubscribe();
            await UniTask.CompletedTask;
        }

        public void Dispose()
        {
            UnSubscribe();
        }
    }
}