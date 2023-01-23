using Game.Sound.Data;

namespace Game.Sound
{
    public interface IAudioSourceProvider
    {
        void Init();
        SoundSource GetFreeAudioSource();
        void ReturnSource(SoundSource source);
    }
}