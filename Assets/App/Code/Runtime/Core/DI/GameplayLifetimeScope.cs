using Assets.App.Code.Runtime.Gameplay.UI;
using VContainer;
using VContainer.Unity;

namespace Assets.App.Code.Runtime.Core.DI
{
    public class GameplayLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            RegistrationHierarchy(builder);
        }

        private void RegistrationHierarchy(IContainerBuilder builder)
        {
            builder.RegisterComponentInHierarchy<HUDViewController>();
        }
    }
}