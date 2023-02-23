using Ecs.Components;
using Ecs.Components.View;
using Leopotam.EcsLite;

namespace Ecs.Systems
{
    public class UpdateMaterialVS : IEcsInitSystem, IEcsRunSystem
    {
        private EcsFilter _blockFilter;
        private EcsFilter _commandFilter;
        private EcsWorld _world;
        
        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _commandFilter = _world.Filter<UpdateMaterialComponent>()
                .Inc<MainMaterialVC>()
                .Inc<TransparentMaterialVC>()
                .Inc<RendererVC>()
                .End();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _commandFilter)
            {
                var renderer = _world.GetComponent<RendererVC>(entity).Value;
                if (_world.HasComponent<IsTransparentComponent>(entity))
                    renderer.sharedMaterial =  _world.GetComponent<TransparentMaterialVC>(entity).Value;
                else
                    renderer.sharedMaterial = _world.GetComponent<MainMaterialVC>(entity).Value;       
                _world.RemoveComponent<UpdateMaterialComponent>(entity);
            }
        }
    }
}