using UnityEngine;

namespace Services.Screen
{
    public interface IScreenService
    {
        bool IsOnScreen(Vector3 worldPosition, float screenOffsetPercent);
    }
}