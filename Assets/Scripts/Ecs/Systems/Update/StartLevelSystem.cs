using Data;
using Ecs.Components;
using Ecs.Components.View;
using Leopotam.EcsLite;
using UI;
using Zenject;

namespace Ecs.Systems
{
    public class StartLevelSystem : IEcsRunSystem, IEcsInitSystem
    {
        [Inject] private IWindowsManager _windowsManager;
        private EcsPool<NumbersBlockVC> _numbersBlockPool;
        private EcsFilter _filter;
        private EcsWorld _world;
        
        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _filter = _world.Filter<PlayerComponent>().Inc<StartLevelComponent>().End();
            _numbersBlockPool = _world.GetPool<NumbersBlockVC>();
        }
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                _windowsManager.ShowProcess();
                _windowsManager.ShowBonusWindow();
                _world.AddComponentToEntity<CanMoveComponent>(entity);
                _world.AddComponentToEntity<CanSpawnComponent>(entity);
                _world.AddComponentToEntity<ElapsedSinceBlockBlockSpawn>(entity);
                _world.RemoveComponent<StartLevelComponent>(entity);
                ref var gameState = ref _world.GetComponent<GameStateComponent>(Pool.PlayerEntity);
                gameState.Value = EGameState.LevelPlay;
                ref var numbersBlock = ref _numbersBlockPool.Get(Pool.LevelEntity);
                numbersBlock.View.Show(true);
            }      
        }

 

    }
}