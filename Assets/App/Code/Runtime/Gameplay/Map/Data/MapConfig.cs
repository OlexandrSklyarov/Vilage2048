using UnityEngine;

namespace Assets.App.Code.Runtime.Gameplay.Map.Data
{
    [CreateAssetMenu(fileName = "MapConfig", menuName = "Configs/MapConfig")]
    public sealed class MapConfig : ScriptableObject
    {
        [field: SerializeField] public MapViewProvider MapPrefab { get; private set; }
        [field: Space, SerializeField, Min(2)] public int MaxNumberToWin { get; private set; } = 2048;
    }
}