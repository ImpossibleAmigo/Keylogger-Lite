using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace KeyloggerLite
{
    public class KeyLogger
    {
        private bool isRunning = false;
        private int seconds = 0;
        private int totalKeyPresses = 0;
        private readonly List<string> loggedKeys = new List<string>();
        private readonly System.Windows.Forms.Timer timer;
        public event Action<string>? TimeUpdated;
        public event Action<int>? TotalKeyPressesUpdated;
        public event Action<Dictionary<string, int>>? KeysUpdated;
        public KeyLogger()
        {
            timer = new System.Windows.Forms.Timer { Interval = 1000 };
            timer.Tick += Timer_Tick;
        }

        public void Start()
        {
            if (!isRunning)
            {
                isRunning = true;
                timer.Start();
            }
        }

        public void Stop()
        {
            if (isRunning)
            {
                isRunning = false;
                timer.Stop();
            }
        }

        public void Clear()
        {
            seconds = 0;
            totalKeyPresses = 0;
            loggedKeys.Clear();
            TimeUpdated?.Invoke("0D; 0H; 0M; 0S");
            TotalKeyPressesUpdated?.Invoke(0);
            KeysUpdated?.Invoke(new Dictionary<string, int>());
        }

        public void HandleKeyPress(string key)
        {
            if (isRunning)
            {
                loggedKeys.Add(key);
                totalKeyPresses++;
                TotalKeyPressesUpdated?.Invoke(totalKeyPresses);
                KeysUpdated?.Invoke(GetKeyCounts());
            }
        }

        public Dictionary<string, int> GetKeyCounts()
        {
            var keyCounts = new Dictionary<string, int>();
            foreach (string key in loggedKeys)
            {
                keyCounts[key] = keyCounts.ContainsKey(key) ? keyCounts[key] + 1 : 1;
            }
            return keyCounts;
        }

        public bool IsRunning => isRunning;
        public List<string> LoggedKeys => loggedKeys;

        private void Timer_Tick(object? sender, EventArgs? e)
        {
            seconds++;
            int days = seconds / 86400;
            int hours = (seconds % 86400) / 3600;
            int minutes = (seconds % 3600) / 60;
            int secs = seconds % 60;
            TimeUpdated?.Invoke($"{days}D; {hours}H; {minutes}M; {secs}S");
        }
    }
}