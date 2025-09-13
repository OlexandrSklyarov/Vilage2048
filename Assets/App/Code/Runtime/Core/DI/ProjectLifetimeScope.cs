using VContainer;
using VContainer.Unity;
using UnityEngine;
using Assets.App.Code.Runtime.Data.Configs;
using Assets.App.Code.Runtime.Services.Scenes.Operations;
using Assets.App.Code.Runtime.Boot.FSM;
using Assets.App.Code.Runtime.Boot.FSM.States;
using Assets.App.Code.Runtime.Services.Scenes.View;
using Assets.App.Code.Runtime.Core.Signals;
using Assets.App.Code.Runtime.Core.SceneManagement.Factory;
using Assets.App.Code.Runtime.Core.Time;

namespace Assets.App.Code.Runtime.Core.DI
{
    public class ProjectLifetimeScope : LifetimeScope
    {
        [SerializeField] private AppConfig _appConfig;

        protected override void Configure(IContainerBuilder builder)
        {
            RegisterConfigs(builder);
            RegisterLoadingOperations(builder);
            RegisterStates(builder);
            RegisterLoadingOperations(builder);
            RegisterServices(builder);
        }

        private void RegisterConfigs(IContainerBuilder builder)
        {
            builder.RegisterInstance<AppConfig>(_appConfig);
        }

        private void RegisterLoadingOperations(IContainerBuilder builder)
        {
            builder.Register<LoadingOperationFactory>(Lifetime.Transient);

            builder.Register<InitializeGlobalServicesOperation>(Lifetime.Transient);
            builder.Register<LoadPlayerProgressOperation>(Lifetime.Transient);
            builder.Register<LoadMainMenuOperation>(Lifetime.Transient);
            builder.Register<LoadGameplayOperation>(Lifetime.Transient);
            builder.Register<RestartGameOperation>(Lifetime.Transient);
        }
        
        private void RegisterStates(IContainerBuilder builder)
        {
            builder.Register<ApplicationFSM>(Lifetime.Singleton);

            builder.Register<BootState>(Lifetime.Transient);
            builder.Register<GameLoadingState>(Lifetime.Transient);
            builder.Register<MainMenuState>(Lifetime.Transient);
            builder.Register<GameplayState>(Lifetime.Transient);
            builder.Register<RestartGameState>(Lifetime.Transient);
        }

        private void RegisterServices(IContainerBuilder builder)
        {
            builder.Register<SignalBus>(Lifetime.Singleton).AsSelf().AsImplementedInterfaces();

            builder.Register<LoadingScreenProvider>(Lifetime.Singleton).AsImplementedInterfaces();

            builder.Register<LoadingScreenFactory>(Lifetime.Singleton).AsImplementedInterfaces();

            builder.Register<GameTimeService>(Lifetime.Singleton).AsImplementedInterfaces();         
        }
    }
}
