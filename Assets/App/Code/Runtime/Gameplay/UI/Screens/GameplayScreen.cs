using Assets.App.Code.Runtime.Core.Signals;
using Cysharp.Threading.Tasks;
using UnityEngine.UIElements;

namespace Assets.App.Code.Runtime.Gameplay.UI.Screens
{
    public sealed class GameplayScreen : BaseHudScreen
    {
        private readonly UIScreenFactory _screenFactory;
        private readonly SignalBus _signalBus;
        private Button _menuButton;

        public GameplayScreen(UIScreenFactory screenFactory, SignalBus signalBus)
        {
            _screenFactory = screenFactory;
            _signalBus = signalBus;
        }

        public override async UniTask InitializeAsync()
        {
            Root = _screenFactory.CreateHUDScreen();

            _menuButton = Root.Q<Button>("MenuButton");

            _menuButton.clicked += OnPressMenuButton;

            await UniTask.CompletedTask;
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
            _signalBus.Fire(new Signal.Gameplay.ActivePause());
        }    

        public override void Dispose()
        {
            _menuButton.clicked -= OnPressMenuButton;
        }

    }
}

