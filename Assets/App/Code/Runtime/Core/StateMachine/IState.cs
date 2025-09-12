using Cysharp.Threading.Tasks;

namespace Assets.App.Code.Runtime.Core.StateMachine
{
    public interface IState : IExitableState
    {
        UniTask Enter();
    }
}