using Assets.App.Code.Runtime.Data.Configs;
using Assets.App.Code.Runtime.Services.Scenes.View;

namespace Assets.App.Code.Runtime.Core.SceneManagement.Factory
{
    public sealed class LoadingScreenFactory : ILoadingScreenFactory
    {
        private readonly AppConfig _appConfig;

        public LoadingScreenFactory(AppConfig appConfig)
        {
            _appConfig = appConfig;
        }        

        public ILoadingScreen Create(ScreenType type = ScreenType.SimpleBackground)
        {
            var prefab = type switch
            {
                ScreenType.SimpleBackground => _appConfig.UI.SimpleBackgroundScreenPrefab,
                _ => throw new System.ArgumentOutOfRangeException(nameof(type), type, null)
            };

            return UnityEngine.Object.Instantiate(prefab);
        }        

        public void Reclaim(ILoadingScreen loadingScreen)
        {
            loadingScreen.Reclaim();
        }
    }
}

