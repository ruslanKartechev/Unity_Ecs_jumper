using System.IO;
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
        private EcsWorld _world;
        [Inject] private ITimeService _timeService;
        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
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
                var speed = _world.GetComponent<MoveSpeedComponent>(entity).Value;
                ref var posComponent = ref _world.GetComponent<PositionComponent>(entity);
                var lookAtPos = _world.GetComponent<LookAtPosition>(entity).Value;
                lookAtPos.x = 0;
                lookAtPos.z *= 0.5f;
                var offset = _world.GetComponent<OffsetComponent>(entity).Value;
                var camPosition = new Vector3(posComponent.Value.x, posComponent.Value.y, posComponent.Value.z);
                posComponent.Value = Vector3.Lerp(  camPosition, lookAtPos + offset,  speed * _timeService.DeltaTime);
            }
        }

    }
}