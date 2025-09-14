using VContainer;
using VContainer.Unity;
using App.Code.Runtime.Gameplay.Process;
using Assets.App.Code.Runtime.Core.Input;
using Assets.App.Code.Runtime.Gameplay;
using Assets.App.Code.Runtime.Gameplay.Box;
using Assets.App.Code.Runtime.Gameplay.FSM;
using Assets.App.Code.Runtime.Gameplay.FSM.States;
using Assets.App.Code.Runtime.Gameplay.Map;
using Assets.App.Code.Runtime.Gameplay.Pause;
using Assets.App.Code.Runtime.Gameplay.UI;
using Assets.App.Code.Runtime.Gameplay.UI.Screens;
using Assets.App.Code.Runtime.Gameplay.Process;
using Assets.App.Code.Runtime.Gameplay.Process.View;

namespace Assets.App.Code.Runtime.Core.DI
{
    public class GameplayLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            RegistrationHierarchy(builder);
            RegistrationScreens(builder);
            RegistrationServices(builder);
            RegistrationStates(builder);
        }

        private void RegistrationStates(IContainerBuilder builder)
        {
            builder.Register<GameplayFSM>(Lifetime.Singleton);

            builder.Register<StartGameplayState>(Lifetime.Scoped);
            builder.Register<GameProcessState>(Lifetime.Scoped);
            builder.Register<WinGameplayState>(Lifetime.Scoped);
            builder.Register<LossGameplayState>(Lifetime.Scoped);
            builder.Register<PauseGameplayState>(Lifetime.Scoped);
            builder.Register<FinishGameplayState>(Lifetime.Scoped);
        }

        private void RegistrationServices(IContainerBuilder builder)
        {
            builder.Register<LevelInfoService>(Lifetime.Singleton);

            builder.Register<BuildGameLocationService>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();

            builder.Register<GamePauseService>(Lifetime.Singleton);

            builder.Register<GameProcessService>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();  

            builder.Register<TouchInputService>(Lifetime.Singleton).AsImplementedInterfaces();

            builder.Register<BoxCollisionHandleService>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();

            builder.Register<GameScoreServices>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();

            builder.Register<ScoreViewModel>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();

            builder.Register<BoxFactory>(Lifetime.Transient);            
        }

        private void RegistrationScreens(IContainerBuilder builder)
        {            
            builder.Register<UIScreenFactory>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();

            builder.Register<BaseScreen, HudScreen>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            builder.Register<BaseScreen, PauseScreen>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            builder.Register<BaseScreen, WinScreen>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            builder.Register<BaseScreen, GameOverScreen>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
        }

        private void RegistrationHierarchy(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<GameplayBootstrap>();
            
            builder.RegisterComponentInHierarchy<GameScreenController>().AsImplementedInterfaces();
        }
    }
}