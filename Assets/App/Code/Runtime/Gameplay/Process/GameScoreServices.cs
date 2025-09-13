namespace Assets.App.Code.Runtime.Gameplay.Process
{
    public sealed class GameScoreServices
    {
        public int CurrentScore { get; private set; } = 0;

        public GameScoreServices()
        {

        }

        public void AddScore(int score)
        {
            CurrentScore += score;
            Util.DebugLog.Print($"AddScore: {score}, CurrentScore: {CurrentScore}");
        }
    }
}

