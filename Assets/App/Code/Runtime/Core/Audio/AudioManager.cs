using System;
using System.Linq;
using UnityEngine;
using System.Threading;
using Cysharp.Threading.Tasks;
using Assets.App.Code.Runtime.Data.Configs;

namespace Assets.App.Code.Runtime.Core.Audio
{
    public sealed class AudioManager : IAudioManager, IDisposable
    {
        private readonly AudioConfig _audioConfig;
        private AudioSource[] _soundSources;
        private AudioSource _musicSourcesA;
        private AudioSource _musicSourcesB;
        private AudioSource _ambientSourcesA;
        private AudioSource _ambientSourcesB;
        private CancellationTokenSource _changeMusicCts;
        private bool _isPlayingMusicSourceA;
      

        public AudioManager(AudioConfig audioConfig)
        {
            _audioConfig = audioConfig;

            var go = new GameObject("[AUDIO_GO]");

            UnityEngine.Object.DontDestroyOnLoad(go);

            //listener
            go.AddComponent<AudioListener>();

            //sounds
            _soundSources = CreateAudioSources(go, 16);

            //music
            _musicSourcesA = CreateMusicSource(go);
            _musicSourcesB = CreateMusicSource(go);

            //ambient
            _ambientSourcesA = CreateAmbientSource(go);
            _ambientSourcesB = CreateAmbientSource(go);
        }   

        public void ChangeAmbientSounds(int one, int two, float progress, float volume = 1)
        {
            //TODO: Need implemented ChangeAmbientSounds

            //_ambientSourcesA;
            //_ambientSourcesB;
        }
        
        public void PlayMusic(int musicId, float volume = 1)
        {
            var clip = GetRandomClip(_audioConfig.Musics.First(x => x.Id == musicId).Clips);          

            var currentSource = (_isPlayingMusicSourceA) ? _musicSourcesA : _musicSourcesB; 
            var nextSource = (_isPlayingMusicSourceA) ? _musicSourcesB : _musicSourcesA; 

            nextSource.clip = clip;
            nextSource.volume = 0f;
            nextSource.loop = true;
            nextSource.Play();

            StopPreviousMusicOperation();

            var token = (_changeMusicCts = new CancellationTokenSource()).Token;

            ChangeMusicAsync(currentSource, nextSource, token).Forget();  

            _isPlayingMusicSourceA = !_isPlayingMusicSourceA;
        }

        public void StopAllMusic()
        {
            StopPreviousMusicOperation();
            _musicSourcesA.Stop(); 
            _musicSourcesB.Stop();
        }

        public void PlaySound(int sfxId, float volume = 1)
        {
            var clip = GetRandomClip(_audioConfig.Sounds.First(x => x.Id == sfxId).Clips);

            var source = _soundSources.FirstOrDefault(x => x.isPlaying == false);

            if (source == null) source = _soundSources.First();

            source.PlayOneShot(clip);
        }

        private AudioSource CreateMusicSource(GameObject go)
        {
            var source = go.AddComponent<AudioSource>();
            source.outputAudioMixerGroup = _audioConfig.Mixer.FindMatchingGroups(AudioConst.MixerGroup.MUSIC)[0];

            return source;
        }

        private AudioSource CreateAmbientSource(GameObject go)
        {
            var source = go.AddComponent<AudioSource>();
            source.outputAudioMixerGroup = _audioConfig.Mixer.FindMatchingGroups(AudioConst.MixerGroup.AMBIENT)[0];

            return source;
        }

        private AudioSource[] CreateAudioSources(GameObject root, int count)
        {
            var sources = new AudioSource[count];

            for (int i = 0; i < sources.Length; i++)
            {
                sources[i] = root.AddComponent<AudioSource>();
                sources[i].outputAudioMixerGroup = _audioConfig.Mixer.FindMatchingGroups(AudioConst.MixerGroup.SOUNDS)[0];
            }

            return sources;
        }

        private void StopPreviousMusicOperation()
        {
            _changeMusicCts?.Cancel();
            _changeMusicCts?.Dispose();
            _changeMusicCts = null;
        }

        private async UniTask ChangeMusicAsync(AudioSource currentSource, AudioSource nextSource, CancellationToken token)
        {
            var progress = 0f;

            while(progress < 1f)
            {
                await UniTask.Yield(cancellationToken: token);
                if (token.IsCancellationRequested) return;

                progress += UnityEngine.Time.unscaledDeltaTime / _audioConfig.ChangeMusicSpeed;

                currentSource.volume = 1f - progress;
                nextSource.volume = progress;
            }

            currentSource.volume = 0f;
            nextSource.volume = 1f;

            currentSource.Stop();
        }        

        private AudioClip GetRandomClip(AudioClip[] clips)
        {
            return clips[UnityEngine.Random.Range(0, clips.Length)];
        }
        

        public void Dispose()
        {
            StopPreviousMusicOperation();
        }        
    }
}
