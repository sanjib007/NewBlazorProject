﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L3T.Infrastructure.Helpers.Models.Mikrotik.SocketModel
{
    public class TimerManager
    {
        private Timer _timer;
        private AutoResetEvent _autoResetEvent;
        private Action _action;
        public DateTime TimerStarted { get; set; }
        public bool IsTimerStarted { get; set; }
        public void PrepareTimer(Action action)
        {
            _action = action;
            _autoResetEvent = new AutoResetEvent(false);
            _timer = new Timer(Execute, _autoResetEvent, 2000, 4000);
            TimerStarted = DateTime.Now;
            IsTimerStarted = true;
        }
        public void Execute(object stateInfo)
        {
            _action();
            if ((DateTime.Now - TimerStarted).TotalSeconds > 300)
            {
                IsTimerStarted = false;
                _timer.Dispose();
            }
        }

    }
}
