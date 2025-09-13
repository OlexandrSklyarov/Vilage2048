using VContainer.Unity;

namespace Assets.App.Code.Runtime.Core.Time
{
    public sealed class GameTimeService : ITimeService, ITickable
    {
        public float DeltaTime { get; internal set; }
        public float FixedDeltaTime { get; internal set; }
        public float Time { get; internal set; }
        public float UnscaledDeltaTime { get; internal set; }
        public float UnscaledTime { get; internal set; }

        void ITickable.Tick()
        {
            Time = UnityEngine.Time.time;
            UnscaledTime = UnityEngine.Time.unscaledTime;
            DeltaTime = UnityEngine.Time.deltaTime;
            FixedDeltaTime = UnityEngine.Time.fixedDeltaTime;
            UnscaledDeltaTime = UnityEngine.Time.unscaledDeltaTime;
        }

        public bool IsTimerEnd(ref float timer, float delay)
        {
            timer -= DeltaTime;

            if (timer <= 0f)
            {
                timer = delay;
                return true;
            }

            return false;
        }

        public void SetTimeScale(float value)
        {
            UnityEngine.Time.timeScale = value;
        }


        public void UpdateTimer(ref float timer)
        {
            timer -= DeltaTime;
        }
    }
}
