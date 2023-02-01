using Ecs.Components;
using Leopotam.EcsLite;
using UnityEngine;

namespace Ecs.Systems
{
    public class ScaleOnJumpSystem : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _filter;
        private EcsWorld _world;
        private float _scaleValue = 0.33f;
        private float _lerpValue = 0.1f;
        private EcsPool<LerpMoveComponent> _lerpMovePool;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _filter = _world.Filter<IsMovingComponent>()
                .Inc<LerpMoveComponent>()
                .Inc<LocalScaleComponent>().End();
            _lerpMovePool = _world.GetPool<LerpMoveComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                ref var lerpMove = ref _lerpMovePool.Get(entity);
                ref var scaleComp = ref _world.GetComponent<LocalScaleComponent>(entity);
                if (lerpMove.Value < _lerpValue)
                {
                    var scale = Mathf.Lerp(1f, _scaleValue, lerpMove.Value);
                    scaleComp.Value = new Vector3(scale, 1f, scale);
                }
                else
                {
                    var scale = Mathf.Lerp(_scaleValue, 1f, lerpMove.Value);
                    scaleComp.Value = new Vector3(scale, 1f, scale);   
                }
            }
        }
    }
}