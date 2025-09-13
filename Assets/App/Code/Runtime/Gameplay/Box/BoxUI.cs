using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.App.Code.Runtime.Gameplay.Box
{
    [RequireComponent(typeof(UIDocument))]
    public sealed class BoxUI : MonoBehaviour
    {
        private Label _label;

        void Awake()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;
            _label = root.Q<Label>("NumLabel");
        }

        public void SetNumber(int number)
        {
            _label.text = number.ToString();
        }
    }
}

