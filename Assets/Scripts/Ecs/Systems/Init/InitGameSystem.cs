using Ecs.Components;
using Leopotam.EcsLite;
using Services.Level;
using UI;
using Zenject;

namespace Ecs.Systems.Init
{
    public class InitGameSystem : IEcsInitSystem
    {
        [Inject] private ILevelService _levelService;
        [Inject] private IWindowsManager _windowsManager;
        
        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            EntityMaker.MakePlayerEntity(world);
            EntityMaker.MakeMapEntity(world);
            EntityMaker.MakeLevelEntity(world);
            ref var comp = ref world.AddComponentToNew<LoadLevelComponent>();
            comp.Index = _levelService.CurrentLevelIndex;
            _windowsManager.CloseAll();
            _windowsManager.ShowStart();
            // world.AddComponentToEntity<StartLevelComponent>(Pool.PlayerEntity);
        }
        
    }
}