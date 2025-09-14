using Assets.App.Code.Runtime.Core.Signals;
using Cysharp.Threading.Tasks;
using UnityEngine.UIElements;

namespace Assets.App.Code.Runtime.Gameplay.UI.Screens
{
    public class GameOverScreen : BaseScreen
    {
        private readonly UIScreenFactory _screenFactory;
        private readonly SignalBus _signalBus;
        private Button _restartButton;

        public GameOverScreen(UIScreenFactory screenFactory, SignalBus signalBus)
        {
            _screenFactory = screenFactory;
            _signalBus = signalBus;
        }

        public override async UniTask InitializeAsync()
        {
            Root = _screenFactory.CreateGameOverScreen();

            _restartButton = Root.Q<Button>("RestartButton");

            _restartButton.clicked += OnPressRestart;

            await UniTask.CompletedTask;
        }               

        private void OnPressRestart()
        {
            _signalBus.Fire(new Signal.Gameplay.Restart());
        }

        public override void Dispose()
        {
            _restartButton.clicked -= OnPressRestart;
        }
    }
}

