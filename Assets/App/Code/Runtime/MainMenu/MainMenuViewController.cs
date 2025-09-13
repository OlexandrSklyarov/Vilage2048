using Assets.App.Code.Runtime.Core.Signals;
using UnityEngine;
using UnityEngine.UIElements;
using VContainer;

namespace Assets.App.Code.Runtime.MainMenu
{
    [RequireComponent(typeof(UIDocument))]
    public sealed class MainMenuViewController : MonoBehaviour
    {
        private Button _playButton;
        private SignalBus _signalBus;
        private VisualElement _root;

        [Inject]
        private void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;              
        }

        void Awake()
        {
            _root = GetComponent<UIDocument>().rootVisualElement;
            _playButton = _root.Q<Button>("PlayButton");
            _playButton.clicked += OnPlayButtonClicked;      
        }
        
        private void OnDestroy()
        {
            _playButton.clicked -= OnPlayButtonClicked;
        }

        private void OnPlayButtonClicked()
        {
            _playButton.SetEnabled(false);
            _signalBus.Fire(new Signal.App.PlayGame());
        }
    }
}

