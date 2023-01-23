using System.Collections.Generic;
using UnityEngine;

namespace Data.Impl
{
    [CreateAssetMenu(fileName = "FlagRepository", menuName = "SO/FlagRepository")]
    public class FlagRepository : ScriptableObject, IFlagRepository
    {
        [SerializeField] private List<Sprite> _sprites;
        [SerializeField] private Sprite _playerFlag;        
        
        private int _indexSeed;

        private void OnEnable()
        {
            _indexSeed = UnityEngine.Random.Range(0, _sprites.Count);
        }
        
        public Sprite GetRandomSprite()
        {
            if (_indexSeed >= _sprites.Count)
                _indexSeed = 0;
            var result = _sprites[_indexSeed];
            _indexSeed++;
            return result;
        }

        public Sprite PlayerFlag() => _playerFlag;
    }
}