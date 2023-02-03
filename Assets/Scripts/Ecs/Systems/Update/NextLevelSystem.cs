using Ecs.Components;
using Leopotam.EcsLite;
using Services.Level;
using Zenject;

namespace Ecs.Systems
{
    public class NextLevelSystem : IEcsRunSystem, IEcsInitSystem
    {
        private EcsWorld _world;
        private EcsFilter _filter;
        private EcsPool<NextLevelComponent> _nextLevelPool;

        [Inject] private ILevelService _levelService;

        
        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _filter = _world.Filter<NextLevelComponent>().End();
            _nextLevelPool = _world.GetPool<NextLevelComponent>();
        }
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                _nextLevelPool.Del(entity);
                ref var comp = ref _world.AddComponentToNew<LoadLevelComponent>();
                comp.Index = _levelService.NextLevel();
            }
        }

    }
}