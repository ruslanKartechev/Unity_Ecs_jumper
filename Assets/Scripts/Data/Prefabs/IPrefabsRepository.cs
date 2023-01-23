using UnityEngine;

namespace Data.Prefabs
{
    public interface IPrefabsRepository
    {
        GameObject GetPrefabGO(string name);
        T GetPrefab<T>(string name);
    }
}