using VContainer;

namespace Assets.App.Code.Runtime.Services.Scenes.Operations
{
    public sealed class LoadingOperationFactory
    {
        private readonly IObjectResolver _resolver;

        public LoadingOperationFactory(IObjectResolver resolver)
        {
            _resolver = resolver;
        }

        public T Create<T>() where T : ILoadingOperation
        {
            return _resolver.Resolve<T>();
        }        
    }
}

