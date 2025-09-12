using System.Collections.Generic;
using Assets.App.Code.Runtime.Core.StateMachine;
using Assets.App.Code.Runtime.Services.Scenes.Operations;
using Assets.App.Code.Runtime.Services.Scenes.View;
using Cysharp.Threading.Tasks;

namespace Assets.App.Code.Runtime.Boot.FSM.States
{
    public sealed class RestartGameState : IState
    {
        private readonly ApplicationFSM _fsm;
        private readonly LoadingOperationFactory _operationFactory;
        private readonly ILoadingScreenProvider _loadingScreenProvider;

        public RestartGameState(ApplicationFSM fsm,
            LoadingOperationFactory operationFactory,
            ILoadingScreenProvider loadingScreenProvider)
        {
            _fsm = fsm;
            _operationFactory = operationFactory;
            _loadingScreenProvider = loadingScreenProvider;
        }

        public async UniTask Enter()
        {
            await HandlePlayGameAsync();
        }

        public async UniTask Exit()
        {
            await UniTask.CompletedTask;
        }
        
        private async UniTask HandlePlayGameAsync()
        {
            var operations = new Queue<ILoadingOperation>();

            operations.Enqueue(_operationFactory.Create<LoadGameplayOperation>());

            await _loadingScreenProvider.LoadAsync(operations);

            _fsm.Enter<GameplayState>().Forget();
        }
    }
}

