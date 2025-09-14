using Assets.App.Code.Runtime.Data.Audio;
using NaughtyAttributes;
using UnityEngine;

namespace Assets.App.Code.Runtime.Core.Audio
{
    [System.Serializable]
    public sealed class MusicItem : IAudioItem
    {
        public int Id => (int)_musicId;
        [SerializeField] private MusicType _musicId;
        [field: HorizontalLine(color:EColor.Pink), Space, SerializeField]  public AudioClip[] Clips {get; private set;}
    }
}

