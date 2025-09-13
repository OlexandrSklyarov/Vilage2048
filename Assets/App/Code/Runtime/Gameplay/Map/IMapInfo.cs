using UnityEngine;

namespace Assets.App.Code.Runtime.Gameplay.Map
{
    public interface IMapInfo
    {
        Transform BoxSpawnPoint { get;  }
        Bounds FieldBounds { get; }
    }
}

