using System;
using System.Windows;
using System.Windows.Media;
using IfenLauncher.Server;
using IfenLauncher.GameLoader;
using IfenLauncher.FlashFileHandler;
using IfenLauncher.Installer;
using IfenLauncher.ProcessManager;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Diagnostics;
using IfenLauncher.Utils;
using System.Net.NetworkInformation;

namespace IfenLauncher
{
    public partial class MainWindow : Window, IfenResourceLoaderCallback, IpDetectListener
    {
        IfenHost ifenHost;
        IfenResourceLoader ifenResourceLoader;
        IFENFeedbackCopy ifenFeedbackCopy;
        MakeRoomCopy makeRoomCopy;
        FolderCopy folderCopy;
        CapitoInstaller capitoInstaller;

        MyProcessManager processManager;

        IpDetect ipDetect;

        public MainWindow()
        {
            InitializeComponent();

            ifenFeedbackCopy = new IFENFeedbackCopy();
            ifenFeedbackCopy.HandleBrainCellFile();

            makeRoomCopy = new MakeRoomCopy();
            makeRoomCopy.HandleMakeRoomFile();

            folderCopy = new FolderCopy();
            folderCopy.DoCopy();

            capitoInstaller = new CapitoInstaller();

            ifenHost = new IfenHost();

            processManager = new MyProcessManager();
            // processManager.RegisterServerFlExitListener(OnServerFlashPlayerExited);

            ifenResourceLoader = new IfenResourceLoader(this);
            ifenResourceLoader.ProcessAllLaunchers();

            ipDetect = new IpDetect(this);
            ipDetect.RefreshIpMatchesDG();

            Debug.WriteLine("current dir: " + Environment.CurrentDirectory);

            NetworkChange.NetworkAvailabilityChanged += OnNetworkAvailabilityChanged;
            Debug.WriteLine("Network listener set");


            StartServer();
        }

        private void AvailabilityChanged(object sender, NetworkAvailabilityEventArgs e)
        {
            if (e.IsAvailable)
                MessageBox.Show("Network connected!");
            else
                MessageBox.Show("Network disconnected!");
        }

        private void OnNetworkAvailabilityChanged(object sender, NetworkAvailabilityEventArgs e)
        {
            this.Dispatcher.Invoke(() =>
            {
                ButtonRefreshIP(null, null);
                //ipDetect.RefreshIpMatchesDG();
            });

            if (e.IsAvailable)
                Console.WriteLine("Network connected!");
            else
                Console.WriteLine("Network dis connected!");
        }

        public void GameMainDirectoryNotExists()
        {

        }

        public void GamesNotFound()
        {

        }

        public void OnResouceProcessFinish(List<LauncherJson> launchers)
        {
            bool isPinball = false;
            bool isAirGame = false;
            bool isErpAnalyzer = false;
            bool isIfenRoads = false;

            if (launchers.Count > 0)
            {
                foreach(var ln in launchers)
                {
                    switch (ln.id)
                    {
                        case "pinball-pc":
                            isPinball = true;
                            break;

                        case "ifen-airgame": // by mistake [actual: airgame-pc]
                            isAirGame = true;
                            break;

                        case "erp-analyzer":
                            isErpAnalyzer = true;
                            break;

                        case "ifen-roads":
                            isIfenRoads = true;
                            break;

                        default:
                            break;
                    }
                }

            }

            launchers.Insert(0, LauncherJsonGenerator.GetCapito());
            launchers.Insert(1, LauncherJsonGenerator.GetAdvancedDimmer());
            launchers.Insert(2, LauncherJsonGenerator.GetIfenUtilities());
            launchers.Insert(3, LauncherJsonGenerator.GetMakeRoom());

            if (!isIfenRoads)
            {
                launchers.Insert(4, LauncherJsonGenerator.GetIfenRoads());
            }

            if (!isPinball)
            {
                launchers.Add(LauncherJsonGenerator.GetPinball());
            }

            if (!isAirGame)
            {
                launchers.Add(LauncherJsonGenerator.GetAirgame());
            }

            if (!isErpAnalyzer)
            {
                launchers.Add(LauncherJsonGenerator.GetIfenErpAnalyzer());
            }

            launchers.Add(LauncherJsonGenerator.GetSwingleAssessment());

            ListViewGames.ItemsSource = launchers;

            ifenResourceLoader.PrintLaunchersInfo(); // print in debug
        }

        public void ExecuteAsAdmin(string fileName)
        {
            //ExecuteAsAdmin(@"C:\Program Files (x86)\BrainMaster\Avatar\Flash Player\FlashPlayer.exe");
            Process proc = new Process();
            proc.StartInfo.FileName = fileName;
            proc.StartInfo.UseShellExecute = true;
            proc.StartInfo.Verb = "runas";
            proc.Start();
        }

        private void RunManualClick(object sender, RoutedEventArgs e)
        {
            /* var messageBoxResult = WpfMessageBox.Show(
                "Message Box Title", "Are you sure?",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning
                );
            return; */

            Button btn = sender as Button;
            LauncherJson launcherJson = btn.DataContext as LauncherJson;
            if(launcherJson.manuals != null)
                ManualSelectMessageBox.Show2(launcherJson.manuals);
            return;

            Debug.WriteLine(launcherJson.manuals);

            if (launcherJson.manuals == null || launcherJson.manuals.Count < 1) return;

            /* string manual = launcherJson.manualDir + @"\" + launcherJson.manuals[0].fileName;
            Debug.WriteLine("manual loc: " + manual);
            try
            {
                Process p = new Process();
                p.StartInfo.FileName = manual;
                p.Start();
            } catch(Exception ep)
            {
                Debug.WriteLine(ep.Message);
            }*/
        }

        private void RunGameClick(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            LauncherJson launcherJson = btn.DataContext as LauncherJson;
            Debug.WriteLine("exeLocation: " + launcherJson.exeLocation);

            if(launcherJson.flag != null)
            {
                switch(launcherJson.flag)
                {
                    case "capito":
                        capitoInstaller.RunInstallerOrProgram();
                        break;

                    case "makeroom":
                        PlatformSelectMessageBox.Show2(() => {
                            try
                            {
                                processManager.StartFlashPlayerFor(MyProcessManager.GameDirName.MakeRoom);
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine("Error StartFlashPlayerFor: " + ex.Message);
                                MessageBox.Show("Please install BrainAvatar first.");
                            }
                        }, () => {
                            MessageBox.Show("Coming soon ...");
                        });
                        break;

                    case "url":
                        Process.Start(launcherJson.url);
                        break;

                    case "advanceddimmer":
                        CheckServerAndRunProgram(() => {
                            LicenseWindow licenseWindow = new LicenseWindow(launcherJson, () => {
                                DimmerWindow dimmerWindow = new DimmerWindow();
                                dimmerWindow.Show();
                            });
                        });
                        break;

                    case "ifenutilities":
                        IfenUtilitiesDialog.Open();
                        break;

                    default:
                        Debug.WriteLine("flag: " + launcherJson.flag + " is not recognized");
                        break;
                }
            } 
            else
            {
                ProcessStartInfo processStartInfo = new ProcessStartInfo();
                processStartInfo.WorkingDirectory = launcherJson.rootDirName;
                processStartInfo.FileName = launcherJson.exeLocation;

                if (launcherJson.serverNotRequired == false)
                {
                    CheckServerAndRunProgram(() => {
                        Process.Start(processStartInfo);
                    });
                } else
                {
                    Process.Start(processStartInfo);
                }
            }

        }

        private void CheckServerAndRunProgram(Action action)
        {
            if (ifenHost != null && ifenHost.IsServerStarted())
            {
                //processStartManager.StartServerFlashPlayer(OnServerExited); // for bypassing the values if it is closed but server is running
                action();
                return;
            }

            MessageBoxResult result = MessageBox.Show("You're Offline. Do you want to continue?",
                                          "Confirmation",
                                          MessageBoxButton.YesNo,
                                          System.Windows.MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                action();
            }
        }

        private void HandleBottomButtons(object sender, RoutedEventArgs e)
        {
            if(sender.Equals(btnCapito))
            {
                Process.Start("https://capito.muenchen-neurofeedback.de/index.php/en/");
            }
            else if (sender.Equals(btnNeurofeedbackPartner))
            {
                Process.Start("https://www.neurofeedback-partner.de/en");
            } 
            else if (sender.Equals(btnIfen))
            {
                Process.Start("https://www.neurofeedback-info.de/en/home-en-2.html");
            } 
            else if (sender.Equals(btnIfenProtocols))
            {
                Process.Start("https://youtu.be/LhtLSCZBkeY");
            }
            else if (sender.Equals(btnProtocols))
            {
                Process.Start("https://www.neurofeedback-partner.de/product_info.php?language=en&info=p321_ifen-pro-z-protocol-for-discovery.html");
            }
            else if (sender.Equals(btnFreeCap))
            {
                Process.Start("https://www.neurofeedback-partner.de/product_info.php?language=en&info=p199_free-cap-19-kanal-mit-gesinterten-silber-silber-chlorid-ag-agcl-elektroden.html");
            }
        }

        private void ButtonServer(object sender, RoutedEventArgs e)
        {
            if (!ifenHost.IsServerStarted())
            {
                StartServer();
            }
            else
            {
                StopSerevr();
            }
        }

        /*private void OnServerFlashPlayerExited(object sender, EventArgs e)
        {
            StopSerevr();
        }*/


        private void StartServer()
        {
            if (ifenHost == null)
            {
                ifenHost = new IfenHost();
            }

            try
            {
                ifenHost.StartServer();
                btnServer.Content = "Offline";
                btnServer.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#000000"));

                try
                {
                    processManager.StartServerFlashPlayer();
                } catch { }
            }
            catch (System.Net.HttpListenerException ex)
            {
                Debug.WriteLine("Already Server Started on PORT 3000");
                Debug.WriteLine("Error: " + ex.Message);
                MessageBox.Show("Cannot Go Online");
            }
        }

        private void StopSerevr()
        {
            if (ifenHost != null)
            {
                ifenHost.StopServer();
                ifenHost = null;
            }

            try
            {
                processManager.KillBmFlashPlayer();

                this.Dispatcher.Invoke(() =>
                {
                    btnServer.Content = "Go Online";
                    btnServer.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#009715"));
                });
            }
            catch { }

        }

        private void ButtonBrainAvatar(object sender, RoutedEventArgs e)
        {
            if(!processManager.IsBrainAvatarInstalled())
            {
                MessageBoxResult result = MessageBox.Show("Do you want to download BrainAvatar now?",
                                              "Download BrainAvatar",
                                              MessageBoxButton.YesNo,
                                              System.Windows.MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    Process.Start(LauncherJsonGenerator.LINK_BASE_URL + "brain-avatar");
                }
                //MessageBox.Show("Please install BrainAvatar first");
                return;
            }

            if(processManager.IsBrainAvatarRunning())
            {
                MessageBox.Show("BrainAvatar Already Running");
            } else
            {
                processManager.RunBrainAvatar(false);
            }
        }
        

        private void ButtonNeuroGuide(object sender, RoutedEventArgs e)
        {
            if(!processManager.IsNeuroGuideInstalled())
            {
                MessageBoxResult result = MessageBox.Show("Do you want to download NeuroGuide now?",
                                              "Download NeuroGuide",
                                              MessageBoxButton.YesNo,
                                              System.Windows.MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    Process.Start(LauncherJsonGenerator.LINK_BASE_URL + "neuro-guide");
                }

                return;
            }

            if(processManager.IsNeuroGuideRunning())
            {
                MessageBox.Show("NeuroGuide Already Running");
            } else
            {
                processManager.RunNeuroGuide(false);
            }
        }

        private void ButtonRefreshIP(object sender, RoutedEventArgs e)
        {
            textIpAddress.Text = " ... ";
            btnIpRefresh.IsEnabled = false;
            TimedAction.ExecuteWithDelay(new Action(delegate {
                ipDetect.RefreshIpMatchesDG();
            }), TimeSpan.FromSeconds(1.5));
        }

        public void IpDefaultGatewayNotFound()
        {
            textIpAddress.Text = "No Host";
            btnIpRefresh.IsEnabled = true;
        }

        public void IpAddressFound(string ipAddress)
        {
            textIpAddress.Text = ipAddress;
            btnIpRefresh.IsEnabled = true;
        }

        public void IpAddressNotFound()
        {
            textIpAddress.Text = "IP N/A";
            btnIpRefresh.IsEnabled = true;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            StopSerevr();

            try
            {
                processManager.KillBmFlashPlayer();

            } catch(Exception exc)
            {
                Debug.WriteLine("Error: " + exc.Message);
            }
        }
    }

}
