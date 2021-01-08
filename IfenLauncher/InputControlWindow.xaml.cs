using IfenLauncher.Server;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace IfenLauncher
{
    /// <summary>
    /// Interaction logic for InputControlWindow.xaml
    /// </summary>
    public partial class InputControlWindow : Window
    {
        public interface Listener
        {
            void OnInputControlClosed();
            void OnReceiveInput(Input input);
            void OnChangeFps(int fps);
        }
        public Listener listener;

        public static Input input;
        private int fps = 5;
        private bool IsDefault = true;

        public InputControlWindow(Listener listener)
        {
            InitializeComponent();
            /*textBoxFps.Text = "" + 40;
            Button_Click(null, null);*/
            this.listener = listener;
            Update();
            CompositionTarget.Rendering += OnRenderUpdate;
            listener.OnChangeFps(-1); // -1 means monitor fps "D"
        }

        int c = 0;
        private void OnRenderUpdate(object sender, EventArgs e)
        {
            // Debug.WriteLine(" --> " + c);
            if (!IsDefault)
            {
                CompositionTarget.Rendering -= OnRenderUpdate;
                return;
            }

            this.Dispatcher.Invoke(() => {
                input = Input.GetInstance();
                listener.OnReceiveInput(input);
            });

            c++;
            // Debug.WriteLine(input.Text(", ") + " --> " + c);
        }

        CancellationTokenSource ts;
        private void Update()
        {
            if (IsDefault) return;

            int count = 0;
            int sleepMills = 1000 / fps;

            StopInputReceiving();

            ts = new CancellationTokenSource();
            CancellationToken ct = ts.Token;
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    if(!IsDefault)
                        Thread.Sleep(sleepMills);

                    this.Dispatcher.Invoke(() => {
                        input = Input.GetInstance();
                        listener.OnReceiveInput(input);
                    });

                    count++;
                    Debug.WriteLine(input.Text(", ") + " --> " + count);

                    if (ct.IsCancellationRequested)
                    {
                        // another thread decided to cancel
                        Console.WriteLine("task canceled");
                        break;
                    }
                }
            }, ct);
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            IsDefault = false;

            try
            {
                fps = Int32.Parse(textBoxFps.Text);
            } catch (Exception ex)
            {
                textUpdating.Text = "Enter a number";
                return;
            }

            textUpdating.Text = "Updating";
            Task.Delay(2000).ContinueWith(_ => {
                Update();
                this.Dispatcher.Invoke(() => {
                    textUpdating.Text = "Running at " + fps + " Hz";
                    listener.OnChangeFps(fps);
                });
            });
        }

        public void StopInputReceiving()
        {
            if (ts != null)
                ts.Cancel();
        }

        public void RemoveRenderListener()
        {
            CompositionTarget.Rendering -= OnRenderUpdate;
        }

        private void On_Closed(object sender, EventArgs e)
        {
            listener.OnInputControlClosed();
        }

        private void Button_Close(object sender, RoutedEventArgs e)
        {
            Hide();
        }
    }
}
