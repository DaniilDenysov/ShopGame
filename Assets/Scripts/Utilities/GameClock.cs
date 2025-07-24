using System;
using UnityEngine;


namespace ShopGame.Utilities
{
    public class GameClock : MonoBehaviour
    {
        [SerializeField] private float timeEstimation = 120f;
        private Clock clock;

        private void Awake()
        {
            clock = new Clock(OnTimeChanged, TimerStoppedCallback, TimerElapsedCallback);
            clock.Start(timeEstimation);
        }

        private void FixedUpdate()
        {
            clock?.Tick(Time.fixedDeltaTime);
        }

        private void OnTimeChanged(float newTime)
        {
            int minutes = Mathf.FloorToInt(newTime / 60);
            int seconds = Mathf.FloorToInt(newTime % 60);

            Debug.Log($"Time: {minutes}:{seconds:D2}");
        }

        private void TimerStoppedCallback()
        {
            Debug.Log("Timer stopped.");
        }

        private void TimerElapsedCallback()
        {
            Debug.Log("Timer reached zero.");
        }

        #region Public API

        public void StopTimer() => clock?.Stop();
        public void ResumeTimer() => clock?.Resume();
        public bool IsRunning() => clock != null && !clock.IsStopped;

        #endregion
    }

    [Serializable]
    public class Clock
    {
        public enum ClockMode
        {
            Countdown,
            Stopwatch
        }

        public float CurrentTime { get; private set; }
        public bool IsStopped { get; private set; }
        public ClockMode Mode { get; private set; } = ClockMode.Countdown;

        private Action<float> onTimeUpdated;
        private Action onStopped;
        private Action onElapsed;

        public Clock(Action<float> timeUpdateCallback, Action stoppedCallback, Action elapsedCallback = null)
        {
            onTimeUpdated = timeUpdateCallback;
            onElapsed = elapsedCallback;
            onStopped = stoppedCallback;
            CurrentTime = 0;
            IsStopped = true;
        }

        public void Start(float time, ClockMode mode = ClockMode.Countdown)
        {
            CurrentTime = mode == ClockMode.Countdown ? time : 0;
            Mode = mode;
            IsStopped = false;
            NotifyTimeUpdate();
        }

        public void Stop()
        {
            IsStopped = true;
            NotifyStopped();
        }

        public void StopNoNotify()
        {
            IsStopped = true;
        }

        public void Resume()
        {
            IsStopped = false;
        }

        public static string ToString(float CurrentTime)
        {
            int minutes = Mathf.FloorToInt(CurrentTime / 60);
            int seconds = Mathf.FloorToInt(CurrentTime % 60);
            return $"{minutes}:{seconds:D2}";
        }

        public override string ToString()
        {
            return ToString(CurrentTime);
        }

        public void Tick(float deltaTime)
        {
            if (IsStopped) return;

            if (Mode == ClockMode.Countdown)
            {
                if (CurrentTime <= 0) return;

                CurrentTime -= deltaTime;

                if (CurrentTime <= 0)
                {
                    CurrentTime = 0;
                    IsStopped = true;
                    onElapsed?.Invoke();
                    return;
                }
            }
            else if (Mode == ClockMode.Stopwatch)
            {
                CurrentTime += deltaTime;
            }

            NotifyTimeUpdate();
        }

        private void NotifyTimeUpdate()
        {
            onTimeUpdated?.Invoke(CurrentTime);
        }

        private void NotifyStopped()
        {
            onStopped?.Invoke();
        }
    }
}
