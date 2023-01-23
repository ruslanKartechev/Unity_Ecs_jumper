using UnityEngine;

namespace Game.Sound.Data
{
    [CreateAssetMenu(fileName = nameof(GlobalSoundSettings), menuName = "SO/Sound/" + nameof(GlobalSoundSettings))]
    public class GlobalSoundSettings : ScriptableObject, IGlobalSoundSettings
    {
        [SerializeField] [Range(0f,1f)] private float _masterVolume;
        [SerializeField]  [Range(0f,1f)] private float _musicVolume;
        [SerializeField] private bool _soundEnabled;
        [SerializeField] private bool _musicEnabled;

        
        public float MasterVolume
        {
            get => _masterVolume;
            set => _masterVolume = value;
        }
        
        public float MusicVolume
        {
            get => _musicVolume;
            set => _musicVolume = value;
        }
        
        public bool SoundEnabled
        {
            get => _soundEnabled;
            set => _soundEnabled = value;
        }
        
        public bool MusicEnabled
        {
            get => _musicEnabled;
            set => _musicEnabled = value;
        }
    }
}