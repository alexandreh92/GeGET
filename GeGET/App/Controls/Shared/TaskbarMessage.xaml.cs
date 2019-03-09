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

namespace GeGET
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class TaskbarMessage : Window
    {
        double value = 100;
        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();


        public TaskbarMessage(string Titulo, string Description)
        {
            InitializeComponent();
            txtTitulo.Text = Titulo;
            txtDescription.Text = Description;
            var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
            WindowStartupLocation = WindowStartupLocation.Manual;
            Top = desktopWorkingArea.Bottom - this.Height;
            Left = desktopWorkingArea.Right - this.Width;
            dispatcherTimer.Tick += new EventHandler(DispatcherTimer_Tick);
            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(10);
            dispatcherTimer.Start();
        }

        private void Init()
        {
            progressBar.Value = value;
            
        }

        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            if (value > 0)
            {
                progressBar.Value = value;
                value = value - 0.1;
            }
            else
            {
                dispatcherTimer.Stop();
                this.Close();
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
