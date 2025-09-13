using UnityEngine;
using System.Collections.Generic;
using Assets.App.Code.Runtime.Services.Scenes.Operations;
using Cysharp.Threading.Tasks;
using Assets.App.Code.Runtime.Core.SceneManagement.Factory;

namespace Assets.App.Code.Runtime.Services.Scenes.View
{
    public class LoadingScreenProvider : ILoadingScreenProvider
    {
        private readonly ILoadingScreenFactory _factory;

        public LoadingScreenProvider(ILoadingScreenFactory factory)
        {
            _factory = factory; 
        }

        public async UniTask LoadAsync(Queue<ILoadingOperation> operations, ScreenType type = ScreenType.SimpleBackground, string showMsg = null)
        {     
            var loadingScreen = _factory.Create(type);           

            loadingScreen.Show();

            var previousPriority = Application.backgroundLoadingPriority;
            
            Application.backgroundLoadingPriority = ThreadPriority.High;

            await loadingScreen.LoadAsync(operations, showMsg);

            loadingScreen.Hide();            

            _factory.Reclaim(loadingScreen);

            Application.backgroundLoadingPriority = previousPriority;
        }       
    }
}
