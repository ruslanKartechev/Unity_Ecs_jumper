using Ecs.Components;
using Leopotam.EcsLite;
using UI;
using Zenject;

namespace Ecs.Systems
{
    public class FailLevelSystem : IEcsRunSystem, IEcsInitSystem
    {
        private EcsFilter _filter;
        private EcsWorld _world;
        private EcsPool<FailLevelComponent> _failPool;
        [Inject] private IWindowsManager _windowsManager;

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                _failPool.Del(entity);
                _windowsManager.ShowFail();
                Pool.World.RemoveComponent<CanMoveComponent>(Pool.PlayerEntity);
                Pool.World.RemoveComponent<CanSpawnComponent>(Pool.PlayerEntity);
            }   
        }

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _filter = _world.Filter<FailLevelComponent>().End();
            _failPool = _world.GetPool<FailLevelComponent>();
        }
    }
}