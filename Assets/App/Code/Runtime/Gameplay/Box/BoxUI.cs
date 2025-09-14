using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.App.Code.Runtime.Gameplay.Box
{
    [RequireComponent(typeof(UIDocument))]
    public sealed class BoxUI : MonoBehaviour
    {
        private VisualElement _root;
        private Label _label;

        public void SetNumber(int number)
        {
            Init();
            
            _label.text = $"{number}";
        }

        private void Init()
        {
            _root ??= GetComponent<UIDocument>().rootVisualElement;
            _label ??= _root.Q<Label>("NumLabel");
        }
    }
}

