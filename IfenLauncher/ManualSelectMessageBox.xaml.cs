using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using IfenLauncher.GameLoader;

namespace IfenLauncher
{
    public partial class ManualSelectMessageBox : Window
    {
        private ManualSelectMessageBox(List<Manual> manuals)
        {
            InitializeComponent();
            ListViewManuals.ItemsSource = manuals;
        }
        static ManualSelectMessageBox _messageBox;

        public static void Show2(List<Manual> m)
        {
            //manuals = m;
            _messageBox = new ManualSelectMessageBox(m);
            _messageBox.ShowDialog();
        }
        private static void SetImageOfMessageBox(MessageBoxImage image)
        {
            switch (image)
            {
                case MessageBoxImage.Warning:
                    _messageBox.SetImage("Warning.png");
                    break;
                case MessageBoxImage.Question:
                    _messageBox.SetImage("Question.png");
                    break;
                case MessageBoxImage.Information:
                    _messageBox.SetImage("Information.png");
                    break;
                case MessageBoxImage.Error:
                    _messageBox.SetImage("Error.png");
                    break;
                default:
                    break;
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            /*if (sender == btnOk)
                _result = MessageBoxResult.OK;
            else if (sender == btnYes)
                _result = MessageBoxResult.Yes;
            else if (sender == btnNo)
                _result = MessageBoxResult.No;
            else if (sender == btnCancel)
                _result = MessageBoxResult.Cancel;
            else
                _result = MessageBoxResult.None; */
            _messageBox.Close();
            _messageBox = null;
        }
        private void SetImage(string imageName)
        {
            string uri = string.Format("/Resources/images/{0}", imageName);
            var uriSource = new Uri(uri, UriKind.RelativeOrAbsolute);
        }


        private void OpenManualClick(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            Manual manual = btn.DataContext as Manual;

            try
            {
                Process.Start(manual.fileOpenUrl);
                _messageBox.Close();
            } catch
            {
                MessageBox.Show("No manual found!",
                                "Ops. Something Error",
                                MessageBoxButton.OK,
                                System.Windows.MessageBoxImage.Warning);
            }
        }

        private void BtnCloseDialog(object sender, RoutedEventArgs e)
        {
            _messageBox.Close();
        }
    }
}
