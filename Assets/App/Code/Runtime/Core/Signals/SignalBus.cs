using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.App.Code.Runtime.Core.Signals
{
    public sealed class SignalBus : IDisposable
    {
        private readonly Dictionary<Type, List<PriorityCallback>> _signals = new();
        private readonly bool _isShowDebug;
        private object _currentState;

        public SignalBus()
        {
            #if UNITY_EDITOR
            _isShowDebug = true;
            #endif
        }

        public void Subscribe<T>(Action<T> action, int priority = 0, bool isNotify = false)
        {
            var key = typeof(T);

            if (_signals.ContainsKey(key))
            {
                _signals[key].Add(new PriorityCallback(priority, action));
            }
            else
            {
                _signals.Add(key, new List<PriorityCallback>() { new PriorityCallback(priority, action) });
            }

            _signals[key] = _signals[key].OrderByDescending(x => x.Priority).ToList();   

            if (isNotify && _currentState != null)
            {
                action?.Invoke((T)_currentState);
            }        

            TryShowRegisterSignals("[ADD_SIGNAL]");
        }

        public void UnSubscribe<T>(Action<T> action)
        {
            var key = typeof(T);

            if (_signals.ContainsKey(key))
            {
                var callbackToDelete = _signals[key].FirstOrDefault(x => x.Callback.Equals(action));

                if (callbackToDelete != null)
                {
                    _signals[key].Remove(callbackToDelete);
                }

                TryShowRegisterSignals("[REMOVE_SIGNAL]");
            }
        }        

        public void Fire<T>(T signal)
        {
            _currentState = signal;
            var key = typeof(T);

            if (_signals.ContainsKey(key))
            {
                var collection = _signals[key];
                
                for (int i = 0; i < collection.Count; i++)
                {
                    var item = collection.ElementAt(i);
                    var callback = item.Callback as Action<T>;
                    callback?.Invoke(signal);
                }
            }
        }  

        private void TryShowRegisterSignals(string operation)
        {
            if (!_isShowDebug) return;

        #if UNITY_EDITOR

            UnityEngine.Debug.Log($"{operation} ******************");
            int i = 0;
            foreach (var item in _signals)
            {
                UnityEngine.Debug.LogWarning($"[{i}] Signal key :: {item.Key} :: subscribers: {item.Value.Count}");
                i++;
            }

        #endif
        }     

        public void Dispose()
        {
            _signals.Clear();
        }
    }
}
