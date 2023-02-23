using Ecs.Components;
using Leopotam.EcsLite;
using Services.Time;
using UnityEngine;
using Zenject;

namespace Ecs.Systems
{
    public class MoveLerpJumpSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsPool<LerpMoveComponent> _lerpMovePool;
        private EcsPool<VerticalOffsetComponent> _verticalOffsetPool;
        private EcsPool<MoveSpeedComponent> _moveSpeedPool;

        private EcsWorld _world;
        [Inject] private ITimeService _timeService;
        
        private float _inPlaceJumpHeight = 0.3f;
        private float _inPlaceJumpSpeedMod = 1.65f;
        private float _jumpDownSpeedMod = 2f;
        
        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _filter = _world.Filter<IsMovingComponent>()
                .Inc<LerpMoveComponent>()
                .Inc<MoveSpeedComponent>()
                .Inc<VerticalOffsetComponent>()
                .Inc<PositionComponent>()
                .Inc<LocalScaleComponent>().End();
            
            _lerpMovePool = _world.GetPool<LerpMoveComponent>();
            _verticalOffsetPool = _world.GetPool<VerticalOffsetComponent>();
            _moveSpeedPool = _world.GetPool<MoveSpeedComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                var world = systems.GetWorld();
                
                ref var lerpMoveComp = ref _lerpMovePool.Get(entity);
                if (lerpMoveComp.Value >= 1f)
                {
                    lerpMoveComp.Value = 0f;
        
                    world.RemoveComponent<IsMovingComponent>(entity);
                    world.RemoveComponent<LerpMoveComponent>(entity);
                    continue;
                }

                var speed = _moveSpeedPool.Get(entity).Value;
                var offsetComp = _verticalOffsetPool.Get(entity);
                if (lerpMoveComp.StartPosition == lerpMoveComp.EndPosition)
                {
                    var upPos = lerpMoveComp.StartPosition + Vector3.up * _inPlaceJumpHeight;
                    var position = lerpMoveComp.StartPosition;
                    if (lerpMoveComp.Value < 0.5f)
                        position = Vector3.Lerp(lerpMoveComp.StartPosition, upPos, lerpMoveComp.Value * 2);
                    else
                        position = Vector3.Lerp(upPos, lerpMoveComp.StartPosition, lerpMoveComp.Value * 2 - 1f);
                    ref var posComp = ref world.GetComponent<PositionComponent>(entity);
                    posComp.Value = position;
                    lerpMoveComp.Value += _timeService.DeltaTime * speed * _inPlaceJumpSpeedMod;
                }
                else
                {
   
                    var p2 = Vector3.Lerp(lerpMoveComp.StartPosition, lerpMoveComp.EndPosition, offsetComp.UpOffsetLerpValue)
                         + Vector3.up * offsetComp.UpOffset;
                    var position = Bezier(lerpMoveComp.StartPosition, p2, lerpMoveComp.EndPosition, lerpMoveComp.Value);
                    ref var posComp = ref world.GetComponent<PositionComponent>(entity);
                    posComp.Value = position;
                    lerpMoveComp.Value += _timeService.DeltaTime
                                          * speed
                                          * (lerpMoveComp.Value < 0.5 ? 1f : _jumpDownSpeedMod);
                }

            }
        }

        private Vector3 Bezier(Vector3 p1, Vector3 p2, Vector3 p3, float t)
        {
            return (1 - t) * (1 - t) * p1 + 2 * (1 - t) * t * p2 + t * t * p3;
        }
    }
}