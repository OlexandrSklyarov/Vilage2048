using Assets.App.Code.Runtime.Boot.FSM;
using Assets.App.Code.Runtime.Boot.FSM.States;
using Cysharp.Threading.Tasks;
using UnityEngine;
using VContainer;

namespace Assets.App.Code.Runtime.Boot
{
    public sealed class AppBootstrap : MonoBehaviour
    {
        [Inject]
        private void Construct(ApplicationFSM fsm, IObjectResolver resolver)
        {
            fsm.RegisterState(resolver.Resolve<BootState>());
            fsm.RegisterState(resolver.Resolve<GameLoadingState>());
            fsm.RegisterState(resolver.Resolve<MainMenuState>());
            fsm.RegisterState(resolver.Resolve<GameplayState>());
            fsm.RegisterState(resolver.Resolve<RestartGameState>());
            
            fsm.Enter<BootState>().Forget();
        }
    }
}
