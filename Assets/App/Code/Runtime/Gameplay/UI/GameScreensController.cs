using System;
using System.Collections.Generic;
using Assets.App.Code.Runtime.Core.Initializable;
using Assets.App.Code.Runtime.Core.Signals;
using Assets.App.Code.Runtime.Gameplay.UI.Screens;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;
using VContainer;

namespace Assets.App.Code.Runtime.Gameplay.UI
{
    [RequireComponent(typeof(UIDocument))]
    public sealed class GameScreensController : MonoBehaviour, IAsyncInitializeProcess
    {
        private Dictionary<Type, BaseScreen> _screens = new();
        private SignalBus _signalBus;
        private VisualElement _rootVisualElement;
        private VisualElement _screenContainer;
        private readonly List<VisualElement> _items = new();

        [Inject]
        private void Construct(SignalBus signalBus, IEnumerable<BaseScreen> screenCollection)
        {
            _signalBus = signalBus;

            //add to screen collection
            foreach (var screen in screenCollection)
            {
                _screens.Add(screen.GetType(), screen);
            }
        }

        void Start()
        {
            _rootVisualElement = GetComponent<UIDocument>().rootVisualElement;
            _screenContainer = _rootVisualElement.Q("RootScreenContainer");
            _screenContainer.pickingMode = PickingMode.Ignore;
        }

        public async UniTask InitializeAsync()
        {
            //screen initialize
            foreach (var screen in _screens)
            {
                await screen.Value.InitializeAsync();
            }

            _signalBus.Subscribe<Signal.ShowGameplayScreen.HUD>(OnShowHUD);
            _signalBus.Subscribe<Signal.ShowGameplayScreen.PauseMenu>(OnShowPauseMenu);
            _signalBus.Subscribe<Signal.ShowGameplayScreen.WinScreen>(OnShowWin);
            _signalBus.Subscribe<Signal.ShowGameplayScreen.GameOverScreen>(OnShowGameOver);
        }


        private void Unsubscribe()
        {
            _signalBus.UnSubscribe<Signal.ShowGameplayScreen.HUD>(OnShowHUD);
            _signalBus.UnSubscribe<Signal.ShowGameplayScreen.PauseMenu>(OnShowPauseMenu);
            _signalBus.UnSubscribe<Signal.ShowGameplayScreen.WinScreen>(OnShowWin);
            _signalBus.UnSubscribe<Signal.ShowGameplayScreen.GameOverScreen>(OnShowGameOver);
        }

        private void OnShowWin(Signal.ShowGameplayScreen.WinScreen evt)
        {
            HideAllScreens();
            ShowScreen<WinScreen>();
        }

        private void OnShowGameOver(Signal.ShowGameplayScreen.GameOverScreen evt)
        {
            HideAllScreens();
            ShowScreen<GameOverScreen>();
        }

        private void OnShowPauseMenu(Signal.ShowGameplayScreen.PauseMenu evt)
        {
            HideAllScreens();
            ShowScreen<PauseScreen>();
        }

        private void OnShowHUD(Signal.ShowGameplayScreen.HUD evt)
        {
            HideAllScreens();
            ShowScreen<HudScreen>();
        }

        private void HideAllScreens()
        {
            foreach (var screen in _screens)
            {
                screen.Value.Hide();
            }
        }

        private void ShowScreen<T>() where T : BaseScreen
        {
            if (!_screens.TryGetValue(typeof(T), out var screen))
            {
                throw new ArgumentException($"not found screen type {typeof(T)}");
            }

            screen.Show(_screenContainer);
        }

        void OnDestroy()
        {
            Unsubscribe();

            foreach (var screen in _screens)
            {
                if (screen.Value is IDisposable item)
                {
                    item.Dispose();
                }
            }
        }
    }
}

