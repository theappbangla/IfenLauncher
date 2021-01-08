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
using System.Windows.Shapes;
using IfenLauncher.GameLoader;
using IfenLauncher.Model;
using IfenLauncher.Utils;
using System.Net.Http;
using System.Diagnostics;
using System.Net;
using System.IO;
using Newtonsoft.Json;

namespace IfenLauncher
{
    /// <summary>
    /// Interaction logic for LicenseWindow.xaml
    /// </summary>
    public partial class LicenseWindow : Window
    {
        LauncherJson launcherJson;
        Action loginSuccessAction;

        public LicenseWindow(LauncherJson launcherJson, Action loginSuccessAction)
        {
            InitializeComponent();
            this.launcherJson = launcherJson;
            this.loginSuccessAction = loginSuccessAction;

            if (Properties.Settings.Default.advancedDimmer == true)
            {
                OnLoginSuccess();
                return;
            }

            textTitle.Text = "License Verification (" + launcherJson.name + ")";

            Show();
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                this.DragMove();
        }

        private void OnClickClose(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void OnClickLogin(object sender, RoutedEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                textLoading.Text = "Please wait ...";
                btnLogin.IsEnabled = false;
            });

            string email = boxEmail.Text;
            string password = boxPassword.Password;
            /*string email = "mubin986@gmail.com";
            string password = "mubin986";*/

            LoginUser loginUser = new LoginUser();
            loginUser.email = email;
            loginUser.password = password;
            loginUser.gameid = launcherJson.id;
            loginUser.pcid = new UniqueKeyWindows().GetUniqueKey() ;

            VerifyLoginUserAsync(loginUser);
        }

        private string url = "https://ifen-game-verification.appspot.com/login";
        //private string url = "http://localhost:8080/login";

        private void VerifyLoginUserAsync(LoginUser loginUser)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json = loginUser.GetJson();
                Debug.WriteLine(json);

                streamWriter.Write(json);
            }

            try
            {
                var httpResponse = httpWebRequest.GetResponse();
                Debug.WriteLine(httpResponse);
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    // login success --> code: 200

                    MessageBox.Show("Thank you for purchasing.", "Verification Successful");

                    OnLoginSuccess();
                }
            }
            catch (WebException ex)
            {
                using (var stream = ex.Response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    ShowLoginError(reader.ReadToEnd());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("unknown error: " + ex.Message);
                ShowLoginError(null);
            }

        }

        private void ShowLoginError(string json)
        {
            textLoading.Text = "";
            btnLogin.IsEnabled = true;

            if (json == null)
            {
                MessageBox.Show("Please Try Again", "Verification Failed");
                return;
            }

            Response response = JsonConvert.DeserializeObject<Response>(json);
            MessageBox.Show(response.info, "Verification Failed");
        }

        private void OnLoginSuccess()
        {
            loginSuccessAction();
            Properties.Settings.Default.advancedDimmer = true;
            Properties.Settings.Default.Save();
            Hide();
        }
    }
}
