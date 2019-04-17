using System;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace SimpleFocus.Wpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private readonly DispatcherTimer timer;
        private TimerModel timerModel;

        public MainWindow()
        {
            InitializeComponent();
            Topmost = true;

            timerModel = new TimerModel();
            DataContext = timerModel;

            timer = new DispatcherTimer();
            timer.Tick += timer_Tick;
            timer.Interval= TimeSpan.FromMilliseconds(10);
            timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (timerModel.TimeIsZero)
                return;
            timerModel.Sub(timer.Interval);
        }

        private void UIElement_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (!(sender is Control senderControl)) return;

            switch (e.Key)
            {
                case Key.Enter:
                {
                    var binding = senderControl.GetBindingExpression(TextBox.TextProperty);
                    binding?.UpdateSource();
                    break;
                }
                case Key.Escape:
                    timerModel.Reset();
                    break;
            }
        }

        private void UIElement_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
