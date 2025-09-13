using System.Collections.Generic;
using App.Code.Runtime.Gameplay.Process;
using Assets.App.Code.Runtime.Core.Initializable;
using Assets.App.Code.Runtime.Core.StateMachine;
using Assets.App.Code.Runtime.Gameplay.Map;
using Assets.App.Code.Runtime.Services.Scenes.Operations;
using Assets.App.Code.Runtime.Services.Scenes.View;
using Cysharp.Threading.Tasks;

namespace Assets.App.Code.Runtime.Gameplay.FSM.States
{
    public sealed class StartGameplayState : IState
    {
        private readonly GameplayFSM _stateMachine;
        private readonly BuildGameLocationService _buildGameLocationService;
        private readonly GameProcessService _gameProcess;
        private readonly ILoadingScreenProvider _loadingScreenProvider;
        private readonly IEnumerable<IAsyncPreInitializeProcess> _preInitProcesses;
        private readonly IEnumerable<IAsyncInitializeProcess> _initProcesses;
        private readonly IEnumerable<IAsyncPostInitializeProcess> _postInitProcesses;

        public StartGameplayState(
            IEnumerable<IAsyncPreInitializeProcess> preInitProcesses,
            IEnumerable<IAsyncInitializeProcess> initProcesses,
            IEnumerable<IAsyncPostInitializeProcess> postInitProcesses,
            ILoadingScreenProvider loadingScreenProvider,
            GameplayFSM stateMachine,
            BuildGameLocationService buildGameLocationService,
            GameProcessService gameProcess)
        {
            _preInitProcesses = preInitProcesses;
            _initProcesses = initProcesses;
            _postInitProcesses = postInitProcesses;
            _loadingScreenProvider = loadingScreenProvider;
            _stateMachine = stateMachine;
            _buildGameLocationService = buildGameLocationService;
            _gameProcess = gameProcess;
        }

        public async UniTask Enter()
        {
            foreach (var process in _preInitProcesses) await process.PreInitializeAsync();
            foreach (var process in _initProcesses) await process.InitializeAsync();
            foreach (var process in _postInitProcesses) await process.PostInitializeAsync();

            RunGameplayAsync().Forget();

            await UniTask.CompletedTask;
        }               

        public async UniTask Exit()
        {
            await UniTask.CompletedTask;
        }             
        
        private async UniTask RunGameplayAsync()
        {
            var operations = new Queue<ILoadingOperation>();

            operations.Enqueue
            (
                new SimpleTaskGroupOperation(new List<UniTask>()
                {
                    _buildGameLocationService.CreateLocationAsync(),
                    UniTask.WaitForSeconds(1f)
                })
            );

            await _loadingScreenProvider.LoadAsync(operations, ScreenType.SimpleBackground);

            _gameProcess.Run();

            await _stateMachine.Enter<GameProcessState>();
        }  
    }
}

