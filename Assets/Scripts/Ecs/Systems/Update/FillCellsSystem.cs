using Data;
using Data.Prefabs;
using Ecs.Components;
using Ecs.Components.View;
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
            _filter = systems.GetWorld().Filter<FillCellComponent>().End();
            _commandPool = systems.GetWorld().GetPool<FillCellComponent>();
            _cellTopsFilter = systems.GetWorld().GetPool<CellTopsComponent>();
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
            var instance = _instantiateService.Spawn<CellBlockView>(prefab.gameObject, _parentService.DefaultParent, spawnPosition);
            data.Position.y += instance.Height;   
        }
        
        public void SpawnDefaultBlock(int entity, EcsWorld world)
        {
            ref var tops = ref _cellTopsFilter.Get(Pool.MapEntity);
            ref var command = ref _commandPool.Get(entity);   
            var prefab = _prefabsRepository.GetPrefab<CellBlockView>(PrefabNames.DefaultBlock);
            ref var data = ref tops.Positions[command.x, command.y];
            var spawnPosition = data.Position;
            var instance = _instantiateService.Spawn<CellBlockView>(prefab.gameObject, _parentService.DefaultParent, spawnPosition);
            data.Position.y += instance.Height;

            var blockEntity = EntityMaker.MakeBlockEntity(world, spawnPosition, instance.transform);
            
            ref var dropMoveComponent = ref world.AddComponentToEntity<DropMoveComponent>(blockEntity);
            dropMoveComponent.EndPosition = spawnPosition;
            dropMoveComponent.StartPosition = spawnPosition + Vector3.up * _spawnUpOffset;
            dropMoveComponent.Time = _dropTime;
        }
        
        
    }
}