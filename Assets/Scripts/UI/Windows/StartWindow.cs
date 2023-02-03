using System;
using Ecs;
using Ecs.Components;
using Ecs.Systems;
using Game.Sound;
using Services.Level;
using UI.Views;
using Zenject;

namespace UI.Windows
{
    public class StartWindow : IWindow
    {
        private readonly StartWindowView _view;
        [Inject] private ILevelService _levelService;
        [Inject] private ISoundManager _soundManager;
        
        private bool _clicked;
        
        public StartWindow(StartWindowView view)
        {
            _view = view;
            _view.InitButtons(OnStartButton, OnSoundButton);
        }

        public void Open(bool animated = true, Action onDone = null)
        {
            _clicked = false;
            if (animated)
                _view.ShowView(onDone);
            else
            {
                _view.IsOpen = true;
            }
            _view.HeaderText = $"LEVEL {_levelService.TotalLevelsPassed + 1}";
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

        private void OnStartButton()
        {
            if (_clicked)
                return;
            _clicked = true;
            Pool.World.AddComponentToEntity<StartLevelComponent>(Pool.PlayerEntity);
            // _soundManager.PlaySound(new SoundPlayArgs()
            // {
            //     name = SoundNames.UIClick,
            //     loop =  false,
            //     once = true
            // });
        }

        private void OnSoundButton()
        {
            var state = _view.SoundButton.State;
            state = !state;
            _view.SoundButton.State = state;
            // var sound = _soundManager.PlaySound(new SoundPlayArgs()
            // {
            //     name = SoundNames.UIClick,
            //     loop =  false,
            //     once = true
            // });
        }
    }
}