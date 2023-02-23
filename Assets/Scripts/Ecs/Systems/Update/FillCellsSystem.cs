using Data;
using Data.Prefabs;
using Ecs.Components;
using Helpers;
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
        private EcsPool<CellTopsComponent> _cellTopsPool;
        private EcsPool<CheckBlockTransparencyComponent> _checkTransparencyPool;
        private EcsPool<MapComponent> _mapPool;
        private EcsPool<PositionComponent> _positionPool;
        private EcsPool<IsTransparentComponent> _isTransparentPool;

        [Inject] private IPrefabsRepository _prefabsRepository;
        [Inject] private IInstantiateService _instantiateService;
        [Inject] private IParentService _parentService;
        private float _spawnUpOffset = 2;
        private float _dropTime = 0.4f;
        private float _rotTime = 0.15f;
        private float _rotationChance = 0.7f;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _filter = _world.Filter<FillCellComponent>().End();
            _commandPool = _world.GetPool<FillCellComponent>();
            _cellTopsPool = _world.GetPool<CellTopsComponent>();
            _checkTransparencyPool = _world.GetPool<CheckBlockTransparencyComponent>();
            _mapPool = _world.GetPool<MapComponent>();
            _positionPool = _world.GetPool<PositionComponent>();
            _isTransparentPool = _world.GetPool<IsTransparentComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                ref var command = ref _commandPool.Get(entity);
                switch (command.Type)
                {
                    case BlockType.Ground:
                        SpawnBaseBlock(entity);
                        break;
                    case BlockType.Default:
                        SpawnDefaultBlock(entity);
                        break;
                    case BlockType.Breakable:
                        SpawnBreakableBlock(entity);
                        break;
                }
                _commandPool.Del(entity);
            }
        }

        private void SpawnBreakableBlock(int commandEntity)
        {
            ref var tops = ref _cellTopsPool.Get(Pool.MapEntity);
            ref var command = ref _commandPool.Get(commandEntity);   
            var prefab = _prefabsRepository.GetPrefab<BreakableBlockView>(PrefabNames.BreakableBlock);
 
            ref var data = ref tops.Positions[command.x, command.y];
            var spawnPosition = data.Position;
            var viewInstance = _instantiateService.Spawn<BreakableBlockView>(prefab.gameObject, _parentService.DefaultParent, spawnPosition);
            data.Position.y += viewInstance.Height;
            var blockCellPos = new Vector2Int(command.x, command.y);
            var blockEntity = EntityMaker.MakeBreakableBlock(_world, 
                spawnPosition, 
                viewInstance, 
                blockCellPos);
            AddMoveComponent(blockEntity, spawnPosition, blockCellPos);
            IncreaseCount();
            AddRotationComponent(blockEntity);
            AddBlocksToList(blockEntity);
            
            // if (_checkTransparencyPool.Has(blockEntity) == false)
            // {
            //     ref var checkTransparency = ref _world.AddComponentToEntity<CheckBlockTransparencyComponent>(blockEntity);
            //     checkTransparency.Height = _world.GetComponent<PositionComponent>(Pool.PlayerEntity).Value.y;
            //     var playerCellPos = _world.GetComponent<CellPositionComponent>(Pool.PlayerEntity);
            //     checkTransparency.xCellPos = playerCellPos.x;
            //     checkTransparency.yCellPos = playerCellPos.y;
            // }
        }
        
        private void SpawnBaseBlock(int entity)
        {
            ref var tops = ref _cellTopsPool.Get(Pool.MapEntity);
            ref var command = ref _commandPool.Get(entity);
            var prefab = _prefabsRepository.GetPrefab<CellBlockView>(PrefabNames.GroundBlock);
            ref var data = ref tops.Positions[command.x, command.y];
            var spawnPosition = data.Position;
            var viewInstance = _instantiateService.Spawn<CellBlockView>(prefab.gameObject, _parentService.DefaultParent, spawnPosition);
            data.Position.y += viewInstance.Height;   
        }
        
        private void SpawnDefaultBlock(int commandEntity)
        {
            ref var tops = ref _cellTopsPool.Get(Pool.MapEntity);
            ref var command = ref _commandPool.Get(commandEntity);   
            var prefab = _prefabsRepository.GetPrefab<CellBlockView>(PrefabNames.DefaultBlock);
            ref var data = ref tops.Positions[command.x, command.y];
            var spawnPosition = data.Position;
            var viewInstance = _instantiateService.Spawn<CellBlockView>(prefab.gameObject, _parentService.DefaultParent, spawnPosition);
            data.Position.y += viewInstance.Height;
            var blockCellPos = new Vector2Int(command.x, command.y);
            var blockEntity = EntityMaker.MakeBlockEntity(_world, 
                spawnPosition, 
                viewInstance, 
                blockCellPos);
            AddMoveComponent(blockEntity, spawnPosition, blockCellPos);
            IncreaseCount();
            AddRotationComponent(blockEntity);
            AddBlocksToList(blockEntity);
            
            if (_checkTransparencyPool.Has(blockEntity) == false)
            {
                ref var checkTransparency = ref _world.AddComponentToEntity<CheckBlockTransparencyComponent>(blockEntity);
                checkTransparency.Height = _world.GetComponent<PositionComponent>(Pool.PlayerEntity).Value.y;
                var playerCellPos = _world.GetComponent<CellPositionComponent>(Pool.PlayerEntity);
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
        
        private void AddRotationComponent(int entity)
        {
            var chance = UnityEngine.Random.Range(0f, 1f);
            if (chance >= 1f - _rotationChance)
            {
                ref var fromTo = ref _world.AddComponentToEntity<RotateFromToComponent>(entity);
                var from = Quaternion.Euler(0f, 90f, 0f);
                var to = Quaternion.identity;
                fromTo.From = new Vector3(0f, 90f, 0f);
                fromTo.To = Vector3.one;
                fromTo.Time = _rotTime;
            }
        }

        private void IncreaseCount()
        {
            ref var blocksCount = ref _world.GetComponent<BlocksCountComponent>(Pool.PlayerEntity);
            blocksCount.Value++;
            ReactDataPool.BlocksCount.Value++;
        }

        private void AddBlocksToList(int entity)
        {
            ref var map = ref _mapPool.Get(Pool.MapEntity);
            map.AllBlocks.Add(entity);
            var lowestLevel = LowestHeight(ref map);
            if (lowestLevel > map.LowestBlockY)
            {
                // Dbg.LogRed($"New lowest: {lowestLevel}, old: {map.LowestBlockY}");
                map.LowestBlockY = lowestLevel;
                for (int i = map.AllAboveBlocks.Count - 1; i >= 0; i--)
                {
                    if (_positionPool.Get(map.AllAboveBlocks[i]).Value.y < lowestLevel)
                    {
                        if (_isTransparentPool.Has(map.AllAboveBlocks[i]))
                        {
                            _isTransparentPool.Del(map.AllAboveBlocks[i]);
                            _world.AddComponentToEntity<UpdateMaterialComponent>(map.AllAboveBlocks[i]);
                        }
                        map.AllAboveBlocks.RemoveAt(i);
                    }
                }
                // Debug.Log($"New above blocks count: {map.AllAboveBlocks.Count}");
            }
            else
            {
                map.AllAboveBlocks.Add(entity);   
            }
            
        }

        private float LowestHeight(ref MapComponent map)
        {
            ref var tops = ref _cellTopsPool.Get(Pool.MapEntity);
            var smallest = float.MaxValue;
            for (int i = 0; i < map.Width; i++)
            {
                for (int j = 0; j < map.Height; j++)
                {
                    var height = tops.Positions[i, j].Position.y;
                    if (height < smallest)
                        smallest = height;
                }
            }
            return smallest;
        }
    }
}