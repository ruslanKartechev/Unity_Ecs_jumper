using Ecs.Components;
using Leopotam.EcsLite;
using Services.Time;
using UnityEngine;
using Zenject;

namespace Ecs.Systems
{
    public class MoveCameraSystem : IEcsRunSystem, IEcsInitSystem
    {
        private EcsFilter _filter;
        [Inject] private ITimeService _timeService;
        public void Init(IEcsSystems systems)
        {
            _filter = systems.GetWorld().Filter<CameraComponent>()
                .Inc<PositionComponent>()
                .Inc<LookAtPosition>()
                .Inc<MoveSpeedComponent>()
                .Inc<OffsetComponent>()
                .End();
        }
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                var world = systems.GetWorld();
                var speed = world.GetComponent<MoveSpeedComponent>(entity).Value;
                ref var posComponent = ref world.GetComponent<PositionComponent>(entity);
                var lookAtPos = world.GetComponent<LookAtPosition>(entity).Value;
                var offset = world.GetComponent<OffsetComponent>(entity).Value;

                var camPosition = posComponent.Value;
                camPosition.y = Mathf.Lerp(posComponent.Value.y, lookAtPos.y + offset, speed * _timeService.DeltaTime);
                posComponent.Value = camPosition;
            }
        }

    }
}