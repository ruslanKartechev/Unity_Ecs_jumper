using Data;
using Ecs.Components;
using Game.Level.Impl;
using Leopotam.EcsLite;
using Services.Instantiate;
using Services.Parent;
using Zenject;

namespace Ecs.Systems
{
    public class LoadLevelSystem : IEcsRunSystem, IEcsInitSystem
    {
        private EcsFilter _filter;
        private EcsPool<LoadLevelComponent> _pool;
        
        [Inject] private ILevelRepository _levelRepository;
        [Inject] private IInstantiateService _instantiateService;
        [Inject] private IParentService _parentService;

        public void Init(IEcsSystems systems)
        {
            _filter = systems.GetWorld().Filter<LoadLevelComponent>().End();
            _pool = systems.GetWorld().GetPool<LoadLevelComponent>();   
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                ref var component = ref _pool.Get(entity);
                var prefab = _levelRepository.GetLevel(component.Index);
                var instance = _instantiateService.Spawn<LevelView>(prefab.gameObject);
                _parentService.DefaultParent = instance.transform;
                
                _pool.Del(entity);
                var world = systems.GetWorld();
                
                ref var cellTops = ref world.GetComponent<CellTopsComponent>(Pool.MapEntity);
                ref var map = ref world.GetComponent<MapComponent>(Pool.MapEntity);
                map.Width = instance.gridSize;
                map.Height = instance.gridSize;

                ref var spawnDelay = ref world.GetComponent<BlockSpawnDelayComponent>(Pool.LevelEntity);
                spawnDelay.Value = instance.spawnDelay;
                
                cellTops.Positions = new CellTopsData[instance.gridSize, instance.gridSize];
                var index = 0;
                for (var i = 0; i < instance.gridSize; i++)
                {
                    for (var k = 0; k < instance.gridSize; k++)
                    {
                        ref var data = ref cellTops.Positions[i, k];
                        data = new CellTopsData();
                        data.Position = instance._spawnPoints[index].position;
                        ref var command = ref world.AddComponentToNew<FillCellComponent>();
                        command.x = i;
                        command.y = k;
                        command.Type = BlockType.Ground;
                        
                        index++;
                    }
                }

                world.AddComponentToNew<SpawnPlayerComponent>();
                
;            }
        }
        
        
        
    }
}