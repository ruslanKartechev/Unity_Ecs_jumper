using Data;
using Ecs.Components;
using Leopotam.EcsLite;
using Services.MonoHelpers;
using UI;
using Zenject;

namespace Ecs.Systems
{
    public class StartLevelSystem : IEcsRunSystem, IEcsInitSystem
    {
        [Inject] private IWindowsManager _windowsManager;
        private EcsFilter _filter;
        private EcsWorld _world;
        
        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _filter = _world.Filter<PlayerComponent>().Inc<StartLevelComponent>().End();
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
            }      
        }

    }
}