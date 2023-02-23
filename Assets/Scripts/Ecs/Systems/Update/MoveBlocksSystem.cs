using Ecs.Components;
using Leopotam.EcsLite;
using Services.Time;
using UnityEngine;
using Zenject;

namespace Ecs.Systems
{
    public class MoveBlocksSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsWorld _world;
        private EcsPool<MoveBlockComponent> _dropPool;
        [Inject] private ITimeService _timeService;
        
        private float _moveSpeedMod = 3;
        
        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _dropPool = _world.GetPool<MoveBlockComponent>();
            _filter = systems.GetWorld().Filter<MoveBlockComponent>().Inc<PositionComponent>().End();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                ref var moveComp = ref _world.GetComponent<MoveBlockComponent>(entity);
                var t = moveComp.ElapsedTime / moveComp.Time;
                moveComp.ElapsedTime += _timeService.DeltaTime * Mathf.Pow(_moveSpeedMod, t + 1);
                
                ref var posComp = ref _world.GetComponent<PositionComponent>(entity);
                posComp.Value = Vector3.Lerp(moveComp.StartPosition, moveComp.EndPosition,
                    moveComp.ElapsedTime / moveComp.Time);
                
                if (t >= 1)
                    _dropPool.Del(entity);
            }
        }
    }
}