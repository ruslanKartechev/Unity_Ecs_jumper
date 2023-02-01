using Ecs.Components;
using Helpers;
using Leopotam.EcsLite;
using UnityEngine;

namespace Ecs.Systems
{
    public class SetPlayerMoveDestinationSystem : IEcsRunSystem, IEcsInitSystem
    {
        private EcsFilter _filter;
        private EcsWorld _world;
        private EcsPool<MoveInputComponent> _moveInputPool;
        private EcsPool<CellPositionComponent> _cellPositionsPool;
        private EcsPool<IsMovingComponent> _isMovingPool;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _filter = _world.Filter<PlayerComponent>().Inc<MoveInputComponent>().Inc<CellPositionComponent>().Inc<CanMoveComponent>().Exc<IsMovingComponent>().End();
            _cellPositionsPool = _world.GetPool<CellPositionComponent>();
            // _isMovingPool = _world.GetPool<IsMovingComponent>();
            _moveInputPool = _world.GetPool<MoveInputComponent>();
        }
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                var inputComp = _moveInputPool.Get(Pool.PlayerEntity);
                _moveInputPool.Del(entity);
                ref var cellPosComponent = ref _cellPositionsPool.Get(entity);
                ref var map = ref _world.GetComponent<MapComponent>(Pool.MapEntity);
                var cell_x = cellPosComponent.x;
                var cell_y = cellPosComponent.y;
                cell_x += inputComp.Value.x;
                cell_y += inputComp.Value.y;
                var validMove = true;
                if (cell_x >= map.Width || cell_x < 0)
                    validMove = false;
                if (cell_y >= map.Height || cell_y < 0)
                    validMove = false;
                _world.AddComponentToNew<CheckPotentialMoveComponent>();
                if (validMove)
                {
                    var playerPos = _world.GetComponent<PositionComponent>(entity).Value;
                    var maxHeight = _world.GetComponent<MaxJumpHeightComponent>(entity).Value;
                    var endPos = MapHelpers.GetPositionAtCell(cell_x, cell_y);
                    var diff = endPos.y - playerPos.y;
                    if (diff > maxHeight)
                    {
                        SetInPlaceJump(entity, _world);
                        return;
                    }
                    
                    #region StartMoving
                    ref var moveComp = ref _world.AddComponentToEntity<LerpMoveComponent>(entity);
                    moveComp.StartPosition = playerPos;
                    moveComp.EndPosition = endPos;
                    cellPosComponent.x = cell_x;
                    cellPosComponent.y = cell_y;
                    _world.AddComponentToEntity<IsMovingComponent>(entity);
                    _world.AddComponentToEntity<JumpStartedComponent>(entity);
                    #endregion
                    
                    #region AddJumpCount
                    ref var moveCountComp = ref _world.GetComponent<JumpCountComponent>(entity);
                    moveCountComp.Value++;
                    ReactDataPool.MoveCount.Value = moveCountComp.Value;
                    #endregion
                    
                    #region BlockComponentTransparency
                    var blocks = _world.Filter<BlockComponent>().End();
                    foreach (var block in blocks)
                    {
                        ref var checkTransparency  = ref _world.AddComponentToEntity<CheckBlockTransparencyComponent>(block);
                        checkTransparency.xCellPos = cell_x;
                        checkTransparency.yCellPos = cell_y;
                        checkTransparency.Height = endPos.y;            
                    }
                    #endregion
            
                }
                else
                {
                    SetInPlaceJump(entity, _world);
                }
            }
        }

        private void SetInPlaceJump(int entity, EcsWorld world)
        {
            var playerPos = world.GetComponent<PositionComponent>(entity).Value;
            ref var moveComp_noMove = ref world.AddComponentToEntity<LerpMoveComponent>(entity);
            moveComp_noMove.StartPosition = playerPos;
            moveComp_noMove.EndPosition = playerPos;
            world.AddComponentToEntity<IsMovingComponent>(entity);

        }
    }
}