using UnityEngine;
using Assets.App.Code.Runtime.Gameplay.Box;

namespace Assets.App.Code.Runtime.Data.Configs
{
    [CreateAssetMenu(fileName = "FactoryConfig", menuName = "Configs/FactoryConfig")]
    public sealed class FactoryConfig : ScriptableObject
    {
        [field: SerializeField] public BoxView BoxPrefab { get; private set; }
    }
}

