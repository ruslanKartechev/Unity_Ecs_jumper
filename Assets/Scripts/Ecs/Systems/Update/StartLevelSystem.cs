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
        
        public void Init(IEcsSystems systems)
        {
            _filter = systems.GetWorld().Filter<PlayerComponent>().Inc<StartLevelComponent>().End();
        }
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                _windowsManager.ShowProcess();
                _windowsManager.ShowBonusWindow();
                Pool.World.AddComponentToEntity<CanMoveComponent>(entity);
                Pool.World.AddComponentToEntity<CanSpawnComponent>(entity);
                Pool.World.AddComponentToEntity<ElapsedSinceBlockBlockSpawn>(entity);
                
                Pool.World.RemoveComponent<StartLevelComponent>(entity);
            }      
        }

    }
}