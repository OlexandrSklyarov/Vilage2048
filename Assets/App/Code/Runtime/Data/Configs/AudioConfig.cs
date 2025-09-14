using System.Collections.Generic;
using System.Diagnostics;
using Assets.App.Code.Runtime.Core.Audio;
using Assets.App.Code.Runtime.Util;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Audio;

namespace Assets.App.Code.Runtime.Data.Configs
{
    [CreateAssetMenu(fileName = "AudioConfig", menuName = "Configs/AudioConfig"), ExecuteInEditMode]
    public sealed class AudioConfig : ScriptableObject
    {
        [field: SerializeField] public AudioMixer Mixer {get; private set;}
        [field: Space, SerializeField, Min(0.01f)] public float ChangeMusicSpeed {get; private set;} = 2f;
        [field: HorizontalLine(color:EColor.Green), Space(20), SerializeField] public MusicItem[] Musics {get; private set;}
        [field: HorizontalLine(color:EColor.Orange), Space(20), SerializeField] public SfxItem[] Sounds {get; private set;}

        private HashSet<int> _hashSet = new();

        [Conditional("UNITY_EDITOR")]
        private void OnValidate()
        {
            CheckDuplicate(Musics);
            CheckDuplicate(Sounds);
        }

        [Conditional("UNITY_EDITOR")]
        private void CheckDuplicate(IAudioItem[] items)
        {    
           _hashSet.Clear();

            foreach(var i in items)
            {
                if (_hashSet.Contains(i.Id))
                {
                    DebugLog.PrintError($"The collection already has a type [{i.Id}]");
                }

                _hashSet.Add(i.Id);
            }
        }
    }
}