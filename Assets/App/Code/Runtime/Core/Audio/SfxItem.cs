using UnityEngine;
using NaughtyAttributes;
using Assets.App.Code.Runtime.Data.Audio;

namespace Assets.App.Code.Runtime.Core.Audio
{
    [System.Serializable]
    public sealed class SfxItem : IAudioItem
    {
        public int Id => (int)_sfxId;
        [SerializeField] private SfxType _sfxId;
        [field: HorizontalLine(color:EColor.Blue), Space, SerializeField]  public AudioClip[] Clips {get; private set;}
    }
}