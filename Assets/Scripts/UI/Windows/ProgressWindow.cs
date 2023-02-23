using System;
using Ecs;
using Ecs.Components;
using Ecs.Systems;
using UI.Views;
using UnityEngine;

namespace UI.Windows
{
    public class ProgressWindow : IWindow
    {
        private ProgressWindowView _view;
        private bool _didSub;
        
        public ProgressWindow(ProgressWindowView view)
        {
            _view = view;
            _view.UpdatePlayerHeight(0);
            _view.InitControl(OnControlButton);
            ReactDataPool.PlayerHeight.SubOnChange((value) => _view.UpdatePlayerHeight(value) );
            ReactDataPool.Tier.SubOnChange(OnTierChange);
        }

        private void OnTierChange(int tier)
        {
            _view.UpdateTier($"TIER {tier}");
        }

        public void Open(bool animated = true, Action onDone = null)
        {
            if(animated) 
                _view.ShowView(onDone);
            else
            {
                _view.UpdateTier($"TIER {ReactDataPool.Tier.Value}");
                _view.IsOpen = true;
            }
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
        
        
        private void OnControlButton(ControlButton button)
        {
            ref var inputComp = ref Pool.World.AddComponentToEntity<MoveInputComponent>(Pool.PlayerEntity);
            switch (button)
            {
                case ControlButton.Up:
                    inputComp.Value = Vector2Int.up;
                    break;
                case ControlButton.Down:
                    inputComp.Value = Vector2Int.down;
                    break;
                case ControlButton.Right:
                    inputComp.Value = Vector2Int.right;
                    break;
                case ControlButton.Left:
                    inputComp.Value = Vector2Int.left;
                    break;
            }
        }

        
    }
}