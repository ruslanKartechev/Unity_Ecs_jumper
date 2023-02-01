using System.Collections.Generic;

namespace Data
{
    public interface ISpawnDelayData
    {
        public IEnumerable<SpawnDelayByTierData> SpawnData { get; }

    }
}