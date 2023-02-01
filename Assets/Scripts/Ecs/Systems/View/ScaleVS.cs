using Ecs.Components;
using Ecs.Components.View;
using Leopotam.EcsLite;

namespace Ecs.Systems
{
    public class ScaleVS : IEcsRunSystem, IEcsInitSystem
    {
        private EcsFilter _filter;
        private EcsPool<TransformVC> _transformPool;
        private EcsPool<LocalScaleComponent> _scalePool;
        private EcsWorld _world;
        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _filter = _world.Filter<TransformVC>().Inc<LocalScaleComponent>().End();
            _transformPool = _world.GetPool<TransformVC>();
            _scalePool = _world.GetPool<LocalScaleComponent>();
        }
        
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                ref var t = ref _transformPool.Get(entity);
                t.Body.localScale = _scalePool.Get(entity).Value;
            }   
        }
    }
}