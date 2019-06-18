using System;
using System.Windows;
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
        private TimerModel timerModel;

        public MainWindow()
        {
            InitializeComponent();
            HideFromAltTab();
            Topmost = true;

            timerModel = new TimerModel();
            DataContext = timerModel;

            var timer = new DispatcherTimer();
            timer.Tick += timer_Tick;
            timer.Interval = TimeSpan.FromMilliseconds(10);
            timer.Start();
        }

        private void HideFromAltTab()
        {
            // Create helper window
            // Location of new window is outside of visible part of screen
            // size of window is enough small to avoid its appearance at the beginning
            // Set window style as ToolWindow to avoid its icon in AltTab
            var w = new Window
            {
                Top = -100,
                Left = -100,
                Width = 1,
                Height = 1,
                WindowStyle = WindowStyle.ToolWindow,
                ShowInTaskbar = false
            };
            w.Show(); // We need to show window before set is as owner to our main window
            Owner = w; // Okey, this will result to disappear icon for main window.
            w.Hide(); // Hide helper window just in case
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (timerModel.TimeIsZero)
                return;

            var now = DateTime.Now;
            var delta = now - (timerModel.LastTick ?? now);
            timerModel.Sub(delta);
            timerModel.LastTick = now;
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
                    EditTimeTextBox.Visibility = Visibility.Hidden;
                    ShowTimeTextBlock.Visibility = Visibility.Visible;
                    break;
                }

                case Key.Escape:
                    timerModel.Reset();
                    break;
            }
        }

        private void DragableArea_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released)
                return;
            DragMove();
        }

        private void ContextMenuQuit_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        protected override void OnClosed(EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void TextBlock_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed && e.ClickCount == 2)
            {
                ShowTimeTextBlock.Visibility = Visibility.Hidden;
                EditTimeTextBox.Visibility = Visibility.Visible;
                return;
            }
            DragableArea_OnMouseDown(sender, e);
        }

        private void EditTimeTextBox_OnLostFocus(object sender, RoutedEventArgs e)
        {
            EditTimeTextBox.Visibility = Visibility.Hidden;
            ShowTimeTextBlock.Visibility = Visibility.Visible;
        }
    }
}