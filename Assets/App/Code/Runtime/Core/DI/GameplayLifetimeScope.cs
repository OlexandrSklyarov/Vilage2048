using System;
using Assets.App.Code.Runtime.Gameplay.UI;
using Assets.App.Code.Runtime.Gameplay.UI.Screens;
using VContainer;
using VContainer.Unity;

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
            // gameplay fsm
        }

        private void RegistrationServices(IContainerBuilder builder)
        {
            // Register your services here
        }

        private void RegistrationScreens(IContainerBuilder builder)
        {            
            builder.Register<UIScreenFactory>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();

            builder.Register<BaseHudScreen, GameplayScreen>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            builder.Register<BaseHudScreen, PauseScreen>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
        }

        private void RegistrationHierarchy(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<HUDViewController>();
        }
    }
}