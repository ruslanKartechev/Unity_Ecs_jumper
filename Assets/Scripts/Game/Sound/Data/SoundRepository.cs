using System.Collections.Generic;
using System.Linq;
using Helpers;
using UnityEngine;

namespace Game.Sound.Data
{
  [CreateAssetMenu(fileName = nameof(SoundRepository), menuName = "SO/Sound/" + nameof(SoundRepository))]
    public class SoundRepository : ScriptableObject, ISoundRepository
    {
        [SerializeField] private List<SoundStoreData> _sounds;
        [SerializeField] private List<MultiSoundOptions> _multiOptions;
        [SerializeField] private List<SoundStoreData> _music;

        private Dictionary<string, SoundStoreData> _soundByName = new Dictionary<string, SoundStoreData>();
        private Dictionary<string, SoundStoreData> _musicByName = new Dictionary<string, SoundStoreData>();
        private Dictionary<string, MultiSoundOptions> _multiOptionsByName = new Dictionary<string, MultiSoundOptions>();

        
        public void Init()
        {
            foreach (var data in _sounds)
            {
                _soundByName.Add(data.Name, data);
            }

            foreach (var data in _music)
            {
                _musicByName.Add(data.Name, data);
            }

            foreach (var data in _multiOptions)
            {
                data.Init();
                _multiOptionsByName.Add(data.Name, data);
            }
        }

        public SoundStoreData GetSound(string name)
        {
            if (_soundByName.ContainsKey(name))
            {
                return _soundByName[name];
            }
            if (_musicByName.ContainsKey(name))
            {
                return _musicByName[name];
            }

            if (_multiOptionsByName.ContainsKey(name))
            {
                var multiData = _multiOptionsByName[name];
                var sound = multiData.GetOption();
                var storeData = new SoundStoreData()
                {
                    Name = multiData.Name, 
                    Type = multiData.Type,
                    Clip = sound.Clip,
                    BaseVolume = sound.Volume,
                    BasePitch = sound.Pitch
                };
                return storeData;
            }
            Dbg.LogException($"Cannot find: {name} sound in repository");
            return null;
        }
    }
}