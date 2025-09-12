using Cysharp.Threading.Tasks;

namespace Assets.App.Code.Runtime.Core.StateMachine
{
    public interface IPaylodedState<TPayload> : IExitableState
    {
        UniTask Enter(TPayload payload);
    }
}