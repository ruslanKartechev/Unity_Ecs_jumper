using Ecs.Components;
using Ecs.Components.View;
using Leopotam.EcsLite;

namespace Ecs.Systems
{
    public class TransformMoveVS : IEcsRunSystem, IEcsInitSystem
    {
        private EcsFilter _filter;
        private EcsPool<TransformVC> _pool;
        private EcsPool<PositionComponent> _positionPool;

        
        public void Init(IEcsSystems systems)
        {
            _filter = systems.GetWorld().Filter<TransformVC>().Inc<PositionComponent>().End();
            _pool = systems.GetWorld().GetPool<TransformVC>();
            _positionPool = systems.GetWorld().GetPool<PositionComponent>();
        }
        
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                ref var comp = ref _pool.Get(entity);
                ref var posComp = ref _positionPool.Get(entity);
                comp.Body.position = posComp.Value;
                
            }
        }

     
    }
}