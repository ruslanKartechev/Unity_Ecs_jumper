using UnityEngine;

namespace Services.Input
{
    public interface IInputService
    {
        void AddTouchListener(ITouchListener listener);
        void RemoveTouchListener(ITouchListener listener);
        bool IsEnabled { get; set; }
        Vector2 MousePosition { get; }
        Vector2 Delta { get; }
    }
}