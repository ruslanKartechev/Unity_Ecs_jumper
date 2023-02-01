using Data;
using Ecs.Components;
using Game.Level.Impl;
using Leopotam.EcsLite;
using Services.Instantiate;
using Services.Parent;
using TMPro;
using Zenject;

namespace Ecs.Systems
{
    public class LoadLevelSystem : IEcsRunSystem, IEcsInitSystem
    {
        private EcsWorld _world;
        private EcsFilter _filter;
        private EcsPool<LoadLevelComponent> _pool;
        
        [Inject] private ILevelRepository _levelRepository;
        [Inject] private IInstantiateService _instantiateService;
        [Inject] private IParentService _parentService;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();

            _filter = systems.GetWorld().Filter<LoadLevelComponent>().End();
            _pool = systems.GetWorld().GetPool<LoadLevelComponent>();   
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                ref var component = ref _pool.Get(entity);
                var prefab = _levelRepository.GetLevel(component.Index);
                var viewInstance = _instantiateService.Spawn<LevelView>(prefab.gameObject);
                _parentService.DefaultParent = viewInstance.transform;
                
                _pool.Del(entity);
                
                ref var cellTops = ref _world.GetComponent<CellTopsComponent>(Pool.MapEntity);
                ref var map = ref _world.GetComponent<MapComponent>(Pool.MapEntity);
                map.Width = viewInstance.gridSize;
                map.Height = viewInstance.gridSize;

                cellTops.Positions = new CellTopsData[viewInstance.gridSize, viewInstance.gridSize];
                var index = 0;
                for (var i = 0; i < viewInstance.gridSize; i++)
                {
                    for (var k = 0; k < viewInstance.gridSize; k++)
                    {
                        ref var data = ref cellTops.Positions[i, k];
                        data = new CellTopsData();
                        data.Position = viewInstance._spawnPoints[index].position;
                        ref var command = ref _world.AddComponentToNew<FillCellComponent>();
                        command.x = i;
                        command.y = k;
                        command.Type = BlockType.Ground;
                        
                        index++;
                    }
                }

                ref var spawnDelay = ref _world.GetComponent<BlockSpawnDelayComponent>(Pool.PlayerEntity);
                spawnDelay.Value = viewInstance.spawnDelay;
                spawnDelay.Tier = 0;
                
                ref var blocksCount = ref _world.GetComponent<BlocksCountComponent>(Pool.PlayerEntity);
                blocksCount.Value = 0;
                ReactDataPool.BlocksCount.Value = 0;
            
                ref var blockSpawnDelay = ref _world.GetComponent<BlockSpawnDataComponent>(Pool.PlayerEntity);
                blockSpawnDelay.Data = viewInstance.spawnDelayData;
                
                SetBonuses();
                _world.AddComponentToNew<SpawnPlayerComponent>();
            }
        }

        private void SetBonuses()
        {
            ref var jumpHeightCount = ref _world.GetComponent<JumpHeightBonusCountComponent>(Pool.PlayerEntity);
            jumpHeightCount.Value = 5;
            ref var jumpToTopCount = ref _world.GetComponent<JumpToTopBonusCountComponent>(Pool.PlayerEntity);
            jumpToTopCount.Value = 4;   
        }
        
        
    }
}