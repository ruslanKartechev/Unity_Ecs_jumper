using UnityEngine;
namespace Services.Time.Impl
{
    public class TimeService : ITimeService
    {
        public double TimePoint
        {
            get
            {
                return UnityEngine.Time.time;
            }
        }

        public float DeltaTime
        {
            get
            {
                return UnityEngine.Time.deltaTime;
            }
        }

        public float RealDealtaTime
        {
            get
            {
                return UnityEngine.Time.unscaledDeltaTime;
            }
        }

        public float FixedDeltaTime
        {
            get
            {
                return UnityEngine.Time.fixedDeltaTime;
            }
        }

    }
}