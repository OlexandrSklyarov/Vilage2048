using System.Collections.Generic;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Assets.App.Code.Runtime.Core.ObjectPool
{
    public sealed class GeneralPool<T> where T : Component
    {
        private readonly Transform _poolParent;
        private readonly Queue<T> _queue;
        private readonly T _prefab;
        private readonly IObjectResolver _resolver;

        public GeneralPool(string poolName, T prefab, int initCount)
        {
            _poolParent = new GameObject(poolName).transform;
            _queue = new Queue<T>();
            _prefab = prefab;

            InitItems(initCount);
        }

        public GeneralPool(string poolName, T prefab, int initCount, IObjectResolver resolver)
        {
            _poolParent = new GameObject(poolName).transform;
            _queue = new Queue<T>();
            _prefab = prefab;
            _resolver = resolver;

            InitItems(initCount);
        }

        private void InitItems(int initCount)
        {
            for(int i = 0; i < initCount; i++)
            {
                var item = CreateNew(Vector3.zero, _prefab);
                item.gameObject.SetActive(false);
                _queue.Enqueue(item);
            }
        }

        public T Get(Vector3 pos, Quaternion rot = default)
        {
            if (_queue.Count > 0)
            {
                var nextItem = _queue.Dequeue();
                nextItem.transform.SetPositionAndRotation(pos, rot);
                nextItem.gameObject.SetActive(true);

                return nextItem;
            }

            return CreateNew(pos, _prefab);                        
        }

        public void Reclaim(T item)
        {
            item.gameObject.SetActive(false);
            _queue.Enqueue(item);
        }

        public void Clear()
        {
           foreach(var item in _queue) UnityEngine.Object.Destroy(item);
        }

        private T CreateNew(Vector3 pos, T item)
        {
            return (_resolver != null)
                ? _resolver.Instantiate<T>(item, pos, Quaternion.identity, _poolParent)
                : UnityEngine.Object.Instantiate(item, pos, Quaternion.identity, _poolParent);
        }
    }
}
