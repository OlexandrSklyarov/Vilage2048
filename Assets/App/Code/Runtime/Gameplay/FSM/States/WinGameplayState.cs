using Assets.App.Code.Runtime.Core.Input;
using Assets.App.Code.Runtime.Core.Signals;
using Assets.App.Code.Runtime.Core.StateMachine;
using Cysharp.Threading.Tasks;

namespace Assets.App.Code.Runtime.Gameplay.FSM.States
{
    public sealed class WinGameplayState : IState
    {
        private readonly GameplayFSM _stateMachine;
        private readonly SignalBus _signalBus;
        private readonly IInputService _inputService;

        public WinGameplayState(
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

            ShowWin();

            ToFinishState();
        }

        public async UniTask Exit() => await UniTask.CompletedTask;

        private void DisablePlayerInput() => _inputService.Disable();

        private void ShowWin() => _signalBus.Fire(new Signal.ShowGameplayScreen.WinScreen());

        private void ToFinishState() => _stateMachine.Enter<FinishGameplayState>().Forget();
    }
}