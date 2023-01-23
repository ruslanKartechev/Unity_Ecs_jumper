using Ecs.Components;
using Leopotam.EcsLite;
using Services.Time;
using UnityEngine;
using Zenject;

namespace Ecs.Systems
{
    public class SpawnBlockCountdownSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsPool<ElapsedSinceBlockBlockSpawn> _pool;
        [Inject] private ITimeService _timeService;
        
        
        public void Init(IEcsSystems systems)
        {
            _filter = systems.GetWorld().Filter<ElapsedSinceBlockBlockSpawn>().Inc<CanSpawnComponent>().End();
            _pool = systems.GetWorld().GetPool<ElapsedSinceBlockBlockSpawn>();
        }
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                ref var spawnDelay = ref Pool.World.GetComponent<BlockSpawnDelayComponent>(Pool.LevelEntity);

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