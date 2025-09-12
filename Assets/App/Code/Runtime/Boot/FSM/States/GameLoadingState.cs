using System.Collections.Generic;
using Assets.App.Code.Runtime.Core.StateMachine;
using Assets.App.Code.Runtime.Services.Scenes.Operations;
using Assets.App.Code.Runtime.Services.Scenes.View;
using Cysharp.Threading.Tasks;

namespace Assets.App.Code.Runtime.Boot.FSM.States
{
    public sealed class GameLoadingState : IState
    {
        private readonly ApplicationFSM _fsm;
        private readonly LoadingOperationFactory _operationFactory;
        private readonly ILoadingScreenProvider _loadingScreenProvider;

        public GameLoadingState(ApplicationFSM fsm,
            LoadingOperationFactory operationFactory,
            ILoadingScreenProvider loadingScreenProvider)
        {
            _fsm = fsm;
            _operationFactory = operationFactory;
            _loadingScreenProvider = loadingScreenProvider;
        }

        public async UniTask Enter()
        {
            Util.DebugLog.PrintCyan("GameLoadingState");
            await LoadGameAsync();
        }

        public async UniTask Exit() => await UniTask.CompletedTask;
        
        private async UniTask LoadGameAsync()
        {
            var operations = new Queue<ILoadingOperation>();

            operations.Enqueue(_operationFactory.Create<InitializeGlobalServicesOperation>());
            operations.Enqueue(_operationFactory.Create<LoadPlayerProgressOperation>());
            operations.Enqueue(_operationFactory.Create<LoadMainMenuOperation>());

            await _loadingScreenProvider.LoadAsync(operations, showMsg: "HELLO");

            _fsm.Enter<MainMenuState>().Forget();
        }   
    }
}

