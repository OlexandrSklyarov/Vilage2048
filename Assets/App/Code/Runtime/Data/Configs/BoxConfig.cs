using UnityEngine;

namespace Assets.App.Code.Runtime.Data.Configs
{
    [CreateAssetMenu(fileName = "BoxConfig", menuName = "Configs/BoxConfig")]
    public sealed class BoxConfig : ScriptableObject
    {
        [field: Space, SerializeField] public Color Box2 { get; private set; } = Color.grey;
        [field: Space, SerializeField] public Color Box4 { get; private set; } = Color.grey;
        [field: Space, SerializeField] public Color Box8 { get; private set; } = Color.grey;
        [field: Space, SerializeField] public Color Box16 { get; private set; } = Color.grey;
        [field: Space, SerializeField] public Color Box32 { get; private set; } = Color.grey;
        [field: Space, SerializeField] public Color Box64 { get; private set; } = Color.grey;
        [field: Space, SerializeField] public Color Box128 { get; private set; } = Color.grey;
        [field: Space, SerializeField] public Color Box256 { get; private set; } = Color.grey;
        [field: Space, SerializeField] public Color Box512 { get; private set; } = Color.grey;
        [field: Space, SerializeField] public Color Box1024 { get; private set; } = Color.grey;
        [field: Space, SerializeField] public Color Box2048 { get; private set; } = Color.grey;

        [field: Space(20), SerializeField, Range(0.01f, 1f)] public float SpawnBigNumChance { get; private set; } = 0.25f;
        [field: Space, SerializeField, Min(1f)] public float MoveSpeed { get; private set; } = 12f;
        [field: Space, SerializeField, Min(1f)] public float PushForce{ get; private set; } = 35f;
        [field: Space, SerializeField, Min(1f)] public float CollideBoxForce { get; private set; } = 10f;
        [field: Space, SerializeField, Min(0.01f)] public float SpawnBoxDelay { get; private set; } = 0.5f;
        [field: Space, SerializeField, Min(0.01f)] public float MinCollideImpulse { get; private set; } = 0.5f;
    }
}