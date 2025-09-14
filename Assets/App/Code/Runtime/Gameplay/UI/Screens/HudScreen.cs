using System;
using System.Collections.Generic;
using Assets.App.Code.Runtime.Core.Signals;
using Assets.App.Code.Runtime.Gameplay.Process.View;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine.UIElements;

namespace Assets.App.Code.Runtime.Gameplay.UI.Screens
{
    public sealed class HudScreen : BaseScreen
    {
        private readonly UIScreenFactory _screenFactory;
        private readonly SignalBus _signalBus;
        private readonly ScoreViewModel _scoreViewModel;
        private Button _menuButton;
        private Label _scoreLabel;
        private Label _boxCountLabel;
        private CompositeDisposable _disposables;


        public HudScreen(
            UIScreenFactory screenFactory,
            SignalBus signalBus,
            ScoreViewModel scoreViewModel)
        {
            _screenFactory = screenFactory;
            _signalBus = signalBus;
            _scoreViewModel = scoreViewModel;
        }

        public override async UniTask InitializeAsync()
        {
            Root = _screenFactory.CreateHUDScreen();

            _menuButton = Root.Q<Button>("MenuButton");            
            _scoreLabel = Root.Q<Label>("ScoreLabel");            
            _boxCountLabel = Root.Q<Label>("BoxCountLabel");            

            _menuButton.clicked += OnPressMenuButton;

            _disposables = new CompositeDisposable(new List<IDisposable>()
            {
                _scoreViewModel.CurrentScore.Subscribe(s => SetScore(s)),
                _scoreViewModel.BoxCount.Subscribe(s => SetBoxCount(s))
            });
                  
            await UniTask.CompletedTask;
        }
       
        private void SetBoxCount(string countText) => _boxCountLabel.text = countText;
        
        private void SetScore(string scoreText) => _scoreLabel.text = scoreText;

        private void OnPressMenuButton()
        {
            _signalBus.Fire(new Signal.Gameplay.ActivePause());
        }

        public override void Dispose()
        {
            _menuButton.clicked -= OnPressMenuButton;
            _disposables.Dispose();
        }
    }
}

