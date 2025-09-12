using Assets.App.Code.Runtime.Services.Scenes.View;
using UnityEngine;

namespace Assets.App.Code.Runtime.Core.Configs
{
    [CreateAssetMenu(fileName = "UIConfig", menuName = "Configs/UIConfig")]
    public sealed class UIConfig : ScriptableObject
    {
        [field: SerializeField] public SimpleBackgroundScreen SimpleBackgroundScreenPrefab { get; private set; }
    }
}

