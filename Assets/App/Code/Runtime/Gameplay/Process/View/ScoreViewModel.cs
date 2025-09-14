using System;
using R3;

namespace Assets.App.Code.Runtime.Gameplay.Process.View
{
    public sealed class ScoreViewModel : IDisposable
    {
        public ReactiveProperty<int> CurrentScore = new(0);

        private readonly GameScoreServices _scoreServices;
        private CompositeDisposable _disposables = new();

        public ScoreViewModel(GameScoreServices scoreServices)
        {
            _scoreServices = scoreServices;
          
            Subscribe();
        }

        private void Subscribe()
        {
            _disposables.Add
            (
                _scoreServices.CurrentScore.Subscribe(score => CurrentScore.Value += score)
            );
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }
    }
}

