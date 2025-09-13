using Assets.App.Code.Runtime.Services.Scenes.View;

namespace Assets.App.Code.Runtime.Core.SceneManagement.Factory
{
    public interface ILoadingScreenFactory
    {
        ILoadingScreen Create(ScreenType type = ScreenType.SimpleBackground);
        void Reclaim(ILoadingScreen loadingScreen);
    }
}

