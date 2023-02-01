using Data;
using Data.Prefabs;
using Ecs.Components;
using Leopotam.EcsLite;
using Services.Instantiate;
using Services.Parent;
using UnityEngine;
using View;
using Zenject;

namespace Ecs.Systems
{
    public class FillCellSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsFilter _filter;
        private EcsPool<FillCellComponent> _commandPool;
        private EcsPool<CellTopsComponent> _cellTopsFilter;

        [Inject] private IPrefabsRepository _prefabsRepository;
        [Inject] private IInstantiateService _instantiateService;
        [Inject] private IParentService _parentService;
        private float _spawnUpOffset = 2;
        private float _dropTime = 0.4f;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _filter = _world.Filter<FillCellComponent>().End();
            _commandPool = _world.GetPool<FillCellComponent>();
            _cellTopsFilter = _world.GetPool<CellTopsComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                ref var command = ref _commandPool.Get(entity);
                switch (command.Type)
                {
                    case BlockType.Ground:
                        SpawnBaseBlock(entity, systems.GetWorld());
                        break;
                    case BlockType.Default:
                        SpawnDefaultBlock(entity, systems.GetWorld());
                        break;
                }
                _commandPool.Del(entity);
            }
        }

        public void SpawnBaseBlock(int entity, EcsWorld world)
        {
            ref var tops = ref _cellTopsFilter.Get(Pool.MapEntity);
            ref var command = ref _commandPool.Get(entity);
            var prefab = _prefabsRepository.GetPrefab<CellBlockView>(PrefabNames.GroundBlock);
            ref var data = ref tops.Positions[command.x, command.y];
            var spawnPosition = data.Position;
            var viewInstance = _instantiateService.Spawn<CellBlockView>(prefab.gameObject, _parentService.DefaultParent, spawnPosition);
            data.Position.y += viewInstance.Height;   
        }
        
        public void SpawnDefaultBlock(int entity, EcsWorld world)
        {
            ref var tops = ref _cellTopsFilter.Get(Pool.MapEntity);
            ref var command = ref _commandPool.Get(entity);   
            var prefab = _prefabsRepository.GetPrefab<CellBlockView>(PrefabNames.DefaultBlock);
            ref var data = ref tops.Positions[command.x, command.y];
            var spawnPosition = data.Position;
            var viewInstance = _instantiateService.Spawn<CellBlockView>(prefab.gameObject, _parentService.DefaultParent, spawnPosition);
            data.Position.y += viewInstance.Height;

            var blockEntity = EntityMaker.MakeBlockEntity(world, 
                spawnPosition, 
                viewInstance, 
                new Vector2Int(command.x, command.y));
            
            ref var dropMoveComponent = ref world.AddComponentToEntity<DropMoveComponent>(blockEntity);
            dropMoveComponent.EndPosition = spawnPosition;
            dropMoveComponent.StartPosition = spawnPosition + Vector3.up * _spawnUpOffset;
            dropMoveComponent.Time = _dropTime;
            IncreaseCount();            

            if (_world.HasComponent<CheckBlockTransparencyComponent>(blockEntity))
            {
                ref var checkTransparency = ref world.AddComponentToEntity<CheckBlockTransparencyComponent>(blockEntity);
                checkTransparency.Height = world.GetComponent<PositionComponent>(Pool.PlayerEntity).Value.y;
                var playerCellPos = world.GetComponent<CellPositionComponent>(Pool.PlayerEntity);
                checkTransparency.xCellPos = playerCellPos.x;
                checkTransparency.yCellPos = playerCellPos.y;
            }
            
        }

        private void IncreaseCount()
        {
            ref var blocksCount = ref _world.GetComponent<BlocksCountComponent>(Pool.PlayerEntity);
            blocksCount.Value++;
            ReactDataPool.BlocksCount.Value++;
        }
    }
}