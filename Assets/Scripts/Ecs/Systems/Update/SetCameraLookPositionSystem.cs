using Ecs.Components;
using Leopotam.EcsLite;

namespace Ecs.Systems
{
    public class SetCameraLookPositionSystem : IEcsRunSystem, IEcsInitSystem
    {
        private EcsFilter _filter;

        public void Init(IEcsSystems systems)
        {
            _filter = systems.GetWorld().Filter<PlayerComponent>().Inc<LerpMoveComponent>().End();
        }
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                var world = systems.GetWorld();
                var playerTargetPosition = world.GetComponent<LerpMoveComponent>(entity).EndPosition;
                ref var lookComp = ref world.GetComponent<LookAtPosition>(Pool.CameraEntity);
                lookComp.Value = playerTargetPosition;
            }
        }

    }
}