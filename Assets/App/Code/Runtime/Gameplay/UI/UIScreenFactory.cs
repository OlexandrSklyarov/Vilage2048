using UnityEngine.UIElements;
using Assets.App.Code.Runtime.Data.Configs;

namespace Assets.App.Code.Runtime.Gameplay.UI
{
    public sealed class UIScreenFactory
    {
        private readonly AppConfig _appConfig;

        public UIScreenFactory(AppConfig appConfig)
        {
            _appConfig = appConfig;
        }         

        public VisualElement CreateHUDScreen()
        {
            return Create
            (
                _appConfig.UI.HUDScreenRef,
                _appConfig.UI.GameMenuStylesRef
            );
        }

        public VisualElement CreatePauseScreen()
        {
            return Create
            (
                _appConfig.UI.PauseScreenRef,
                _appConfig.UI.GameMenuStylesRef
            );
        }

        public VisualElement CreateWinScreen()
        {
            return Create
            (
                _appConfig.UI.WinScreenRef,
                _appConfig.UI.GameMenuStylesRef
            );
        }
        
        public VisualElement CreateGameOverScreen()
        {
            return Create
            (
                _appConfig.UI.GameOverScreenRef,
                _appConfig.UI.GameMenuStylesRef
            );
        }

        private VisualElement Create(VisualTreeAsset screenAsset, StyleSheet styles, string rootName = "Background")
        {
            var root = screenAsset.Instantiate();
            root.style.flexGrow = 1;
            root.styleSheets.Add(styles);

            return root;
        }   
    }
}


