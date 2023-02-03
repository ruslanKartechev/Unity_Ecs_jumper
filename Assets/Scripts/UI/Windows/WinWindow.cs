using System;
using Ecs;
using Ecs.Components;
using Ecs.Systems;
using Game;
using Game.Sound;
using Game.Sound.Data;
using Services.Level;
using UI.Views;
using UnityEngine;
using Zenject;

namespace UI.Windows
{
    public class WinWindow : IWindow
    {
        private WinWindowView _view;
        [Inject] private ILevelService _levelService;
        [Inject] private ISoundManager _soundManager;

        private bool _clicked;

        public WinWindow(WinWindowView view)
        {
            _view = view;
            _view.InitButtons(OnNextLevelClick);
        }
        
        public void Open(bool animated = true, Action onDone = null)
        {
            _clicked = false;
            if(animated)
                _view.ShowView(onDone);
            else
            {
                _view.IsOpen = true;
            }
            _view.HeaderText = $"LEVEL {_levelService.TotalLevelsPassed + 1} COMPLETED";
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

        private void OnNextLevelClick()
        {
            if (_clicked)
                return;
            _clicked = true;
            Pool.World.AddComponentToNew<NextLevelComponent>();
            // var sound = _soundManager.PlaySound(new SoundPlayArgs()
            // {
            //     name = SoundNames.UIClick,
            //     loop =  false,
            //     once = true
            // });
        }
    }
}