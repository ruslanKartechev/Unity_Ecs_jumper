using Ecs.Components;
using Leopotam.EcsLite;
using Services.Time;
using UnityEngine;
using Zenject;

namespace Ecs.Systems
{
    public class DropMoveSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsWorld _world;
        [Inject] private ITimeService _timeService;
        private float _moveSpeedMod = 4;
        
        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _filter = systems.GetWorld().Filter<DropMoveComponent>().Inc<PositionComponent>().End();
        }

        public void Run(IEcsSystems systems)
        {
            
            foreach (var entity in _filter)
            {
                ref var moveComp = ref _world.GetComponent<DropMoveComponent>(entity);
                var t = moveComp.ElapsedTime / moveComp.Time;
                moveComp.ElapsedTime += _timeService.DeltaTime * Mathf.Pow(_moveSpeedMod, t);
                ref var posComp = ref _world.GetComponent<PositionComponent>(entity);
                posComp.Value = Vector3.Lerp(moveComp.StartPosition, moveComp.EndPosition,
                    moveComp.ElapsedTime / moveComp.Time);
                if (t >= 1)
                {
                    _world.RemoveComponent<DropMoveComponent>(entity);
                }
            }
        }
    }
}