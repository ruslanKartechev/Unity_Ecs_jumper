using System;
using UI.Views;

namespace UI.Windows
{
    public class ProgressWindow : IWindow
    {
        private ProgressWindowView _view;
        private bool _didSub;
        
        public ProgressWindow(ProgressWindowView view)
        {
            _view = view;
        }
        
        public void Open(bool animated = true, Action onDone = null)
        {
            if(animated) 
                _view.ShowView(onDone);
            else
            {
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
        
        public void OnDefeatedEnemiesCountChange(int value)
        {
            SetEnemiesCount();
        }

        private void SetEnemiesCount()
        {
        }
    }
}