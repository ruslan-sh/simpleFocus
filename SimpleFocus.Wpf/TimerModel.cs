using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using SimpleFocus.Wpf.Annotations;

namespace SimpleFocus.Wpf
{
    public class TimerModel : INotifyPropertyChanged
    {
        private TimeSpan _timeLeft { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public bool TimeIsZero => _timeLeft.Ticks <= 0;

        public void Sub(TimeSpan timerInterval)
        {
            _timeLeft = _timeLeft - timerInterval;
            if (_timeLeft.Ticks < 0) _timeLeft = TimeSpan.Zero;
            OnPropertyChanged(nameof(StringValue));
        }

        public string StringValue
        {
            get => _timeLeft.ToString("g");
            set
            {
                _timeLeft = TimeSpan.FromMinutes(Convert.ToDouble(value));
                OnPropertyChanged(nameof(StringValue));
            }
        }

        public void Reset()
        {
            _timeLeft = TimeSpan.Zero;
            OnPropertyChanged(nameof(StringValue));
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
