using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Updater
{
    class Helpers
    {
        public void Open<T>(string BackForm, bool Instanced) where T : UserControl, new()
        {
            T frm = new T();
            var targetWindow = Application.Current.Windows.Cast<Window>().FirstOrDefault(window => window is MainWindow) as MainWindow;
            targetWindow.GridMain.Children.Clear();
            targetWindow.GridMain.Children.Add(frm);
        }
    }
}
