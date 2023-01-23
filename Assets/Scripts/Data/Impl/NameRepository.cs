using System.Collections.Generic;
using UnityEngine;

namespace Data.Impl
{
    [CreateAssetMenu(fileName = "NameRepository", menuName = "SO/NameRepository")]

    public class NameRepository : ScriptableObject, INameRepository
    {
        [SerializeField] private List<string> _names;
        [SerializeField] private string _playerName;
        
        private int _indexSeed;

        private void OnEnable()
        {
            _indexSeed = UnityEngine.Random.Range(0, _names.Count);
        }

        public string GetRandomName()
        {
            if (_indexSeed >= _names.Count)
                _indexSeed = 0;
            var result = _names[_indexSeed];
            _indexSeed++;
            return result;
        }

        public string PlayerName() => _playerName;
    }
}