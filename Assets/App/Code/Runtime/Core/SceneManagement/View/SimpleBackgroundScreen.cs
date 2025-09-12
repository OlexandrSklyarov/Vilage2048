using System;
using System.Collections.Generic;
using Assets.App.Code.Runtime.Services.Scenes.Operations;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.App.Code.Runtime.Services.Scenes.View
{
    [RequireComponent(typeof(UIDocument))]
    public sealed class SimpleBackgroundScreen : LoadingScreenBase, ILoadingScreen
    {
        private VisualElement _background;
        private Label _loadingLabel;
        private const float HIDE_DURATION = 0.5f;

        public override void Show() => gameObject.SetActive(true);

        public override void Hide() => gameObject.SetActive(false);

        private void Setup()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;

            _background = root.Q<VisualElement>("Background");
            _loadingLabel= root.Q<Label>("LoadingLabel");
        }

        public async override UniTask LoadAsync(Queue<ILoadingOperation> operations, string showMsg)
        {
            Setup();

            DisplayMsg(showMsg);

            _background.style.opacity = 1f;
            _background.style.visibility = Visibility.Visible;

            await UniTask.Delay(100);

            foreach (var operation in operations)
            {
                await operation.Load((progress) => { });
            }           
            
            await UniTask.Delay(100);

            await FadeoutScreen();

            _background.style.visibility = Visibility.Hidden;
        }

        private void DisplayMsg(string msg)
        {
            _loadingLabel.text = !string.IsNullOrEmpty(msg) ? msg : "Loading...";            
        }

        private async UniTask FadeoutScreen()
        {
            while (_background.style.opacity.value > 0f)
            {
                var v = _background.style.opacity.value;
                v -= Time.deltaTime / HIDE_DURATION;
                _background.style.opacity = v;

                await UniTask.Yield();
            }
        }
    }
}
