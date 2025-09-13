using Assets.App.Code.Runtime.Gameplay.Box;

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

        public struct ShowGameplayScreen
        {
            public struct HUD { }
            public struct PauseMenu { }
            public struct WinScreen { }
            public struct LoseScreen { }
        }

        public struct Gameplay
        {
            public struct ActivePause { }
            public struct ResumePause { }
            public struct Win { }
            public struct Lose { }
            public struct Restart { }
            public struct NextLevel { }
            public struct ExitToMainMenu { }
        }  

        public struct GameEvent
        {
            public struct BoxCollision
            {
                public (BoxView self, BoxView other, float impulseMagnitude) CollidePair;
            }
        } 
    }
}

