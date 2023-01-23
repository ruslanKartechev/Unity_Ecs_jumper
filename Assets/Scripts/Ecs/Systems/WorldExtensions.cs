using Leopotam.EcsLite;

namespace Ecs.Systems
{
    public static class WorldExtensions
    {
        public static ref T AddComponentToNew<T>(this EcsWorld world) where T : struct
        {
            var entity = world.NewEntity();
            var pool = world.GetPool<T>();
            return ref pool.Add(entity);
        }

        public static ref T AddComponentToEntity<T>(this EcsWorld world, int entity) where T : struct
        {
            var pool = world.GetPool<T>();
            return ref pool.Add(entity);
        }

        public static ref T GetComponent<T>(this EcsWorld world, int entity) where T : struct
        {
            var pool = world.GetPool<T>();
            return ref pool.Get(entity);
        }

        public static void RemoveComponent<T>(this EcsWorld world, int entity) where T : struct
        {
            var pool = world.GetPool<T>();
            pool.Del(entity);
        }

        public static bool HasComponent<T>(this EcsWorld world, int entity) where T : struct
        {
            var pool = world.GetPool<T>();
            return pool.Has(entity);
        }
        
    }
}