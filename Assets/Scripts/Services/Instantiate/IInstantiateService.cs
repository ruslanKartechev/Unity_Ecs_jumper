using UnityEngine;

namespace Services.Instantiate
{
    public interface IInstantiateService
    {
        T Spawn<T>(GameObject prefab);
        T Spawn<T>(GameObject prefab, Transform parent);
        T Spawn<T>(GameObject prefab, Transform parent, Vector3 position);
        T Spawn<T>(GameObject prefab, Transform parent, Vector3 position, Quaternion rotation);
    }
}