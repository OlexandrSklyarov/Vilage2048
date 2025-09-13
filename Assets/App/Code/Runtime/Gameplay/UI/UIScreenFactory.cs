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

        public VisualElement CreateGameResultScreen()
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

        private VisualElement Create(VisualTreeAsset screenAsset, StyleSheet styles, string rootName = "root")
        {
            var root = screenAsset.Instantiate().Q(rootName);
            root.styleSheets.Add(styles);

            return root;
        }   
    }
}


