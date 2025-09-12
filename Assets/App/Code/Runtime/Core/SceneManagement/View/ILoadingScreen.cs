using System.Collections.Generic;
using Assets.App.Code.Runtime.Services.Scenes.Operations;
using Cysharp.Threading.Tasks;

namespace Assets.App.Code.Runtime.Services.Scenes.View
{
    public interface ILoadingScreen
    {
        bool IsCompleted {get;}
        void Show();
        void Hide();
        UniTask LoadAsync(Queue<ILoadingOperation> operations, string showMsg);
        void Reclaim();
    }
}

