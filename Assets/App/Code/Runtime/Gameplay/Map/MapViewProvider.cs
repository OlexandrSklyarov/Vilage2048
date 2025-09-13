using UnityEngine;

namespace Assets.App.Code.Runtime.Gameplay.Map
{
    public sealed class MapViewProvider : MonoBehaviour
    {
        [field: SerializeField] public Transform BoxSpawnPoint { get; private set; }
        [field: SerializeField] public Bounds FieldBounds { get; private set; }
        
        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(FieldBounds.center, FieldBounds.size);
        }
    }
}

