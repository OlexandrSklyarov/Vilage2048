using System.Collections;
using UnityEngine;

namespace Assets.App.Code.Runtime.Services.Scenes.View
{
    public abstract class AnimatedLoadingScreenElementBase : MonoBehaviour
    {
        public abstract IEnumerator StartAnimation();
    }
}
