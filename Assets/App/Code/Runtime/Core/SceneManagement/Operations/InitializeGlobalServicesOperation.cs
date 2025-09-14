using System;
using Cysharp.Threading.Tasks;

namespace Assets.App.Code.Runtime.Services.Scenes.Operations
{
    public sealed class InitializeGlobalServicesOperation : ILoadingOperation
    {

        public InitializeGlobalServicesOperation
        (
            //some global services can be initialized here
        )
        {
        }

        public async UniTask Load(Action<float> onProgressCallback)
        {
            onProgressCallback?.Invoke(0f);

            //Simulate some loading time...
            await UniTask.Delay(100);

            onProgressCallback?.Invoke(1f);
        }                
    }
}
