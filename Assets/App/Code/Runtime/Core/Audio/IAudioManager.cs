namespace Assets.App.Code.Runtime.Core.Audio
{
    public interface IAudioManager
    {
        void PlaySound(int sound, float volume = 1f);
        void PlayMusic(int music, float volume = 1f);
        void StopAllMusic();
        void ChangeAmbientSounds(int one, int two, float progress, float volume = 1f);
    }
}
