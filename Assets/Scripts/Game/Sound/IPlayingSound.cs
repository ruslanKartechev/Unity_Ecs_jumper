namespace Game.Sound
{
    public interface IPlayingSound
    {
        void Pause();
        void Resume();
        void Kill();
    }
}