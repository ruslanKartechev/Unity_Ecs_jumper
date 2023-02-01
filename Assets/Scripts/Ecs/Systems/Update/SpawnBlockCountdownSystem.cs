using Ecs.Components;
using Leopotam.EcsLite;
using Services.Time;
using Zenject;

namespace Ecs.Systems
{
    public class SpawnBlockCountdownSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsPool<ElapsedSinceBlockBlockSpawn> _pool;
        private EcsWorld _world;
        [Inject] private ITimeService _timeService;
        
        
        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _filter = _world.Filter<ElapsedSinceBlockBlockSpawn>().Inc<CanSpawnComponent>().Inc<BlockSpawnDelayComponent>().End();
            _pool = _world.GetPool<ElapsedSinceBlockBlockSpawn>();
        }
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                ref var spawnDelay = ref _world.GetComponent<BlockSpawnDelayComponent>(entity);

                ref var elapsedComponent = ref _pool.Get(entity);
                elapsedComponent.Value += _timeService.DeltaTime;
                if (elapsedComponent.Value >= spawnDelay.Value)
                {
                    elapsedComponent.Value = 0f;
                    Pool.World.AddComponentToEntity<SpawnRandomBlockComponent>(Pool.MapEntity);
                }
            }   
        }
    }
}