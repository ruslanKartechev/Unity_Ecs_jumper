using System.Collections.Generic;
using Game.Level.Impl;
using UnityEngine;

namespace Data.Impl
{
    [CreateAssetMenu(fileName = "LevelRepository", menuName = "SO/LevelRepository")]
    public class LevelRepository : ScriptableObject, ILevelRepository
    {
        [SerializeField] private List<LevelView> _levels;
        
        public LevelView GetLevel(int index)
        {
            if (index >= 0 && index < _levels.Count)
            {
                return _levels[index];
            }

            return null;
        }

        public int MaxIndex => _levels.Count;
    }
}