using Assets.App.Code.Runtime.Gameplay.FSM;
using Assets.App.Code.Runtime.Gameplay.FSM.States;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace Assets.App.Code.Runtime.Gameplay
{
    public sealed class GameplayBootstrap : MonoBehaviour
    {
        [Inject]
        private void Construct(GameplayFSM fsm, IObjectResolver resolver)
        {
            fsm.RegisterState(resolver.Resolve<StartGameplayState>());
            fsm.RegisterState(resolver.Resolve<GameProcessState>());
            fsm.RegisterState(resolver.Resolve<WinGameplayState>());
            fsm.RegisterState(resolver.Resolve<LossGameplayState>());
            fsm.RegisterState(resolver.Resolve<PauseGameplayState>());
            fsm.RegisterState(resolver.Resolve<FinishGameplayState>());
            
            fsm.Enter<StartGameplayState>().Forget();
        }
    }
}

