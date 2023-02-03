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
            var blockCellPos = new Vector2Int(command.x, command.y);
            var blockEntity = EntityMaker.MakeBlockEntity(world, 
                spawnPosition, 
                viewInstance, 
                blockCellPos);
            AddMoveComponent(blockEntity, spawnPosition, blockCellPos);
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

        private void AddMoveComponent(int blockEntity, Vector3 spawnPosition, Vector2Int cellPos)
        {
            ref var dropMoveComponent = ref _world.AddComponentToEntity<MoveBlockComponent>(blockEntity);
            dropMoveComponent.EndPosition = spawnPosition;
            dropMoveComponent.Time = _dropTime;
            if (cellPos.y == 1 && cellPos.x == 0)
            {
                dropMoveComponent.StartPosition = spawnPosition + Vector3.left * _spawnUpOffset;
            }
            else if (cellPos.y == 1 && cellPos.x == 2)
            {
                dropMoveComponent.StartPosition = spawnPosition + Vector3.right * _spawnUpOffset;
            }
            else if (cellPos.y == 0 && cellPos.x == 1)
            {
                dropMoveComponent.StartPosition = spawnPosition + Vector3.back * _spawnUpOffset;
            }
            else if (cellPos.y == 2 && cellPos.x == 1)
            {
                dropMoveComponent.StartPosition = spawnPosition + Vector3.forward * _spawnUpOffset;
            }
            else if (cellPos.y == 0 && cellPos.x == 0) // corner left bottom
            {
                var random = UnityEngine.Random.Range(0f, 1f);
                if (random > 0.5f)
                    dropMoveComponent.StartPosition = spawnPosition + Vector3.left * _spawnUpOffset;
                else
                    dropMoveComponent.StartPosition = spawnPosition + Vector3.back * _spawnUpOffset;
            }
            else if (cellPos.y == 0 && cellPos.x == 2) // corner right bottom
            {
                var random = UnityEngine.Random.Range(0f, 1f);
                if (random > 0.5f)
                    dropMoveComponent.StartPosition = spawnPosition + Vector3.right * _spawnUpOffset;
                else
                    dropMoveComponent.StartPosition = spawnPosition + Vector3.back * _spawnUpOffset;
            }
            else if (cellPos.y == 2 && cellPos.x == 0) // corner left top
            {
                var random = UnityEngine.Random.Range(0f, 1f);
                if (random > 0.5f)
                    dropMoveComponent.StartPosition = spawnPosition + Vector3.left * _spawnUpOffset;
                else
                    dropMoveComponent.StartPosition = spawnPosition + Vector3.forward * _spawnUpOffset;
            }
            else if (cellPos.y == 2 && cellPos.x == 0) // corner right top
            {
                var random = UnityEngine.Random.Range(0f, 1f);
                if (random > 0.5f)
                    dropMoveComponent.StartPosition = spawnPosition + Vector3.right * _spawnUpOffset;
                else
                    dropMoveComponent.StartPosition = spawnPosition + Vector3.forward * _spawnUpOffset;
            }
            else
            {
                dropMoveComponent.StartPosition = spawnPosition + Vector3.up * _spawnUpOffset;
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