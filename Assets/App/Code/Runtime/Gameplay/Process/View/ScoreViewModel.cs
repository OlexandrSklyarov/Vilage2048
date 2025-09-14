using System;
using System.Collections.Generic;
using R3;

namespace Assets.App.Code.Runtime.Gameplay.Process.View
{
    public sealed class ScoreViewModel : IDisposable
    {
        public ReactiveProperty<string> CurrentScore = new();
        public ReactiveProperty<string> BoxCount = new();

        private readonly GameScoreServices _scoreServices;
        private readonly CheckGameResultService _checkGameResultService;
        private CompositeDisposable _disposables = new();

        public ScoreViewModel(GameScoreServices scoreServices, CheckGameResultService checkGameResultService)
        {
            _scoreServices = scoreServices;
            _checkGameResultService = checkGameResultService;
          
            Subscribe();
        }

        private void Subscribe()
        {
            _disposables = new CompositeDisposable(new List<IDisposable>()
            {
                _scoreServices.CurrentScore.Subscribe(x => CurrentScore.Value = $"Score: {x}"),
                _checkGameResultService.AliveBoxCount.Subscribe(x => BoxCount.Value = $"Box: {x}")
            });
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}

