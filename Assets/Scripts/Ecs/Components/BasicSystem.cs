using Leopotam.EcsLite;

namespace Ecs.Components
{
    public abstract class BasicSystem : IEcsInitSystem, IEcsRunSystem
    {
        protected EcsFilter _filter;
        protected EcsWorld _world;
        public abstract void Init(IEcsSystems systems);
        public abstract void Run(IEcsSystems systems);
    }
}