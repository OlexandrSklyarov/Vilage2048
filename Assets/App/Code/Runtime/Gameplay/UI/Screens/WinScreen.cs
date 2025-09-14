using Assets.App.Code.Runtime.Core.Signals;
using Cysharp.Threading.Tasks;
using UnityEngine.UIElements;

namespace Assets.App.Code.Runtime.Gameplay.UI.Screens
{
    public sealed class WinScreen : BaseScreen
    {
        private readonly UIScreenFactory _screenFactory;
        private readonly SignalBus _signalBus;
        private Button _continueButton;

        public WinScreen(UIScreenFactory screenFactory, SignalBus signalBus)
        {
            _screenFactory = screenFactory;
            _signalBus = signalBus;
        }

        public override async UniTask InitializeAsync()
        {
            Root = _screenFactory.CreateWinScreen();

            _continueButton = Root.Q<Button>("ContinueButton");

            _continueButton.clicked += OnPressContinue;

            await UniTask.CompletedTask;
        }

        private void OnPressContinue()
        {
            _signalBus.Fire(new Signal.Gameplay.NextLevel());
        }

        public override void Dispose()
        {
            _continueButton.clicked -= OnPressContinue;
        }
    }
}

