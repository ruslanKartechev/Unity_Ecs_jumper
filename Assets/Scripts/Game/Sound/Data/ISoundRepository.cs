namespace Game.Sound.Data
{
    public interface ISoundRepository
    {
        void Init();
        SoundStoreData GetSound(string name);
    }
}