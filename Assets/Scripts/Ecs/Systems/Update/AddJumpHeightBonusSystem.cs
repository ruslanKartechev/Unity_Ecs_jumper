using Ecs.Components;
using Leopotam.EcsLite;

namespace Ecs.Systems
{
    public class AddJumpHeightBonusSystem : IEcsRunSystem, IEcsInitSystem
    {
        private EcsFilter _countFilter;
        private EcsFilter _addFilter;
        private EcsWorld _world;
        private EcsPool<AddJumHeightBonusComponent> _setJumpHeightPool;
        private EcsPool<MaxJumpHeightComponent> _jumpHeightPool;
        private EcsPool<JumpsWithBonusCountComponent> _jumpWithBonusCountPool;

        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _countFilter = _world.Filter<JumpStartedComponent>().Inc<JumpsWithBonusCountComponent>().End();
            _addFilter = _world.Filter<AddJumHeightBonusComponent>()
                .Inc<MaxJumpHeightComponent>()
                .End();
            _setJumpHeightPool = _world.GetPool<AddJumHeightBonusComponent>();
            _jumpHeightPool = _world.GetPool<MaxJumpHeightComponent>();
            _jumpWithBonusCountPool = _world.GetPool<JumpsWithBonusCountComponent>();
        }
        
        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _addFilter)
            {
                ref var bonusComp = ref _setJumpHeightPool.Get(entity);
                ref var jumpHeight = ref _jumpHeightPool.Get(entity);
                jumpHeight.Value = bonusComp.Value;
                ref var bonusJumpCount = ref _world.AddComponentToEntity<JumpsWithBonusCountComponent>(entity);
                bonusJumpCount.Count = 0;
                bonusJumpCount.MaxCount = bonusComp.JumpCount;
                       
                _setJumpHeightPool.Del(entity);
            }

            foreach (var entity in _countFilter)
            {
                ref var comp = ref _jumpWithBonusCountPool.Get(entity);
                comp.Count++;
                // Dbg.LogGreen($"Jump count with bonus: {comp.Count}, max count: {comp.MaxCount}");
                if (comp.Count >= comp.MaxCount)
                {
                    _jumpWithBonusCountPool.Del(entity);
                    ref var jumpHeight = ref _jumpHeightPool.Get(entity);
                    jumpHeight.Value = 1f;
                }
            }
            
            

        }

 
    }
}