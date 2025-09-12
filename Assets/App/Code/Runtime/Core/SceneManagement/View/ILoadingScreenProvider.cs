using System.Collections.Generic;
using Assets.App.Code.Runtime.Services.Scenes.Operations;
using Cysharp.Threading.Tasks;

namespace Assets.App.Code.Runtime.Services.Scenes.View
{
    public interface ILoadingScreenProvider
    {
        UniTask LoadAsync(Queue<ILoadingOperation> operations, ScreenType type = ScreenType.SimpleBackground, string showMsg = null);
    }
}
