using System.Collections.Generic;
using UnityEngine;

namespace Data.Impl
{
    [CreateAssetMenu(fileName = "SpawnDelayData", menuName = "SO/SpawnDelayData")]
    public class SpawnDelayData : ScriptableObject, ISpawnDelayData
    {
        [SerializeField] private List<SpawnDelayByTierData> _spawnDelayByTier;
        
        
        public IEnumerable<SpawnDelayByTierData> SpawnData => _spawnDelayByTier;
    }
}