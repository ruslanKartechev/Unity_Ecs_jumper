using System;

namespace UI.Windows
{
    public interface IWindow
    {
        void Open(bool animated = true, Action onDone = null);
        void Close(bool animated = true, Action onDone = null);
    }
}