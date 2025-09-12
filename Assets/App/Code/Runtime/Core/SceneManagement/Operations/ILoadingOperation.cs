using System;
using Cysharp.Threading.Tasks;

namespace Assets.App.Code.Runtime.Services.Scenes.Operations
{
    public interface ILoadingOperation
    {        
        UniTask Load(Action<float> onProgressCallback);
    }
}