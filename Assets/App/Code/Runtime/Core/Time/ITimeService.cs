namespace Assets.App.Code.Runtime.Core.Time
{
    public interface ITimeService
    {
        float DeltaTime { get; }
        float FixedDeltaTime { get; }
        float Time { get; }
        float UnscaledDeltaTime { get; }
        float UnscaledTime { get; }

        void SetTimeScale(float minValue);
        bool IsTimerEnd(ref float timer, float delay);
        void UpdateTimer(ref float timer);
    }
}
