using System;
namespace Services.Time
{
    public interface ISlowMotionService
    {
        void SetNormalScale(Action onEnd);
        void SetSlowScale(Action onEnd);
        void SetScale(float scale, float time, Action onEnd);
        void Refresh();
        float CurrentScale { get; }
        float CurrentFixedDeltaMultiplier { get; }
    }
}