using UnityEngine;

namespace Services.Input
{
    public interface ITouchListener
    {
        void Touch(Vector2 position, int finger);
        void Hold(Vector2 position, int finger);
        void Release(int finger);
    }
}