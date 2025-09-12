using System;
using Assets.App.Code.Runtime.Core.SceneManagement;
using Assets.App.Code.Runtime.Services.Extensions;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace Assets.App.Code.Runtime.Services.Scenes.Operations
{
    public sealed class LoadGameplayOperation : ILoadingOperation
    {
        public async UniTask Load(Action<float> onProgressCallback)
        {
            onProgressCallback?.Invoke(0f);

            var loadTasks = new UniTask[]
            {
                SceneExtensions.LoadSceneAsync(SceneConst.MEDIATOR, LoadSceneMode.Single, onProgressCallback),
                SceneExtensions.LoadSceneAsync(SceneConst.GAMEPLAY, LoadSceneMode.Single, onProgressCallback),
                SceneExtensions.LoadSceneAsync(SceneConst.ENVIRONMENT, LoadSceneMode.Additive, onProgressCallback),
            }; 

            await UniTask.WhenAll(loadTasks); 
            await UniTask.Yield();             

            onProgressCallback?.Invoke(1f);
        }

        
    }
}
