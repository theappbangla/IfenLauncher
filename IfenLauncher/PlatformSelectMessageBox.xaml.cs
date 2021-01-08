using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using IfenLauncher.GameLoader;

namespace IfenLauncher
{
    public partial class PlatformSelectMessageBox : Window
    {
        Action actionBA, actionNG;
        private PlatformSelectMessageBox(Action actionBA, Action actionNG)
        {
            InitializeComponent();

            this.actionBA = actionBA;
            this.actionNG = actionNG;
        }

        static PlatformSelectMessageBox _messageBox;

        public static void Show2(Action actionBA, Action actionNG)
        {
            //manuals = m;
            _messageBox = new PlatformSelectMessageBox(actionBA, actionNG);
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


        private void OnPlatformClick(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;

            switch(btn.Name)
            {
                case "btnBA":
                    actionBA();
                    break;

                case "btnNG":
                    actionNG();
                    break;

                default:
                    break;
            }


            _messageBox.Close();
        }

        private void BtnCloseDialog(object sender, RoutedEventArgs e)
        {
            _messageBox.Close();
        }
    }
}
