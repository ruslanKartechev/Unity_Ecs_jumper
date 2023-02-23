using Ecs.Components;
using Leopotam.EcsLite;
using Services.Time;
using UnityEngine;
using Zenject;

namespace Ecs.Systems
{
    public class RotateFromToSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _moveFilter;
        private EcsWorld _world;
        
        private EcsPool<RotateFromToComponent> _rotFromToPool;
        private EcsPool<RotationComponent> _rotPool;
        [Inject] private ITimeService _timeService;
        
        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _rotFromToPool = _world.GetPool<RotateFromToComponent>();
            _rotPool = _world.GetPool<RotationComponent>();
            _moveFilter = systems.GetWorld().Filter<RotateFromToComponent>().Inc<RotationComponent>().End();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _moveFilter)
            {
                ref var fromToRot = ref _rotFromToPool.Get(entity);
                var t = fromToRot.Elapsed / fromToRot.Time;
                var eulers = Vector3.Lerp(fromToRot.From, fromToRot.To, t);
                
                ref var rotComp = ref _rotPool.Get(entity);
                rotComp.Value = Quaternion.Euler(eulers);
                if (t >= 1)
                {
                    _rotFromToPool.Del(entity);
                }
                fromToRot.Elapsed += _timeService.DeltaTime;
            }   
        }
        
    }
}