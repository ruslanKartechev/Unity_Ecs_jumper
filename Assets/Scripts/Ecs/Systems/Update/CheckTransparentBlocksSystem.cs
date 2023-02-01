using Ecs.Components;
using Leopotam.EcsLite;

namespace Ecs.Systems
{
    public class CheckTransparentBlocksSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _blockFilter;
        private EcsWorld _world;
        private float _threshold = 1f;
        
        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _blockFilter = _world.Filter<CheckBlockTransparencyComponent>().Inc<BlockComponent>().Inc<CellPositionComponent>().Inc<PositionComponent>().End();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var block in _blockFilter)
            {
                var command = _world.GetComponent<CheckBlockTransparencyComponent>(block);
                _world.RemoveComponent<CheckBlockTransparencyComponent>(block);
                // var playerHeight = command.Height;
                // var playerCellY = command.yCellPos;
                var height = 0f;
                if (_world.HasComponent<DropMoveComponent>(block))
                {
                    height = _world.GetComponent<DropMoveComponent>(block).EndPosition.y;   
                }
                else
                {
                    height = _world.GetComponent<PositionComponent>(block).Value.y;   
                }
                
                if (height >= command.Height)
                {
                    var blockCellPos = _world.GetComponent<CellPositionComponent>(block);
                    if (blockCellPos.y >= command.yCellPos)
                    {
                        SetMainMat(block);
                        continue;
                    }
                    
                    if (_world.HasComponent<IsTransparentComponent>(block) == false)
                    {
                        _world.AddComponentToEntity<IsTransparentComponent>(block);
                        _world.AddComponentToEntity<UpdateMaterialComponent>(block);             
                    }
                }
                else
                    SetMainMat(block);
            }
        }

        private void SetMainMat(int block)
        {
            if (_world.HasComponent<IsTransparentComponent>(block))
            {
                _world.RemoveComponent<IsTransparentComponent>(block);
                _world.AddComponentToEntity<UpdateMaterialComponent>(block);
            }
        }
    }
}