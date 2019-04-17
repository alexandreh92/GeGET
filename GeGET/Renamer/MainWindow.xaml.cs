using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Renamer
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            string oldfile = "Updater.exe";
            string newfile = "Updater_Old.exe";

            if (File.Exists(oldfile))
            {
                if (File.Exists(newfile))
                {
                    File.Delete(newfile);
                }
                File.Move(oldfile, newfile);
            }
            this.Close();
        }
    }
}
