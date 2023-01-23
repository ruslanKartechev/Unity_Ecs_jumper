using Data.Prefabs;
using UnityEngine;
using Zenject;

namespace Services.Prefabs
{
    public class PrefabsService
    {
        private readonly IPrefabsRepository _prefabsRepository;

        [Inject]
        public PrefabsService(IPrefabsRepository prefabsRepository)
        {
            _prefabsRepository = prefabsRepository;
        }
        
        public GameObject SpawnGameObject(string name)
        {
            var prefab = _prefabsRepository.GetPrefabGO(name);
            var instance = UnityEngine.Object.Instantiate(prefab) as GameObject;
            return instance;
        }
        
        public T SpawnFromPrefab<T>(string name)
        {
            var prefab = _prefabsRepository.GetPrefabGO(name);
            var instance = UnityEngine.Object.Instantiate(prefab) as GameObject;
            var objectInstance = instance.GetComponent<T>();
            if(objectInstance == null)
                Debug.Log($"[PrefabsRepository] Cannot get component of type: {typeof(T).ToString()} from prefab with name: {name}");
            return objectInstance;
        }
    }
}