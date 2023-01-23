using Services.Parent;
using UnityEngine;
using Zenject;

namespace Services.Instantiate.Impl
{
    public class InstantiateService : IInstantiateService
    {
        private readonly DiContainer _container;
        private readonly IParentService _parentService;
        
        public InstantiateService(DiContainer container, IParentService parentService)
        {
            _container = container;
            _parentService = parentService;
        }
        
        public T Spawn<T>(GameObject prefab)
        {
            return Spawn<T>(prefab, _parentService.DefaultParent, Vector3.zero, Quaternion.identity);
        }

        public T Spawn<T>(GameObject prefab, Transform parent)
        {
            return Spawn<T>(prefab, parent, Vector3.zero, Quaternion.identity);
        }

        public T Spawn<T>(GameObject prefab, Transform parent, Vector3 position)
        {
            return Spawn<T>(prefab, parent, position, Quaternion.identity);
        }

        public T Spawn<T>(GameObject prefab, Transform parent, Vector3 position, Quaternion rotation)
        {
            var go =  _container.InstantiatePrefab(prefab, position, rotation, parent);
            return go.GetComponent<T>();
        }
    }
}