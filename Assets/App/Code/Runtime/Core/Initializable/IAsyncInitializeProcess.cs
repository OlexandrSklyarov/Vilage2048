using Cysharp.Threading.Tasks;

namespace Assets.App.Code.Runtime.Core.Initializable
{
    public interface IAsyncInitializeProcess
    {
        UniTask InitializeAsync();
    }
}
