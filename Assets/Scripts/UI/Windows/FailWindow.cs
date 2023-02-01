using System;
using Game.Sound;
using Game.Sound.Data;
using Services.Level;
using UI.Views;
using Zenject;

namespace UI.Windows
{
    public class FailWindow : IWindow
    {
        [Inject] private ILevelService _levelService;
        [Inject] private ISoundManager _soundManager;

        private FailWindowView _view;
        private bool _clicked;
        public FailWindow(FailWindowView failWindow)
        {
            _view = failWindow;
            _view.InitButtons(OnReplayButton);
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
            _view.HeaderText = $"LEVEL {_levelService.TotalLevelsPassed + 1} FAILED";
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

        private void OnReplayButton()
        {
            if (_clicked)
                return;
            _clicked = true;
            var sound = _soundManager.PlaySound(new SoundPlayArgs()
            {
                name = SoundNames.UIClick,
                loop =  false,
                once = true
            });
        }
    }
}