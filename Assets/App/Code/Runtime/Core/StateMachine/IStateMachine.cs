using Cysharp.Threading.Tasks;

namespace Assets.App.Code.Runtime.Core.StateMachine
{
    public interface IStateMachine
    {
        UniTask Enter<TState>() where TState : class, IState;
        UniTask Enter<TState, TPayload>(TPayload payload) where TState : class, IPaylodedState<TPayload>;
        void RegisterState<TState>(TState state) where TState : IExitableState;
    }
}