using System.Collections.Generic;
using Assets.App.Code.Runtime.Core.Signals;
using Assets.App.Code.Runtime.Core.StateMachine;
using Assets.App.Code.Runtime.Services.Scenes.Operations;
using Assets.App.Code.Runtime.Services.Scenes.View;
using Cysharp.Threading.Tasks;

namespace Assets.App.Code.Runtime.Boot.FSM.States
{
    public sealed class MainMenuState : IState
    {
        private readonly ApplicationFSM _fsm;
        private readonly LoadingOperationFactory _operationFactory;
        private readonly SignalBus _signalBus;
        private readonly ILoadingScreenProvider _loadingScreenProvider;

        public MainMenuState(ApplicationFSM fsm,
            LoadingOperationFactory operationFactory,
            SignalBus signalBus,
            ILoadingScreenProvider loadingScreenProvider)
        {
            _fsm = fsm;
            _operationFactory = operationFactory;
            _loadingScreenProvider = loadingScreenProvider;
            _signalBus = signalBus;
        }

        public async UniTask Enter()
        {
            _signalBus.Subscribe<Signal.App.PlayGame>(OnPlayGame);

            await UniTask.CompletedTask;        
        }  

        private void OnPlayGame(Signal.App.PlayGame signal)
        {                   
            _signalBus.UnSubscribe<Signal.App.PlayGame>(OnPlayGame);

            HandlePlayGameAsync().Forget();
        }

        private async UniTask HandlePlayGameAsync()
        {
            var operations = new Queue<ILoadingOperation>();

            operations.Enqueue(_operationFactory.Create<LoadGameplayOperation>());

            await _loadingScreenProvider.LoadAsync(operations);

            _fsm.Enter<GameplayState>().Forget();
        }

        public async UniTask Exit()
        {      
            await UniTask.CompletedTask;
        }
    }
}

