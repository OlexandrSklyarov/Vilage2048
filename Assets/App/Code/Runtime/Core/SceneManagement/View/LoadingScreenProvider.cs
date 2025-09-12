using UnityEngine;
using VContainer;
using VContainer.Unity;
using System.Collections.Generic;
using Assets.App.Code.Runtime.Services.Scenes.Operations;
using Cysharp.Threading.Tasks;
using Assets.App.Code.Runtime.Core.Configs;

namespace Assets.App.Code.Runtime.Services.Scenes.View
{
    public class LoadingScreenProvider : ILoadingScreenProvider
    {
        private readonly AppConfig _appConfig;

        public LoadingScreenProvider(AppConfig appConfig)
        {
            _appConfig = appConfig; 
        }

        public async UniTask LoadAsync(Queue<ILoadingOperation> operations, ScreenType type = ScreenType.SimpleBackground, string showMsg = null)
        {
            var prefab = type switch
            {
                ScreenType.SimpleBackground => _appConfig.UI.SimpleBackgroundScreenPrefab,
                _ => throw new System.ArgumentOutOfRangeException(nameof(type), type, null)
            };

            var loadingScreen = UnityEngine.Object.Instantiate(prefab) as ILoadingScreen;

            loadingScreen.Show();

            var previousPriority = Application.backgroundLoadingPriority;
            
            Application.backgroundLoadingPriority = ThreadPriority.High;

            await loadingScreen.LoadAsync(operations, showMsg);

            loadingScreen.Hide();

            loadingScreen.Reclaim();

            Application.backgroundLoadingPriority = previousPriority;
        }       
    }
}
