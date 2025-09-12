using UnityEngine;
using UnityEngine.UIElements;
using VContainer;

namespace Assets.App.Code.Runtime.Gameplay.UI
{
    [RequireComponent(typeof(UIDocument))]
    public sealed class HUDViewController : MonoBehaviour
    {
        [Inject]
        private void Construct()
        {
            var root = GetComponent<UIDocument>().rootVisualElement;
        }
    }
}

