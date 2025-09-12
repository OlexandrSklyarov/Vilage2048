using System;
using Cysharp.Threading.Tasks;
using UnityEngine.SceneManagement;
using Assets.App.Code.Runtime.Services.Extensions;
using Assets.App.Code.Runtime.Core.SceneManagement;

namespace Assets.App.Code.Runtime.Services.Scenes.Operations
{
    public sealed class LoadMainMenuOperation : ILoadingOperation
    {
        public async UniTask Load(Action<float> onProgressCallback)
        {
            onProgressCallback?.Invoke(0.2f);   

            await UniTask.Yield();

            onProgressCallback?.Invoke(0.3f);

            var loadTasks = new UniTask[]
            {
                SceneExtensions.LoadSceneAsync(SceneConst.MEDIATOR, LoadSceneMode.Single, onProgressCallback),
                SceneExtensions.LoadSceneAsync(SceneConst.MAIN_MENU, LoadSceneMode.Single, onProgressCallback)
            };

            await UniTask.WhenAll(loadTasks);            

            onProgressCallback?.Invoke(1f);
        }        
    }
}
