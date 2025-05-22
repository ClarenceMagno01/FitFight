using UnityEngine;

namespace _Project.Scripts.Main
{
    public class TimerManager : MonoBehaviour
    {
        public static TimerManager Instance { get; private set; }

        private float _elapsedTime;
        private bool _isRunning;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Update()
        {
            if (_isRunning)
                _elapsedTime += Time.deltaTime;
        }

        public void StartTimer()
        {
            _elapsedTime = 0f;
            _isRunning = true;
        }

        public void StopTimer()
        {
            _isRunning = false;
        }

        public float GetElapsedTime()
        {
            return _elapsedTime;
        }

        public string GetFormattedTime()
        {
            int minutes = Mathf.FloorToInt(_elapsedTime / 60f);
            int seconds = Mathf.FloorToInt(_elapsedTime % 60f);
            return $"{minutes:D2}:{seconds:D2}";
        }
    }
}
