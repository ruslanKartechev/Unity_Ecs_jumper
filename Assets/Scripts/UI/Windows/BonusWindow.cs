using System;
using Ecs;
using Ecs.Components;
using Ecs.Systems;
using Helpers;
using UI.Views;

namespace UI.Windows
{
    public class BonusWindow : IWindow, IBonusWindow
    {
        private BonusWindowView _view;
        private float _bonusHeight = 2;
        
        public BonusWindow(BonusWindowView view)
        {
            _view = view;
            _view.Init(OnJumpHeightBonus, OnToTopBonus);
        }
        

        public void Open(bool animated = true, Action onDone = null)
        {
            if(animated) 
                _view.ShowView(onDone);
            else
            {
                _view.IsOpen = true;
            }
            UpdateBonusCounts();
        }

        private void UpdateBonusCounts()
        {
            _view.SetJumpHeightBonusCount(Pool.World.GetComponent<JumpHeightBonusCountComponent>(Pool.PlayerEntity).Value);
            _view.SetJumpToTopBonusCount(Pool.World.GetComponent<JumpToTopBonusCountComponent>(Pool.PlayerEntity).Value);
        }

        private void OnToTopBonus()
        {
            if (Pool.World.HasComponent<AddJumpToTopBonusComponent>(Pool.PlayerEntity))
            {
                Dbg.Log($"Already added this bonus");
                return;
            }
            ref var countComp = ref Pool.World.GetComponent<JumpToTopBonusCountComponent>(Pool.PlayerEntity);
            if (countComp.Value <= 0)
                return;
            ref var comp = ref Pool.World.AddComponentToEntity<AddJumpToTopBonusComponent>(Pool.PlayerEntity);
            countComp.Value--;
            _view.SetJumpToTopBonusCount(Pool.World.GetComponent<JumpToTopBonusCountComponent>(Pool.PlayerEntity).Value);
        }

        private void OnJumpHeightBonus()
        {
            if (Pool.World.HasComponent<AddJumHeightBonusComponent>(Pool.PlayerEntity))
            {
                Dbg.Log($"Already added this bonus");
                return;
            }

            if (Pool.World.HasComponent<JumpsWithBonusCountComponent>(Pool.PlayerEntity))
                return;
            
            ref var countComp = ref Pool.World.GetComponent<JumpHeightBonusCountComponent>(Pool.PlayerEntity);
            if (countComp.Value <= 0)
                return;
            ref var comp = ref Pool.World.AddComponentToEntity<AddJumHeightBonusComponent>(Pool.PlayerEntity);
            comp.Value = _bonusHeight;
            countComp.Value--;
            _view.SetJumpHeightBonusCount(Pool.World.GetComponent<JumpHeightBonusCountComponent>(Pool.PlayerEntity).Value);
        }
        
        public void Close(bool animated = true, Action onDone = null)
        {
            if(animated)
                _view.CloseView(onDone);
            else
            {
                _view.IsOpen = false;
            }
        }


        public void HighlightJumpHeightBonus() => _view.HighlightJumpHeightBonus();

        public void HighlightJumpToTopBonus() => _view.HighlightJumpToTopBonus();
    }
}