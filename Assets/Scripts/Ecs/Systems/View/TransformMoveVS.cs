using Ecs.Components;
using Ecs.Components.View;
using Leopotam.EcsLite;

namespace Ecs.Systems
{
    public class TransformMoveVS : IEcsRunSystem, IEcsInitSystem
    {
        private EcsFilter _filter;
        private EcsFilter _rotFilter;

        private EcsPool<TransformVC> _transformPool;
        private EcsPool<PositionComponent> _positionPool;
        private EcsPool<RotationComponent> _rotationPool;

        
        public void Init(IEcsSystems systems)
        {
            _filter = systems.GetWorld().Filter<TransformVC>().Inc<RotationComponent>().Inc<PositionComponent>().End();
            
            _transformPool = systems.GetWorld().GetPool<TransformVC>();
            _positionPool = systems.GetWorld().GetPool<PositionComponent>();
            _rotationPool = systems.GetWorld().GetPool<RotationComponent>();
        }
        
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                ref var transformComp = ref _transformPool.Get(entity);
                ref var posComp = ref _positionPool.Get(entity);
                transformComp.Body.position = posComp.Value;
                var rot = _rotationPool.Get(entity);
                transformComp.Body.rotation = rot.Value;
            }
        }

     
    }
}