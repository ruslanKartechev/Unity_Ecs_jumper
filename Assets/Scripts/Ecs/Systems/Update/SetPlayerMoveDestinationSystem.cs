using Ecs.Components;
using Helpers;
using Leopotam.EcsLite;
using UnityEngine;

namespace Ecs.Systems
{
    public class SetPlayerMoveDestinationSystem : IEcsRunSystem, IEcsInitSystem
    {
        private EcsFilter _filter;
        private EcsPool<MoveInputComponent> _moveInputPool;

        public void Init(IEcsSystems systems)
        {
            _filter = systems.GetWorld().Filter<PlayerComponent>().Inc<CellPositionComponent>().End();
            _moveInputPool = systems.GetWorld().GetPool<MoveInputComponent>();
        }
        
        public void Run(IEcsSystems systems)
        {
            var canMove = Pool.World.HasComponent<CanMoveComponent>(Pool.PlayerEntity);
            if (canMove == false)
                return;
            
            ref var input = ref _moveInputPool.Get(Pool.PlayerEntity);
            if (input.Value == Vector2.zero)
                return;
            
            foreach (var entity in _filter)
            {
                var world = systems.GetWorld();
                if (world.HasComponent<IsMovingComponent>(entity))
                    continue;
                
                ref var cellPosComponent = ref world.GetComponent<CellPositionComponent>(entity);
                ref var map = ref world.GetComponent<MapComponent>(Pool.MapEntity);
                var cell_x = cellPosComponent.x;
                var cell_y = cellPosComponent.y;
                cell_x += input.Value.x;
                cell_y += input.Value.y;
                var validMove = true;
                if (cell_x >= map.Width || cell_x < 0)
                    validMove = false;
                if (cell_y >= map.Height || cell_y < 0)
                    validMove = false;

                if (validMove)
                {
                    var playerPos = world.GetComponent<PositionComponent>(entity).Value;
                    var maxHeight = world.GetComponent<MaxJumpHeightComponent>(entity).Value;
                    var position = MapHelpers.GetPositionAtCell(cell_x, cell_y);
                    var diff = position.y - playerPos.y;
                    if (diff > maxHeight)
                    {
                        SetInPlaceJump(entity, world);
                        return;
                    }
                    ref var moveComp = ref world.AddComponentToEntity<LerpMoveComponent>(entity);
                    moveComp.StartPosition = playerPos;
                    moveComp.EndPosition = position;
                    cellPosComponent.x = cell_x;
                    cellPosComponent.y = cell_y;
                    world.AddComponentToEntity<IsMovingComponent>(entity);
                    world.AddComponentToEntity<JumpStartedComponent>(entity);

                }
                else
                {
                    SetInPlaceJump(entity, world);
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