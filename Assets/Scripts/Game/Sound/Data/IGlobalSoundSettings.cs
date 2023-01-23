namespace Game.Sound.Data
{
    public interface IGlobalSoundSettings
    {
        float MasterVolume { get; set; }
        float MusicVolume { get; set; }
        bool SoundEnabled { get; set; }
        bool MusicEnabled { get; set; }
        
    }
}