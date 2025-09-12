using Assets.App.Code.Runtime.MainMenu;
using VContainer;
using VContainer.Unity;

namespace Assets.App.Code.Runtime.Core.DI
{
    public class MainMenuLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            RegistrationHierarchy(builder);
        }

        private void RegistrationHierarchy(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<MainMenuViewController>();
        }
    }
}