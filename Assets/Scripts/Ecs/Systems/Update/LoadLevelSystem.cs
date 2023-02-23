using Data;
using Ecs.Components;
using Ecs.Components.View;
using Game;
using Game.Level.Impl;
using Leopotam.EcsLite;
using Services.Instantiate;
using Services.Parent;
using UI;
using Zenject;

namespace Ecs.Systems
{
    public class LoadLevelSystem : IEcsRunSystem, IEcsInitSystem
    {
        private EcsWorld _world;
        private EcsFilter _filter;
        private EcsPool<LoadLevelComponent> _pool;
        
        [Inject] private IWindowsManager _windowsManager;
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
                EntityMaker.MakeLevelEntity(_world);

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
                spawnDelay.Tier = 1;
                
                ref var blocksCount = ref _world.GetComponent<BlocksCountComponent>(Pool.PlayerEntity);
                blocksCount.Value = 0;
                ReactDataPool.BlocksCount.Value = 0;
            
                ref var blockSpawnDelay = ref _world.GetComponent<BlockSpawnDataComponent>(Pool.PlayerEntity);
                blockSpawnDelay.Data = viewInstance.spawnDelayData;
                ResetStats();
                SetBonuses();
                ref var gameState = ref _world.GetComponent<GameStateComponent>(Pool.PlayerEntity);
                gameState.Value = EGameState.StartWindow;
                _world.AddComponentToNew<SpawnPlayerComponent>();

                ref var colors = ref _world.GetComponent<TearMatDataComponent>(Pool.MapEntity);
                colors.Data = viewInstance.tearBlockMatData;
                
                ref var numbersBlock = ref _world.GetComponent<NumbersBlockVC>(Pool.LevelEntity);
                numbersBlock.View = viewInstance.NumberBlock;

                _windowsManager.CloseAll();
                _windowsManager.ShowStart();
            }
        }

        
        
        private void SetBonuses()
        {
            ref var jumpHeightCount = ref _world.GetComponent<JumpHeightBonusCountComponent>(Pool.PlayerEntity);
            jumpHeightCount.Value = 3;
            ref var jumpToTopCount = ref _world.GetComponent<JumpToTopBonusCountComponent>(Pool.PlayerEntity);
            jumpToTopCount.Value = 2;   
        }
        
        private void ResetStats()
        {
            ReactDataPool.Tier.Value = 1;
            ReactDataPool.BlocksCount.Value = 0;
            ReactDataPool.MoveCount.Value = 0;
            ReactDataPool.PlayerHeight.Value = 0;
        }
        
    }
}