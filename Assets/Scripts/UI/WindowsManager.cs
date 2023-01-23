using UI.Windows;

namespace UI
{
    public class WindowsManager : IWindowsManager
    {
        // [Inject] 
        private StartWindow _startWindow;
        // [Inject] 
        private ProgressWindow _progressWindow;
        // [Inject] 
        private FailWindow _failWindow;
        // [Inject] 
        private WinWindow _winWindow;
        private IWindow _activeWindow = null;
        
        
        public void Init(StartWindow startWindow, ProgressWindow progressWindow, FailWindow failWindow, WinWindow winWindow)
        {
            _startWindow = startWindow;
            _progressWindow = progressWindow;
            _failWindow = failWindow;
            _winWindow = winWindow;
        }

        public void CloseAll()
        {
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
            // Dbg.Log("show process");
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
    }
}