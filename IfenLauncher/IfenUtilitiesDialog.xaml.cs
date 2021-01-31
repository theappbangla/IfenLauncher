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
                case "btnFixFlashStep1":
                    MessageBoxResult result = MessageBox.Show("Click OK.\nIt will popup flash player uninstaller.\nMust click on uninstall.\nThen, click Done and run Step-2.",
                                                  "Note - Must Read",
                                                  MessageBoxButton.OK,
                                                  System.Windows.MessageBoxImage.Information);

                    if (result == MessageBoxResult.OK)
                    {
                        FlashFix1();
                    }
                    break;
                case "btnFixFlashStep2":
                    FlashFix2();
                    MessageBox.Show("Please restart IFEN Neuroscience Launcher.",
                                        "Success",
                                        MessageBoxButton.OK,
                                        System.Windows.MessageBoxImage.Information);
                    break;

                default:
                    break;
            }
        }

        private void FlashFix1()
        {
            var proc1 = new ProcessStartInfo();
            string anyCommand = @"C:\Windows\SysWOW64\Macromed\Flash\FlashUtil_ActiveX.exe";
            proc1.UseShellExecute = true;
            proc1.WorkingDirectory = @"C:\Windows\System32";
            proc1.FileName = @"C:\Windows\System32\cmd.exe";
            proc1.Verb = "runas";
            proc1.Arguments = "/c " + anyCommand;
            proc1.WindowStyle = ProcessWindowStyle.Hidden;
            Process.Start(proc1);
        }

        private void FlashFix2()
        {
            var proc1 = new ProcessStartInfo();
            string anyCommand = @"C:\Windows\SysWOW64\regsvr32.exe ""C:\Program Files (x86)\IFEN Neuroscience\Assets\Flash32_32_0_0_371.ocx""";
            proc1.UseShellExecute = true;
            proc1.WorkingDirectory = @"C:\Windows\System32";
            proc1.FileName = @"C:\Windows\System32\cmd.exe";
            proc1.Verb = "runas";
            proc1.Arguments = "/c " + anyCommand;
            proc1.WindowStyle = ProcessWindowStyle.Hidden;
            Process.Start(proc1);
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
