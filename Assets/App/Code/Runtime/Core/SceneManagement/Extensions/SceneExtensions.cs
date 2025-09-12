using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.App.Code.Runtime.Services.Extensions
{
    public static class SceneExtensions
    {
        public static async UniTask LoadSceneAsync(string sceneName, LoadSceneMode mode, Action<float> onProgressCallback)
        {
            await SceneManager.LoadSceneAsync(sceneName, mode)
               .ToUniTask
               (
                   Progress.CreateOnlyValueChanged(progress => onProgressCallback?.Invoke(progress),
                   EqualityComparer<float>.Default)
               );
        }        

        public static T FindByType<T>() where T : MonoBehaviour
        {
            return UnityEngine.Object.FindFirstObjectByType<T>();
        }
        
        public static GameObject FindByTag(string tag)
        {
            return UnityEngine.GameObject.FindGameObjectWithTag(tag);
        }
    }    
}

