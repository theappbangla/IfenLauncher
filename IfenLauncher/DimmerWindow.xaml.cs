using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Diagnostics;
using IfenLauncher.Server;
using AudioSwitcher.AudioApi.CoreAudio;

namespace IfenLauncher
{
    /// <summary>
    /// Interaction logic for DimmerWindow.xaml
    /// </summary>
    public partial class DimmerWindow : Window, InputControlWindow.Listener
    {
        bool isMaximized = false;
        InputControlWindow inputControlWindow;

        List<Color> colors;
        int currentColorPos = 2;

        bool isDotShown = false;
        bool isVolumeSelected = false;

        CoreAudioDevice defaultPlaybackDevice;

        public DimmerWindow()
        {
            InitializeComponent();

            //CompositionTarget.Rendering += UpdateDimmer;

            inputControlWindow = new InputControlWindow(this);

            colors = new List<Color>();
            colors.Add(Colors.Red);
            colors.Add(Colors.Blue);
            colors.Add(Colors.Black);
            colors.Add(Colors.Green);
            colors.Add(Colors.Yellow);


            this.KeyDown += new KeyEventHandler(OnButtonKeyDown);

            var posDotX = Properties.Settings.Default.posDotX;
            var posDotY = Properties.Settings.Default.posDotY;
            var dimmerWidth = Properties.Settings.Default.dimmerWidth;
            var dimmerHeight = Properties.Settings.Default.dimmerHeight;

            if (posDotX > -1 && posDotY > -1)
            {
                var x = this.Width * (posDotX / dimmerWidth);
                var y = this.Height * (posDotY / dimmerHeight);
                SetDotXY(x, y);
                // SetDotXY(posDotX, posDotY);
            }

            /*if (dimmerWidth > -1 && dimmerHeight > -1)
            {
                this.Width = dimmerWidth;
                this.Height = dimmerHeight;
            }*/

            dot.Visibility = Visibility.Hidden;

            toggleDotVisibility();
            // toggleVolumeFeedback();
        }

        private void SetDotXY(double x, double y)
        {
            Canvas.SetLeft(this.dot, x);
            Canvas.SetTop(this.dot, y);
        }

        private void OnButtonKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Right)
            {
                currentColorPos += 1;
                if (currentColorPos >= colors.Count) currentColorPos = 0;
                bg.Color = colors[currentColorPos];
            }

            if (e.Key == Key.Left)
            {
                currentColorPos -= 1;
                if (currentColorPos < 0) currentColorPos = colors.Count - 1;
                bg.Color = colors[currentColorPos];
            }

        }

        private void UpdateDimmer(Input input)
        {

            /*if (e.Key == Key.Up && bg.Opacity <= 1)
            {
                bg.Opacity += 0.01;
            }

            if (e.Key == Key.Down && bg.Opacity >= 0)
            {
                bg.Opacity -= 0.01;
            }*/

            float In5 = input.In5;
            //Debug.WriteLine("Opacity: " + bg.Opacity + ", In5: " + In5);

            if (In5 == -1000f) return;

            if (In5 != 0)
            {
                if (bg.Opacity < 0.10) return;
                bg.Opacity -= 0.01;
                dot.Fill = new SolidColorBrush(GetBWColorFromAlphaChannel(bg.Opacity));
                if (isVolumeSelected) defaultPlaybackDevice.Volume = defaultPlaybackDevice.Volume < 2 ? 0 : defaultPlaybackDevice.Volume - 1;
            }
            else
            {
                if (bg.Opacity > 0.90) return;
                bg.Opacity += 0.01;
                dot.Fill = new SolidColorBrush(GetBWColorFromAlphaChannel(bg.Opacity));
                if (isVolumeSelected) defaultPlaybackDevice.Volume = defaultPlaybackDevice.Volume > 98 ? 100 : defaultPlaybackDevice.Volume + 1;
            }

            //test.Content = test.Content + e.Key.ToString();
        }

        private Color GetBWColorFromAlphaChannel(double a)
        {
            var all = Convert.ToByte(255 * a);
            var nColor = Color.FromRgb(all, all, all);

            return nColor;
        }

        private void Button_Maximize(object sender, RoutedEventArgs e)
        {
            if (!isMaximized)
            {
                WindowState = WindowState.Maximized;
            }
            else
            {
                WindowState = WindowState.Normal;
            }

            isMaximized = !isMaximized;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void Button_Exit(object sender, RoutedEventArgs e)
        {
            this.Close();
            inputControlWindow.StopInputReceiving();
            inputControlWindow.RemoveRenderListener();

        }

        private void On_Closed(object sender, EventArgs e)
        {
            inputControlWindow.Close();
            /*UIElement container = VisualTreeHelper.GetParent(this.dot) as UIElement;
            Point relativeLocation = this.dot.TranslatePoint(new Point(0, 0), container);*/

            Properties.Settings.Default.posDotX = Canvas.GetLeft(this.dot);
            Properties.Settings.Default.posDotY = Canvas.GetTop(this.dot);

            Properties.Settings.Default.dimmerWidth = this.Width;
            Properties.Settings.Default.dimmerHeight = this.Height;

            Properties.Settings.Default.Save();

        }

        private void On_Render(object sender, EventArgs e)
        {
            //inputControlWindow.Show();
        }

        public void OnInputControlClosed()
        {
            //Button_Exit(null, null);
        }

        public void OnReceiveInput(Input input)
        {
            UpdateDimmer(input);
        }

        public void OnChangeFps(int fps)
        {
            if (fps == -1) // -1 means monitor fps "D"
            {
                fpsText.Text = "D";
            }
            else
            {
                fpsText.Text = "" + fps;
            }
        }

        private void Button_ShowInputControl(object sender, RoutedEventArgs e)
        {
            inputControlWindow.Show();
        }

        private void Button_DotControl(object sender, RoutedEventArgs e)
        {
            toggleDotVisibility();
        }

        private void Button_VolumeControl(object sender, RoutedEventArgs e)
        {
            toggleVolumeFeedback();
        }

        private void toggleVolumeFeedback()
        {
            if (defaultPlaybackDevice == null)
            {
                defaultPlaybackDevice = new CoreAudioController().DefaultPlaybackDevice;
            }

            isVolumeSelected = !isVolumeSelected;

            if (isVolumeSelected)
            {
                btnVolume.Background = Brushes.White;
                btnVolume.Foreground = Brushes.Black;
            }
            else
            {
                btnVolume.Background = Brushes.Black;
                btnVolume.Foreground = Brushes.White;
            }
        }


        private void toggleDotVisibility()
        {
            isDotShown = !isDotShown;

            if (isDotShown)
            {
                btnDot.Background = Brushes.White;
                btnDot.Foreground = Brushes.Black;
                dot.Visibility = Visibility.Visible;
            }
            else
            {
                btnDot.Background = Brushes.Black;
                btnDot.Foreground = Brushes.White;
                dot.Visibility = Visibility.Hidden;
            }
        }


        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var oldX = Canvas.GetLeft(this.dot);
            var oldY = Canvas.GetTop(this.dot);

            var oldW = e.PreviousSize.Width;
            var oldH = e.PreviousSize.Height;

            var ratioDotX = oldX / oldW;
            var ratioDotY = oldY / oldH;

            var newW = e.NewSize.Width;
            var newH = e.NewSize.Height;

            var diffW = newW - oldW;
            var diffH = newH - oldH;

            var newX = oldX + (ratioDotX * diffW);
            var newY = oldY + (ratioDotY * diffH);

            Console.WriteLine("newX = " + newX + ", newY = " + newY);

            if (double.IsInfinity(newX) || double.IsInfinity(newY)) return;

            SetDotXY(newX, newY);
            // Console.WriteLine("w = " + w + ", h = " + h + " --> newW = " + e.NewSize.Width + ", newH = " + e.NewSize.Height + " --> x = " + x + ", y = " + y);

            // Console.WriteLine(w + ", " + h);
        }
    }
}
