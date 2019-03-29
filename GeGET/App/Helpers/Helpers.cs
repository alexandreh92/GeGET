using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;
using DevExpress.Xpf.Grid;

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

    class CommitHelper
    {
        public static readonly DependencyProperty CommitOnValueChangedProperty = DependencyProperty.RegisterAttached("CommitOnValueChanged", typeof(bool), typeof(CommitHelper), new PropertyMetadata(CommitOnValueChangedPropertyChanged));

        public static void SetCommitOnValueChanged(GridColumn element, bool value)
        {
            element.SetValue(CommitOnValueChangedProperty, value);
        }

        public static bool GetCommitOnValueChanged(GridColumn element)
        {
            return (bool)element.GetValue(CommitOnValueChangedProperty);
        }

        private static void CommitOnValueChangedPropertyChanged(DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            GridColumn col = source as GridColumn;
            if (col.View == null)
                Dispatcher.CurrentDispatcher.BeginInvoke(new Action<GridColumn, bool>((column, subscribe) => {
                    ToggleCellValueChanging(column, subscribe);
                }), col, (bool)e.NewValue);
            else
                ToggleCellValueChanging(col, (bool)e.NewValue);
        }

        private static void ToggleCellValueChanging(GridColumn col, bool subscribe)
        {
            TableView view = col.View as TableView;
            if (view == null)
                return;

            if (subscribe)
                view.CellValueChanging += new CellValueChangedEventHandler(view_CellValueChanging);
            else
                view.CellValueChanging -= view_CellValueChanging;
        }

        static void view_CellValueChanging(object sender, CellValueChangedEventArgs e)
        {
            TableView view = sender as TableView;
            if ((bool)e.Column.GetValue(CommitOnValueChangedProperty))
                view.PostEditor();
        }
    }

}
