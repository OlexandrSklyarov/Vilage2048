using UnityEngine.UIElements;
using Assets.App.Code.Runtime.Data.Configs;
using UnityEngine;

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

            if (_appConfig.UI.UseSafeArea)
            {
                var multiplier = 1f;

                var safeArea = Screen.safeArea;
                var left = safeArea.x;
                var right = Screen.width - safeArea.xMax;
                var top = Screen.height - safeArea.yMax;
                var bottom = safeArea.y;

                var content = root.Q<VisualElement>("Content");

                content.style.borderBottomWidth = bottom * multiplier;
                content.style.borderTopWidth = top * multiplier;
                content.style.borderLeftWidth = left * multiplier;
                content.style.borderRightWidth = right * multiplier;
            }

            return root;
        }   
    }
}


