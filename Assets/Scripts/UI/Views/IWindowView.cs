using System;

namespace UI.Views
{
    public interface IWindowView
    {
        void ShowView(Action onShown);
        void CloseView(Action onClosed);
    }
}