using System.Collections.Generic;
using Data;
using Helpers;
using UnityEngine;
using Zenject;

namespace Services.Level.Impl
{
    [CreateAssetMenu(fileName ="PrefsLevelService", menuName = "SO/PrefsLevelService")]
    public class PrefsLevelService : ScriptableObject, ILevelService
    {
        [SerializeField] private List<GameObject> LevelPrefabs;
        
        private int _maxIndex = 0;

        private void OnEnable()
        {
            if(PlayerPrefs.HasKey(PrefsKeysHolder.KEY_CURRENT) == false)
                PlayerPrefs.SetInt(PrefsKeysHolder.KEY_CURRENT, 0);
            if(PlayerPrefs.HasKey(PrefsKeysHolder.KEY_TOTAL) == false)
                PlayerPrefs.SetInt(PrefsKeysHolder.KEY_TOTAL, 0);
            for (int i = LevelPrefabs.Count - 1; i >= 0; i--)
            {
                if (LevelPrefabs[i] == null)
                {
                    LevelPrefabs.RemoveAt(i);
                }
            }
            _maxIndex = LevelPrefabs.Count - 1;
        }

        public int CurrentLevelIndex
        {
            get => CorrectIndex(PlayerPrefs.GetInt(PrefsKeysHolder.KEY_CURRENT));
            set => PlayerPrefs.SetInt(PrefsKeysHolder.KEY_CURRENT, CorrectIndex(value));
        }

        public int TotalLevelsPassed
        {
            get => PlayerPrefs.GetInt(PrefsKeysHolder.KEY_TOTAL);
            set
            {
                var rec = value;
                if (rec < 0)
                    rec = 0;
                PlayerPrefs.SetInt(PrefsKeysHolder.KEY_TOTAL, rec);
            }
        }
        
        public int NextLevel()
        {
            CurrentLevelIndex += 1;
            TotalLevelsPassed += 1;
            return CurrentLevelIndex;
        }

        public int PrevLevel()
        {
            CurrentLevelIndex -= 1;
            TotalLevelsPassed -= 1;
            return CurrentLevelIndex;
        }

        public int CorrectIndex(int index)
        {
            _maxIndex = LevelPrefabs.Count - 1;
            if (index > _maxIndex)
                index = _maxIndex;
            if (index <= 0)
                index = 0;
            return index;
        }

        public GameObject GetPrefab(int index)
        {
            return LevelPrefabs[index];
        }

        public int GetTotalCount()
        {
            return LevelPrefabs.Count;
        }
    }
}