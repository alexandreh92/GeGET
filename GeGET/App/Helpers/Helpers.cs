using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace GeGET
{
    class Helpers
    {
        private static string back;
        private static string instanced_Back;

        public string Back { get => back; set => back = value; }
        public string Instanced_Back { get => instanced_Back; set => instanced_Back = value; }

        public void Close()
        {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                var targetWindow = Application.Current.Windows.Cast<Window>().FirstOrDefault(window => window is Layout) as Layout;
                targetWindow.GridMain.Children.Clear();
            }
            ));
        }
        public void Open<T>(string BackForm, bool Instanced) where T : UserControl, new()
        {
            if (Instanced)
            {
                instanced_Back = BackForm;
            }
            else
            {
                back = BackForm;
            }
            T frm = new T();
            var targetWindow = Application.Current.Windows.Cast<Window>().FirstOrDefault(window => window is Layout) as Layout;
            targetWindow.GridMain.Children.Clear();
            targetWindow.GridMain.Children.Add(frm);
        }

        public void OpenBack(bool Instanced)
        {
            Type type;
            if (Instanced)
            {
                type = Type.GetType("GeGET." + instanced_Back);
            }
            else
            {
                type = Type.GetType("GeGET." + back);
            }
            Type.GetType("GeGET." + back);
            UserControl ctrl = (UserControl)Activator.CreateInstance(type);
            var targetWindow = Application.Current.Windows.Cast<Window>().FirstOrDefault(window => window is Layout) as Layout;
            targetWindow.GridMain.Children.Clear();
            targetWindow.GridMain.Children.Add(ctrl);
        }


    }
}
