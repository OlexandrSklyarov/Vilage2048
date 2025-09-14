using R3;

namespace Assets.App.Code.Runtime.Gameplay.Process
{
    public sealed class GameScoreServices
    {
        public ReactiveProperty<int> CurrentScore = new(0);     

        public void AddScore(int score)
        {
            CurrentScore.Value += score;
        }
    }
}

