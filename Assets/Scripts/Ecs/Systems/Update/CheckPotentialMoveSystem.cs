using Ecs.Components;
using Helpers;
using Leopotam.EcsLite;

namespace Ecs.Systems
{
    public class CheckPotentialMoveSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsFilter _filter;
        private float _maxHeightWithBonus = 2f;
        
        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _filter = _world.Filter<CheckPotentialMoveComponent>().End();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                _world.RemoveComponent<CheckPotentialMoveComponent>(entity);
                var cellPos = _world.GetComponent<CellPositionComponent>(Pool.PlayerEntity);
                var playerY = _world.GetComponent<PositionComponent>(Pool.PlayerEntity).Value.y;
                var maxHeight = _world.GetComponent<MaxJumpHeightComponent>(Pool.PlayerEntity).Value;
                var cellTops = _world.GetComponent<CellTopsComponent>(Pool.MapEntity);
                var map = _world.GetComponent<MapComponent>(Pool.MapEntity);

                var canMove = CheckPos(cellPos.x + 1, cellPos.y, ref cellTops, ref map, playerY, maxHeight)
                          || CheckPos(cellPos.x - 1, cellPos.y, ref cellTops, ref map, playerY, maxHeight)
                          || CheckPos(cellPos.x, cellPos.y + 1, ref cellTops, ref map, playerY, maxHeight)
                          || CheckPos(cellPos.x, cellPos.y - 1, ref cellTops, ref map, playerY, maxHeight);
                
                if (canMove == false)
                {
                    Dbg.Log($"CAN MOVE: false, playerY: {playerY}, max jump height: {maxHeight}");
                    
        
                    ref var heightBonusCount = ref Pool.World.GetComponent<JumpHeightBonusCountComponent>(Pool.PlayerEntity);
                    if (heightBonusCount.Value > 0)
                    {
                        var canMoveWithBonus = CheckPos(cellPos.x + 1, cellPos.y, ref cellTops, ref map, playerY, _maxHeightWithBonus)
                                      || CheckPos(cellPos.x - 1, cellPos.y, ref cellTops, ref map, playerY, _maxHeightWithBonus)
                                      || CheckPos(cellPos.x, cellPos.y + 1, ref cellTops, ref map, playerY, _maxHeightWithBonus)
                                      || CheckPos(cellPos.x, cellPos.y - 1, ref cellTops, ref map, playerY, _maxHeightWithBonus);
                        if (canMoveWithBonus)
                        {
                            _world.AddComponentToNew<HighlightJumpHeightBonusComponent>();
                            Dbg.LogGreen($"has bonus +jump height bonus to use!");
                            continue;
                        }
                        Dbg.LogRed("Cannot move with 2x bonus jump height");
                    }
                    
                    ref var jumpToTopBonusCount = ref Pool.World.GetComponent<JumpToTopBonusCountComponent>(Pool.PlayerEntity);
                    if (jumpToTopBonusCount.Value > 0)
                    {
                        Dbg.LogGreen($"has bonus to top bonus to use!");
                        _world.AddComponentToNew<HighlightJumpToTopBonusComponent>();
                        continue;
                    }
                    
                    Dbg.Log("no bonus options, fail level");
                    _world.AddComponentToNew<FailLevelComponent>();
                }   
            }
        }
        
        
        

        private bool CheckPos(int x, int y, ref CellTopsComponent tops, ref MapComponent map, float playerY, float maxHeight)
        {
            if (x >= map.Width || x < 0
                || y >= map.Height || y < 0)
                return false;

            var top = tops.Positions[x, y].Position.y;
            return playerY + maxHeight >= top;
        }
    }
}