using UnityEngine;
using VContainer;

namespace Assets.App.Code.Runtime.Core.ObjectPool
{
    public sealed class GeneralPoolFactory
    {
        private readonly IObjectResolver _resolver;

        public GeneralPoolFactory(IObjectResolver resolver)
        {
            _resolver = resolver;
        }

        public GeneralPool<T> Create<T>(string poolName, T prefab, int initCount) where T : Component
        {
            return new GeneralPool<T>(poolName, prefab, initCount, _resolver);
        }               
    }
}

