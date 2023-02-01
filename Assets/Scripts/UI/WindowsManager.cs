using UI.Windows;

namespace UI
{
    public class WindowsManager : IWindowsManager
    {
        private StartWindow _startWindow;
        private ProgressWindow _progressWindow;
        private FailWindow _failWindow;
        private WinWindow _winWindow;
        private BonusWindow _bonusWindow;
        private IWindow _activeWindow = null;
        
        public void Init(StartWindow startWindow,
                ProgressWindow progressWindow, 
                FailWindow failWindow, 
                WinWindow winWindow, 
                BonusWindow bonusWindow)
        {
            _startWindow = startWindow;
            _progressWindow = progressWindow;
            _failWindow = failWindow;
            _winWindow = winWindow;
            _bonusWindow = bonusWindow;
        }

        public void CloseAll()
        {
            _bonusWindow.Close(false);
            _startWindow.Close(false);
            _progressWindow.Close(false);    
            _failWindow.Close(false);
            _winWindow.Close(false);
        }
        
        public void ShowStart()
        {
            if (_activeWindow != null)
            {
                _activeWindow.Close(true, () => { _startWindow.Open();});
            }
            else
            {
                _startWindow.Open();
            }
            _activeWindow = _startWindow;
        }

        public void ShowProcess()
        {
            if (_activeWindow != null)
            {
                _activeWindow.Close(true, () => { _progressWindow.Open();});
            }
            else
            {
                _progressWindow.Open();
            }
            _activeWindow = _progressWindow;
        }

        public void ShowWin()
        {
            if (_activeWindow != null)
            {
                _activeWindow.Close(true,() => { _winWindow.Open();});
            }
            else
            {
                _winWindow.Open();
            }
            _activeWindow = _winWindow;

        }

        public void ShowFail()
        {
            if (_activeWindow != null)
            {
                _activeWindow.Close(true, () => { _failWindow.Open();});
            }
            else
            {
                _failWindow.Open();
            }
            _activeWindow = _failWindow;

        }

        public void ShowBonusWindow()
        {
            _bonusWindow.Open();
        }

        public void CloseBonusWindow()
        {
            _bonusWindow.Close();
        }
    }
}