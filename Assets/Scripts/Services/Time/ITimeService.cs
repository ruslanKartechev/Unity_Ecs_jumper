namespace Services.Time
{
    public interface ITimeService
    {
        double TimePoint { get; }
        float DeltaTime { get; }
        float RealDealtaTime { get; }
        float FixedDeltaTime { get; }
    }
}