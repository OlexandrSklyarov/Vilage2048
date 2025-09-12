using System;
using System.Collections.Generic;
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

            await UniTask.WaitForSeconds(0.2f);

            onProgressCallback?.Invoke(1f);
        }                
    }
}
