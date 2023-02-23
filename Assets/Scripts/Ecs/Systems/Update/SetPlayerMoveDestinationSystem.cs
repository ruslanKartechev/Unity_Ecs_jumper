using System.Collections.Generic;
using Ecs.Components;
using Ecs.Components.View;
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
        private EcsPool<MapComponent> _mapPool;
        private EcsPool<CheckBlockTransparencyComponent> _checkTransparencyPool;

        private float _spinChance = 1f;
        private float _spinTime = 0.3f;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _filter = _world.Filter<PlayerComponent>().Inc<MoveInputComponent>().Inc<CellPositionComponent>().Inc<CanMoveComponent>().Exc<IsMovingComponent>().End();
            _cellPositionsPool = _world.GetPool<CellPositionComponent>();
            _mapPool = _world.GetPool<MapComponent>();
            _moveInputPool = _world.GetPool<MoveInputComponent>();
            _checkTransparencyPool = _world.GetPool<CheckBlockTransparencyComponent>();
        }
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                var inputComp = _moveInputPool.Get(Pool.PlayerEntity);
                _moveInputPool.Del(entity);
                ref var cellPosComponent = ref _cellPositionsPool.Get(entity);
                ref var map = ref _mapPool.Get(Pool.MapEntity);
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
                    
                    Move(entity, playerPos, endPos, cell_x, cell_y);
                    
                    ref var height = ref _world.GetComponent<CurrentHeightComponent>(entity);
                    height.Value = endPos.y;
                    ReactDataPool.PlayerHeight.Value = height.Value;
                    
                    #region AddJumpCount
                    ref var moveCountComp = ref _world.GetComponent<JumpCountComponent>(entity);
                    moveCountComp.Value++;
                    ReactDataPool.MoveCount.Value = moveCountComp.Value;
                    #endregion
                    
                    #region BlockComponentTransparency
                    foreach (var block in map.AllAboveBlocks)
                    {
                        if(_checkTransparencyPool.Has(block))
                            continue;
                        ref var checkTransparency  = ref _checkTransparencyPool.Add(block);
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
        

        private void Move(int entity, Vector3 playerPos, Vector3 endPos, int cell_x, int cell_y)
        {
            ref var moveComp = ref _world.AddComponentToEntity<LerpMoveComponent>(entity);
            moveComp.StartPosition = playerPos;
            moveComp.EndPosition = endPos;
            ref var cellPosComponent = ref _cellPositionsPool.Get(entity);
            cellPosComponent.x = cell_x;
            cellPosComponent.y = cell_y;
            _world.AddComponentToEntity<IsMovingComponent>(entity);
            _world.AddComponentToEntity<JumpStartedComponent>(entity);
            SetHeight(entity, endPos.y);
            MakeRotate(entity);
        }

        private void MakeRotate(int entity)
        {
            var rotVar = UnityEngine.Random.Range(0f, 1f);
            if (rotVar <= _spinChance)
            {
                if(_world.HasComponent<RotateFromToComponent>(entity))
                    _world.RemoveComponent<RotateFromToComponent>(entity);
                ref var rotFromToComp = ref _world.AddComponentToEntity<RotateFromToComponent>(entity);
                float y = 360;
                var randomY = UnityEngine.Random.Range(-1f, 1f);
                if (randomY < 0)
                    y = -360;
                rotFromToComp.From = new Vector3(0f, y,0f);
                rotFromToComp.To = Vector3.one;
                rotFromToComp.Time = _spinTime;
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

        private void SetHeight(int entity, float height)
        {
            ref var heightComp = ref _world.GetComponent<MaxReachedHeightComponent>(entity);
            if (height > heightComp.Value)
            {
                heightComp.Value = height;
                ref var passedHeights = ref _world.GetComponent<PassedHeightsVC>(Pool.LevelEntity);
                passedHeights.Values.Add(height);
                ref var heightsBlock = ref _world.GetComponent<NumbersBlockVC>(Pool.LevelEntity);
                heightsBlock.View.Highlight(height);
            }
        }
                
        
    }
}