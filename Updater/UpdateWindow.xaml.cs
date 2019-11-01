using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace Updater
{
    /// <summary>
    /// Interaction logic for UpdateWindow.xaml
    /// </summary>
    public partial class UpdateWindow : UserControl
    {
        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        private static int oVersion;
        private static string FileLocation;
        private static string Filename;

        public UpdateWindow()
        {
            InitializeComponent();
            slider.Children.Add(new Slide1());
            dispatcherTimer.Tick += new EventHandler(DispatcherTimer_Tick);
            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(5000);
            dispatcherTimer.Start();
        }

        private void ReadXML()
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
                progressBar.Value = 100;
                lblInfo.Text = "Download completo.";
                lblInfo.Text = "Instalando...";
                RemoveFiles();
                Extractor();
            }
            else
            {
                MessageBox.Show(e.Error.Message);
            }
            ((WebClient)sender).Dispose();
        }

        private void Client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        { 
            progressBar.Value = e.ProgressPercentage;
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            //add slide 2
            //slider.Children.Add(new Slide1());
            dispatcherTimer.Stop();
            ReadXML();
            StartUpdate();
        }

        private void RemoveFiles()
        {
            string[] fileEntries = Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory);
            foreach (string fileName in fileEntries)
            {
                try
                {
                    if (System.IO.Path.GetFileName(fileName) != "Deploy.zip" && System.IO.Path.GetFileName(fileName) != "Updater_Old.exe" && System.IO.Path.GetFileName(fileName) != "MaterialDesignThemes.Wpf.dll" && System.IO.Path.GetFileName(fileName) != "MaterialDesignColors.dll" && System.IO.Path.GetFileName(fileName) != "Ionic.Zip.dll" && System.IO.Path.GetFileName(fileName) != "CommonServiceLocator.dll")
                    {
                        File.Delete(fileName);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.ToString());
                }
            }
        }


        private void Extractor()
        {
            DirectoryInfo di = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
            FileInfo[] files = di.GetFiles("*.tmp")
                                 .Where(p => p.Extension == ".tmp").ToArray();
            foreach (FileInfo file in files)
                try
                {
                    file.Attributes = FileAttributes.Normal;
                    File.Delete(file.FullName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Erro no update. Contate um administrador!/nErro: " + ex);
                }
            try
            {


                string zipToUnpack = "Deploy.zip";
                string unpackDirectory = AppDomain.CurrentDomain.BaseDirectory;
                using (ZipFile zip1 = ZipFile.Read(zipToUnpack))
                {
                    foreach (ZipEntry e in zip1)
                    {
                        if (e.FileName != "Ionic.Zip.dll" && e.FileName != "MaterialDesignColors.dll" && e.FileName != "CommonServiceLocator.dll" && e.FileName != "MaterialDesignThemes.Wpf.dll" && e.FileName != "Updater_Old.exe")
                        {
                            e.Extract(unpackDirectory, ExtractExistingFileAction.OverwriteSilently);
                        }
                    }
                }
                
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
            finally
            {
                File.Delete("Deploy.zip");
                lblInfo.Text = "Concluído.";
                btnOpen.IsEnabled = true;
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Window.GetWindow(this).Close();
        }

        private void BtnOpen_Click(object sender, RoutedEventArgs e)
        {
            Process.Start("GeGET.exe");
            Window.GetWindow(this).Close();
        }
    }
}
