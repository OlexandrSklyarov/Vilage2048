using UnityEngine;

namespace Assets.App.Code.Runtime.Gameplay.Map.Data
{
    [CreateAssetMenu(fileName = "MapConfig", menuName = "Configs/MapConfig")]
    public sealed class MapConfig : ScriptableObject
    {
        [field: SerializeField] public MapController MapPrefab { get; private set; }
    }
}