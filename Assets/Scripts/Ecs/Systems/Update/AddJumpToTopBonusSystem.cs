using UnityEngine;
using Ecs.Components;
using Helpers;
using Leopotam.EcsLite;

namespace Ecs.Systems
{
    public class AddJumpToTopBonusSystem : IEcsRunSystem, IEcsInitSystem
    {
        private EcsWorld _world;
        private EcsFilter _filter;
        
        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _filter = _world.Filter<AddJumpToTopBonusComponent>().End();
        }
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                var topCell = FindTop();
                if (topCell.x == -1 || topCell.y == -1)
                {
                    Dbg.LogRed("cannot find top cell");
                    continue;
                }
                
                ref var cellPosComponent = ref _world.GetComponent<CellPositionComponent>(Pool.PlayerEntity);
                var playerPos = _world.GetComponent<PositionComponent>(entity).Value;
                var endPos = MapHelpers.GetPositionAtCell(topCell.x, topCell.y);

                #region StartMoving
                ref var moveComp = ref _world.AddComponentToEntity<LerpMoveComponent>(entity);
                moveComp.StartPosition = playerPos;
                moveComp.EndPosition = endPos;
                cellPosComponent.x = topCell.x;
                cellPosComponent.y = topCell.y;
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
                    checkTransparency.xCellPos = topCell.x;
                    checkTransparency.yCellPos = topCell.y;
                    checkTransparency.Height = endPos.y;            
                }
                #endregion
                
                _world.RemoveComponent<AddJumpToTopBonusComponent>(entity);
            }
        }


        private Vector2Int FindTop()
        {
            ref var tops = ref _world.GetComponent<CellTopsComponent>(Pool.MapEntity).Positions;
            var veryTop = 0f;
            var result = new Vector2Int(-1, -1);
            for (int i = 0; i < tops.GetLength(0); i++)
            {
                for (int k = 0; k < tops.GetLength(1); k++)
                {
                    var height = tops[i, k].Position.y;
                    if (height >= veryTop)
                    {
                        veryTop = height;
                        result.x = i;
                        result.y = k;
                    }
                }
            }
            return result;
        }
    }
}