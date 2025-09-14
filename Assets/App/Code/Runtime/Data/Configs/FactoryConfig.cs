using UnityEngine;
using Assets.App.Code.Runtime.Gameplay.Box;
using Assets.App.Code.Runtime.Gameplay.VFX;

namespace Assets.App.Code.Runtime.Data.Configs
{
    [CreateAssetMenu(fileName = "FactoryConfig", menuName = "Configs/FactoryConfig")]
    public sealed class FactoryConfig : ScriptableObject
    {
        [field: SerializeField] public BoxView BoxPrefab { get; private set; }
        [field: Space(50), SerializeField] public VfxItem[] Vfx { get; private set; }


        [System.Serializable]
        public sealed class VfxItem
        {
            [field: SerializeField] public GameVfxType Type { get; private set; }
            [field: SerializeField] public VfxView Prefab { get; private set; }
        }
    }
}

