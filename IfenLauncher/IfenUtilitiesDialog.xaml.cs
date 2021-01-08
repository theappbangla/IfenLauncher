using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using IfenLauncher.GameLoader;

namespace IfenLauncher
{
    public partial class IfenUtilitiesDialog : Window
    {
        private IfenUtilitiesDialog()
        {
            InitializeComponent();
        }

        static IfenUtilitiesDialog _messageBox;

        public static void Open()
        {
            _messageBox = new IfenUtilitiesDialog();
            _messageBox.ShowDialog();
        }

        private void SetImage(string imageName)
        {
            string uri = string.Format("/Resources/images/{0}", imageName);
            var uriSource = new Uri(uri, UriKind.RelativeOrAbsolute);
        }


        private void OnClick(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;

            switch(btn.Name)
            {
                case "btnDeleteGsettings":
                    RemoveGSettings();
                    break;

                default:
                    break;
            }
        }

        private void CloseMe()
        {
            _messageBox.Close();
        }


        static void RemoveGSettings()
        {
            try
            {
                var dirPath = @"C:\ProgramData\BrainMaster";
                DirectoryInfo directoryInfo = new DirectoryInfo(dirPath);
                var folders = directoryInfo.GetDirectories();
                var atLeastOneDeleted = false;

                foreach (DirectoryInfo folder in folders)
                {
                    foreach (FileInfo f in folder.GetFiles())
                    {
                        var shouldDelete = f.Name.Contains("gsettings.bdb2.");

                        if (shouldDelete)
                        {
                            atLeastOneDeleted = true;
                            f.Delete();
                        }

                    }
                }

                if (atLeastOneDeleted) MessageBox.Show("Successfully deleted all gsettings");
                else MessageBox.Show("No gsettings found to delete.\nMaybe already deleted before");
            } catch {
                MessageBox.Show("Something Error Occured");
            }

        }

        private void BtnCloseDialog(object sender, RoutedEventArgs e)
        {
            _messageBox.Close();
        }
    }
}
