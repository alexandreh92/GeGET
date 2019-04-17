using System;
using System.Windows;
using System.Windows.Media;
using DTO;
using BLL;
using System.Xml.Linq;
using System.Reflection;
using System.Net;
using System.ComponentModel;
using System.Diagnostics;

namespace GeGET
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
       
        #region Declarations
        LoginDTO dto = new LoginDTO();
        LoginBLL bll = new LoginBLL();

        #region Updater Declarations
        private static string cVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString().Replace(".","");
        private static int oVersion;
        private static string FileLocation;
        private static string Filename;
        public static bool HaveUpdate;
        #endregion

        #endregion

        #region Initialize
        public Login()
        {
            InitializeComponent();
            CheckUpdate();
            Remember_Data();
            progressBar.Value = 0;
            var bc = new BrushConverter();
            progressBar.Background = (Brush)bc.ConvertFrom("#282828");
            txtVersion.Text = "version" + Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }
        #endregion

        #region Events

        #region Clicks

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            if (!HaveUpdate)
            {
                Application.Current.Shutdown();
            }
            else
            {
                MessageBox.Show("Você não pode fechar o programa enquanto está fazendo update!");
            }
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            dto.Usuario = txtLogin.Text;
            dto.Senha = txtPassword.Password;
            Save_Data();
            if (bll.Login(dto))
            {
                Layout mw = new Layout();
                mw.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Usuário ou senha inválidos.");
            }
        }

        #endregion

        #endregion

        #region Properties

        private void Remember_Data()
        {
            if (Properties.Settings.Default.UserName != string.Empty)
            {
                if (Properties.Settings.Default.Remme == "yes")
                {
                    txtLogin.Text = Properties.Settings.Default.UserName;
                    txtPassword.Password = Properties.Settings.Default.Password;
                    toggleRemme.IsChecked = true;
                }
                else
                {
                    txtLogin.Text = Properties.Settings.Default.UserName;
                }
            }
        }

        private void Save_Data()
        {
            if (Convert.ToBoolean(toggleRemme.IsChecked))
            {
                Properties.Settings.Default.UserName = txtLogin.Text;
                Properties.Settings.Default.Password = txtPassword.Password;
                Properties.Settings.Default.Remme = "yes";
                Properties.Settings.Default.Save();
            }
            else
            {
                Properties.Settings.Default.UserName = txtLogin.Text;
                Properties.Settings.Default.Password = txtPassword.Password;
                Properties.Settings.Default.Remme = "no";
                Properties.Settings.Default.Save();
            }
        }

        #endregion

        #region Updater

        private void CheckUpdate()
        {
            try
            {
                XDocument doc2 = XDocument.Parse(Properties.Resources.updateurl);

                var Mirror = doc2.Descendants("Mirror");
                var mirrorurl = string.Concat(Mirror.Nodes());

                XDocument doc = XDocument.Load(mirrorurl);

                var VersionElement = doc.Descendants("Version");
                oVersion = Convert.ToInt32(string.Concat(VersionElement.Nodes()).Replace(".", ""));

                var LocationElement = doc.Descendants("FileLocation");
                FileLocation = string.Concat(LocationElement.Nodes());

                var FilenameElement = doc.Descendants("Filename");
                Filename = string.Concat(FilenameElement.Nodes());


                if (Convert.ToInt32(cVersion) < oVersion)
                {
                    HaveUpdate = true;
                    btnLogin.IsEnabled = false;
                    UpdateFields.Visibility = Visibility.Visible;
                    StartUpdate();
                }
            }
            catch (Exception)
            {
            }
        }

        private void StartUpdate()
        {
            lblInfo.Text = "Fazendo Download...";
            WebClient client = new WebClient();
            client.DownloadFileAsync(new Uri(FileLocation), System.AppDomain.CurrentDomain.BaseDirectory + "/" + Filename);
            client.DownloadFileCompleted += new AsyncCompletedEventHandler(Client_DownloadFileCompleted);
            client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(Client_DownloadProgressChanged);
        }
        private void Client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                lblInfo.Text = "Download completo! Instalando...";
                HaveUpdate = false;
                Process.Start("Updater.exe");
                Application.Current.Shutdown();
            }
            else
            {
                MessageBox.Show(e.Error.Message);
            }
            ((WebClient)sender).Dispose();
        }

        private void Client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            var total = (int)e.TotalBytesToReceive;
            var recieved = (int)e.BytesReceived;
            int current = 1/(total/recieved)*100;
            progressBar.Value = current;
        }

        #endregion

    }
}
