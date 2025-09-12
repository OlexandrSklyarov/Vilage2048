using System;
using Cysharp.Threading.Tasks;
using UnityEngine.UIElements;

namespace Assets.App.Code.Runtime.Gameplay.UI.Screens
{
    public abstract class BaseHudScreen : IDisposable
    {
        public VisualElement Root { get; protected set; }
        public bool IsVisible => Root?.parent != null;

        public abstract UniTask InitializeAsync();

        public virtual void Show(VisualElement parent)
        {
            parent.Add(Root);
        }

        public virtual void Hide()
        {
            Root.RemoveFromHierarchy();
        }

        public abstract void Dispose();
    }
}

