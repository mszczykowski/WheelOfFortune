using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace WheelOfFortune
{
    class GameTimer
    {
        private DispatcherTimer dispatcherTimer;
        public EventHandler TimeEnded;
        public EventHandler TimerUpdated;
        public int Time { get => _time; }

        private readonly int decisionTime = 10;
        private readonly int answerTime = 20;

        private int _time;
        public GameTimer()
        {
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Interval = TimeSpan.FromSeconds(1);
            dispatcherTimer.Tick += TimerTick;
        }

        public void SetTimerDecison()
        {
            StopTimer();
            _time = decisionTime;
            EventHelper.RaiseEvent(TimerUpdated);
        }
        public void SetTimerAnswer()
        {
            StopTimer();
            _time = answerTime;
            EventHelper.RaiseEvent(TimerUpdated);
        }
        public void StartTimer()
        {
            dispatcherTimer.Start();
        }
        public void StopTimer()
        {
            dispatcherTimer.Stop();
        }
        private void TimerTick(object sender, EventArgs e)
        {
            _time--;
            EventHelper.RaiseEvent(TimerUpdated);
            if(_time == 0)
            {
                StopTimer();
                EventHelper.RaiseEvent(TimeEnded);
            }
        }
    }
}
