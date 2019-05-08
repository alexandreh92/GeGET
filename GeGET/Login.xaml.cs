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
using System.Security;
using System.IO;
using SimpleImpersonation;

namespace GeGET
{
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
        string UpdaterLocation;
        string UpdaterFilename;
        #endregion

        #endregion

        #region Initialize
        public Login()
        {
            CheckUpdate();
            InitializeComponent();
            Remember_Data();
           // progressBar.Value = 0;
           // var bc = new BrushConverter();
           // progressBar.Background = (Brush)bc.ConvertFrom("#282828");
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
                CustomOKMessageBox.Show("Você não pode fechar o programa enquanto está fazendo update!", "Atenção!", Window.GetWindow(this));
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
                CustomOKMessageBox.Show("Credenciais inválidas!", "Atenção!", Window.GetWindow(this));
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

                var UpdaterLocationElement = doc.Descendants("UpdaterLocation");
                UpdaterLocation = string.Concat(UpdaterLocationElement.Nodes());

                var UpdaterFilenameElement = doc.Descendants("UpdaterFilename");
                UpdaterFilename = string.Concat(UpdaterFilenameElement.Nodes());

                if (Convert.ToInt32(cVersion) < oVersion)
                {
                    string oldfile = "Updater.exe";
                    string newfile = "Updater_Old.exe";
                    if (File.Exists(oldfile))
                    {
                        if (File.Exists(newfile))
                        {
                            File.Delete(newfile);
                        }
                        File.Move(oldfile,newfile);
                    }
                    if (File.Exists(newfile))
                    {
                        Process.Start(newfile);
                    }
                    else
                    {
                        StartUpdate();
                    }
                    this.Close();
                }
            }
            catch (Exception)
            {
            }
        }

        private void StartUpdate()
        {
            
            WebClient client = new WebClient();
            client.DownloadFileAsync(new Uri(UpdaterLocation), System.AppDomain.CurrentDomain.BaseDirectory + "/" + UpdaterFilename);
            client.DownloadFileCompleted += new AsyncCompletedEventHandler(Client_DownloadFileCompleted);
            client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(Client_DownloadProgressChanged);
        }
        private void Client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                Process.Start("Updater_Old.exe");
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
            
        }

        #endregion

        private void TxtPassword_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
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
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
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
        }
    }
}
