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
using Ionic.Zip;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Diagnostics;
using System.Threading;

namespace Updater
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        Helpers helpers = new Helpers();

        public MainWindow()
        {
            InitializeComponent();
            helpers.Open<Intro>("", false);
            /*new Thread(() => 
            {
                DirectoryInfo di = new DirectoryInfo(System.AppDomain.CurrentDomain.BaseDirectory);
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
                    string unpackDirectory = System.AppDomain.CurrentDomain.BaseDirectory;
                    using (ZipFile zip1 = ZipFile.Read(zipToUnpack))
                    {
                        foreach (ZipEntry e in zip1)
                        {
                            e.Extract(unpackDirectory, ExtractExistingFileAction.OverwriteSilently);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                    Process.GetCurrentProcess().Kill();
                }
                finally
                {
                    ProcessStartInfo info = new ProcessStartInfo("GeGET.exe");
                    info.UseShellExecute = true;
                    info.Verb = "runas";
                    Process.Start(info);
                    Process.GetCurrentProcess().Kill();
                }
            }).Start();*/
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
