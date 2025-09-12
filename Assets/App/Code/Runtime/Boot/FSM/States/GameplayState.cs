using System;
using System.Collections.Generic;
using Assets.App.Code.Runtime.Core.Signals;
using Assets.App.Code.Runtime.Core.StateMachine;
using Assets.App.Code.Runtime.Services.Scenes.Operations;
using Assets.App.Code.Runtime.Services.Scenes.View;
using Cysharp.Threading.Tasks;

namespace Assets.App.Code.Runtime.Boot.FSM.States
{
    public sealed class GameplayState : IState, IDisposable
    {
        private readonly ApplicationFSM _fsm;
        private readonly LoadingOperationFactory _operationFactory;
        private readonly SignalBus _signalBus;
        private readonly ILoadingScreenProvider _loadingScreenProvider;

        public GameplayState(ApplicationFSM fsm,
            LoadingOperationFactory operationFactory,
            SignalBus signalBus,
            ILoadingScreenProvider loadingScreenProvider)
        {
            _fsm = fsm;
            _operationFactory = operationFactory;
            _signalBus = signalBus;
            _loadingScreenProvider = loadingScreenProvider;
        }

        public async UniTask Enter()
        {
            _signalBus.Subscribe<Signal.App.MainMenu>(OnMeinMenu);
            _signalBus.Subscribe<Signal.App.RestartGame>(OnRestartGame);

            await UniTask.CompletedTask;
        }

        public async UniTask Exit()
        {
            Unsubscribe();
            await UniTask.CompletedTask;
        }
        
        private void Unsubscribe()
        {
            _signalBus.UnSubscribe<Signal.App.MainMenu>(OnMeinMenu);
            _signalBus.UnSubscribe<Signal.App.RestartGame>(OnRestartGame);
        }

        private void OnRestartGame(Signal.App.RestartGame evt) => HandleRestartGameAsync().Forget();       

        private void OnMeinMenu(Signal.App.MainMenu evt) => HandleMainMenuAsync().Forget();

        private async UniTask HandleRestartGameAsync()
        {
            var operations = new Queue<ILoadingOperation>();

            operations.Enqueue(_operationFactory.Create<RestartGameOperation>());

            await _loadingScreenProvider.LoadAsync(operations, ScreenType.SimpleBackground);

            _fsm.Enter<RestartGameState>().Forget();
        }

        private async UniTask HandleMainMenuAsync()
        {
            var operations = new Queue<ILoadingOperation>();

            operations.Enqueue(_operationFactory.Create<LoadMainMenuOperation>());

            await _loadingScreenProvider.LoadAsync(operations);

            _fsm.Enter<MainMenuState>().Forget();
        }


        public void Dispose()
        {
            Unsubscribe();
        }
    }
}

