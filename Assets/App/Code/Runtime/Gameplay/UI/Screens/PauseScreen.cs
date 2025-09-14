using System;
using Assets.App.Code.Runtime.Core.Signals;
using Cysharp.Threading.Tasks;
using UnityEngine.UIElements;

namespace Assets.App.Code.Runtime.Gameplay.UI.Screens
{
    public sealed class PauseScreen : BaseScreen
    {
        private readonly UIScreenFactory _screenFactory;
        private readonly SignalBus _signalBus;
        private Button _menuButton;
        private Button _closeButton;

        public PauseScreen(UIScreenFactory screenFactory, SignalBus signalBus)
        {
            _screenFactory = screenFactory;
            _signalBus = signalBus;
        }

        public override async UniTask InitializeAsync()
        {
            Root = _screenFactory.CreatePauseScreen();

            _menuButton = Root.Q<Button>("MenuButton");
            _closeButton = Root.Q<Button>("CloseButton");

            _menuButton.clicked += OnPressMenuButton;
            _closeButton.clicked += OnPressCloseButton;

            await UniTask.CompletedTask;
        }       

        public override void Dispose()
        {
            _menuButton.clicked -= OnPressMenuButton;
            _closeButton.clicked -= OnPressCloseButton;
        }              

        private void OnPressMenuButton()
        {
            _signalBus.Fire(new Signal.App.MainMenu());
        }  
        
        private void OnPressCloseButton()
        {
            _signalBus.Fire(new Signal.Gameplay.ResumePause());
        }
    }
}

