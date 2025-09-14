using System;
using Cysharp.Threading.Tasks;

namespace Assets.App.Code.Runtime.Services.Scenes.Operations
{
    public sealed class LoadPlayerProgressOperation : ILoadingOperation
    {
        public LoadPlayerProgressOperation()
        {
        }

        public async UniTask Load(Action<float> onProgressCallback)
        {
            onProgressCallback?.Invoke(0f);

            await UniTask.Delay(100);

            onProgressCallback?.Invoke(1f);
        }                
    }
}
