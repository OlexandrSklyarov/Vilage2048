using Assets.App.Code.Runtime.Services.Scenes.View;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.App.Code.Runtime.Core.Configs
{
    [CreateAssetMenu(fileName = "UIConfig", menuName = "Configs/UIConfig")]
    public sealed class UIConfig : ScriptableObject
    {
        [field: SerializeField] public SimpleBackgroundScreen SimpleBackgroundScreenPrefab { get; private set; }

        [field: Space, SerializeField] public VisualTreeAsset HUDScreenRef  { get; private set; }
        [field: SerializeField] public VisualTreeAsset PauseScreenRef  { get; private set; }
        [field: SerializeField] public VisualTreeAsset WinScreenRef  { get; private set; }
        [field: SerializeField] public VisualTreeAsset GameOverScreenRef  { get; private set; }
        
        [field: Space, SerializeField] public StyleSheet GameMenuStylesRef  { get; private set; }
    }
}

