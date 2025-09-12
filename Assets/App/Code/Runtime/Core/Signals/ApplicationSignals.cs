namespace Assets.App.Code.Runtime.Core.Signals
{
    public struct Signal
    {
        public struct App
        {
            public struct MainMenu { }
            public struct PlayGame { }
            public struct RestartGame { }
        }

        public struct Gameplay
        {
            public struct ActivePause { }
            public struct ResumePause { }
        }  
    }
}

