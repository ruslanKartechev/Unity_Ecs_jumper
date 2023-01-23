using Ecs.Components;
using Leopotam.EcsLite;
using Services.Level;
using Zenject;

namespace Ecs.Systems.Init
{
    public class InitGameSystem : IEcsInitSystem
    {
        [Inject] private ILevelService _levelService;
        
        public void Init(IEcsSystems systems)
        {
            var world = systems.GetWorld();
            EntityMaker.MakePlayerEntity(world);
            EntityMaker.MakeMapEntity(world);
            EntityMaker.MakeLevelEntity(world);
            ref var comp = ref world.AddComponentToNew<LoadLevelComponent>();
            comp.Index = _levelService.CurrentLevelIndex;
            world.AddComponentToEntity<StartLevelComponent>(Pool.PlayerEntity);
        }
        
    }
}