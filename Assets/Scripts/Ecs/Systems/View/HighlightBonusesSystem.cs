using Ecs.Components;
using Leopotam.EcsLite;
using UI.Windows;
using Zenject;

namespace Ecs.Systems
{
    public class HighlightBonusesSystem  : IEcsInitSystem, IEcsRunSystem
    {
        private EcsWorld _world;
        private EcsFilter _filterHeight;
        private EcsPool<HighlightJumpHeightBonusComponent> _heightPool;
        private EcsFilter _filterToTop;
        private EcsPool<HighlightJumpToTopBonusComponent> _toTopPool;
        
        [Inject] private IBonusWindow _bonusWindow;
        
        public void Init(IEcsSystems systems)
        {
            _world = systems.GetWorld();
            _filterHeight = _world.Filter<HighlightJumpHeightBonusComponent>().End();
            _heightPool = _world.GetPool<HighlightJumpHeightBonusComponent>();
            
            _filterToTop = _world.Filter<HighlightJumpToTopBonusComponent>().End();
            _toTopPool = _world.GetPool<HighlightJumpToTopBonusComponent>();
        }

        public void Run(IEcsSystems systems)
        {
            foreach (var entity in _filterHeight)
            {
                _heightPool.Del(entity);
                _bonusWindow.HighlightJumpHeightBonus();
            }
            
            foreach (var entity in _filterToTop)
            {
                _toTopPool.Del(entity);
                _bonusWindow.HighlightJumpToTopBonus();
            }
        }
    }
}