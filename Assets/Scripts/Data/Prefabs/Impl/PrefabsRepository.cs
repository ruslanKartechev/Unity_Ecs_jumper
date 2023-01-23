using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data.Prefabs.Impl
{
    [CreateAssetMenu(fileName = nameof(PrefabsRepository), menuName = "SO/PrefabsRepository")]
    public class PrefabsRepository : ScriptableObject, IPrefabsRepository
    {
        [SerializeField] private List<PrefabData> _prefabDatas;

        public GameObject GetPrefabGO(string name)
        {
            var prefab = _prefabDatas.Find(t => t.Name == name);
            if (prefab == null)
            {
                throw new SystemException($"[PrefabsRepository] Cannot find a prefab with name: {name}");
            }

            return prefab.Prefab;
        }

        public T GetPrefab<T>(string name)
        {
            var prefab = _prefabDatas.Find(t => t.Name == name);
            if (prefab == null)
            {
                throw new SystemException($"[PrefabsRepository] Cannot find a prefab with name: {name}");
            }

            return prefab.Prefab.GetComponent<T>();
        }
    }
}