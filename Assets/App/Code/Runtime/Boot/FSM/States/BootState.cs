using Assets.App.Code.Runtime.Core.StateMachine;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.App.Code.Runtime.Boot.FSM.States
{
    public sealed class BootState : IState
    {
        private readonly ApplicationFSM _fsm;

        public BootState(ApplicationFSM fsm)
        {
            _fsm = fsm;
        }

        public async UniTask Enter()
        {            
            Application.targetFrameRate = 60;    

            Screen.sleepTimeout = SleepTimeout.NeverSleep;

            Screen.orientation = ScreenOrientation.Portrait;

            _fsm.Enter<GameLoadingState>().Forget();
            
            await UniTask.CompletedTask;          
        }        
      
        public async UniTask Exit() => await UniTask.CompletedTask;
    }
}

