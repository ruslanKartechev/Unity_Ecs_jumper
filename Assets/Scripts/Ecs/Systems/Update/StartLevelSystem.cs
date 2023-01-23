using Ecs.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace Ecs.Systems
{
    public class StartLevelSystem : IEcsRunSystem, IEcsInitSystem
    {
        private EcsFilter _filter;
        
        public void Init(IEcsSystems systems)
        {
            _filter = systems.GetWorld().Filter<PlayerComponent>().Inc<StartLevelComponent>().End();
        }
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                Pool.World.AddComponentToEntity<CanMoveComponent>(entity);
                Pool.World.AddComponentToEntity<CanSpawnComponent>(entity);
                Pool.World.AddComponentToEntity<ElapsedSinceBlockBlockSpawn>(entity);
                
                Pool.World.RemoveComponent<StartLevelComponent>(entity);
            }      
        }

    }
}