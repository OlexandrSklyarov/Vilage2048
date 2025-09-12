using System;
using Assets.App.Code.Runtime.Core.Signals;
using Cysharp.Threading.Tasks;
using UnityEngine.UIElements;

namespace Assets.App.Code.Runtime.Gameplay.UI.Screens
{
    public sealed class PauseScreen : BaseHudScreen
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
            Root = _screenFactory.CreateHUDScreen();

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

        public override void Show(VisualElement container)
        {
            base.Show(container);

            //subscribe with added dispose handler            
        }

        public override void Hide()
        {
            Dispose();
            base.Hide();
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

