using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace Assets.App.Code.Runtime.Services.Scenes.Operations
{
    public sealed class SimpleTaskGroupOperation : ILoadingOperation
    {
        private readonly List<UniTask> _tasks;

        public SimpleTaskGroupOperation(List<UniTask> tasks)
        {
            _tasks = tasks;
        }

        public async UniTask Load(Action<float> onProgressCallback)
        {
            onProgressCallback?.Invoke(0f);

            await UniTask.WhenAll(_tasks);

            onProgressCallback?.Invoke(1f);
        }        
    }
}