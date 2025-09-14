using Assets.App.Code.Runtime.Core.Input;
using Assets.App.Code.Runtime.Core.Signals;
using Assets.App.Code.Runtime.Core.StateMachine;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.App.Code.Runtime.Gameplay.FSM.States
{
    public sealed class LossGameplayState : IState
    {
        private readonly GameplayFSM _stateMachine;
        private readonly SignalBus _signalBus;
        private readonly IInputService _inputService;

        public LossGameplayState(
            GameplayFSM stateMachine,
            SignalBus signalBus,
            IInputService inputService)
        {
            _stateMachine = stateMachine;
            _signalBus = signalBus;
            _inputService = inputService;
        }

        public async UniTask Enter()
        {
            DisablePlayerInput();

            await UniTask.WaitForSeconds(3f);

            ShowGameOverScreen();
            ToFinishState();

            await UniTask.CompletedTask;
        }        

        public async UniTask Exit() =>  await UniTask.CompletedTask;

        private void ShowGameOverScreen() =>_signalBus.Fire(new Signal.ShowGameplayScreen.GameOverScreen());

        private void DisablePlayerInput() => _inputService.Disable();                   

        private void ToFinishState() => _stateMachine.Enter<FinishGameplayState>().Forget();
    }
}
