using Assets.App.Code.Runtime.Gameplay.Map.Data;
using UnityEngine;

namespace Assets.App.Code.Runtime.Data.Configs
{
    [CreateAssetMenu(fileName = "AppConfig", menuName = "Configs/AppConfig")]
    public sealed class AppConfig : ScriptableObject
    {
        [field: Space, SerializeField] public AudioConfig Audio { get; private set; }
        [field: Space, SerializeField] public UIConfig UI { get; private set; }
        [field: Space, SerializeField] public FactoryConfig Factory { get; private set; }
        [field: Space, SerializeField] public BoxConfig BoxInfo { get; private set; }
        [field: Space, SerializeField] public MapConfig[] Maps { get; private set; }
    }
}

