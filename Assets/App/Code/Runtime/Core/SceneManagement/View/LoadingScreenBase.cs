using System.Collections.Generic;
using Assets.App.Code.Runtime.Services.Scenes.Operations;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Assets.App.Code.Runtime.Services.Scenes.View
{
    public abstract class LoadingScreenBase : MonoBehaviour, ILoadingScreen
    {
        public bool IsCompleted { get; protected set; }       

        protected virtual void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
        }       

        public abstract void Show();
        public abstract void Hide();
        public abstract UniTask LoadAsync(Queue<ILoadingOperation> operations, string showMsg);       
        public void Reclaim() => Destroy(this.gameObject);
    }
}

