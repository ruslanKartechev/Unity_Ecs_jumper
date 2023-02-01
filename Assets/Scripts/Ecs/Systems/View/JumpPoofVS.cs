using Ecs.Components;
using Ecs.Components.View;
using Leopotam.EcsLite;

namespace Ecs.Systems
{
    public class JumpPoofVS : IEcsRunSystem, IEcsInitSystem
    {
        private EcsFilter _filter;
        private EcsWorld _world;
        
        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _filter = _world.Filter<JumpStartedComponent>().Inc<JumpParticlesVC>().End();
        }
       
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filter)
            {
                ref var particles = ref _world.GetComponent<JumpParticlesVC>(entity);
                particles.PooParticles.Play();
                _world.RemoveComponent<JumpStartedComponent>(entity);
            }

        }

    }
}