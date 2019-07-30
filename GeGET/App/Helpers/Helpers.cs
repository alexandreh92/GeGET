using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
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
                if (targetWindow.actionTabs.SelectedItem != null)
                {
                    var tab = targetWindow.actionTabs.SelectedItem as TabItem;
                    targetWindow.actionTabs.Items.Remove(tab);
                }
            }
            ));
        }
        

        public void OpenTab<T>(object sender, MouseButtonEventArgs e, string BackForm, bool Instanced) where T : UserControl, new()
        {
            if (Instanced)
            {
                instanced_Back = BackForm;
            }
            else
            {
                back = BackForm;
            }
            var targetWindow = Application.Current.Windows.Cast<Window>().FirstOrDefault(window => window is Layout) as Layout;
            T frm = new T();
            if (e != null)
            {
                if (e.ChangedButton == MouseButton.Middle && e.ButtonState == MouseButtonState.Pressed)
                {
                    var tab = new TabItem()
                    {
                        Content = frm,
                        Header = frm.Tag,
                    };
                    targetWindow.actionTabs.Items.Add(tab);
                    targetWindow.actionTabs.SelectedItem = tab;
                }
                else if (e.ChangedButton == MouseButton.Left && e.ButtonState == MouseButtonState.Pressed)
                {
                    if (targetWindow.actionTabs.SelectedItem != null && targetWindow.actionTabs.Items.Count > 0)
                    {
                        var tab = targetWindow.actionTabs.SelectedItem as TabItem;
                        targetWindow.actionTabs.Items.Remove(tab);
                        var newtab = new TabItem()
                        {
                            Content = frm,
                            Header = frm.Tag
                        };
                        targetWindow.actionTabs.Items.Add(newtab);
                        targetWindow.actionTabs.SelectedItem = newtab;
                    }
                    else
                    {
                        var tab = new TabItem()
                        {
                            Content = frm,
                            Header = frm.Tag
                        };
                        targetWindow.actionTabs.Items.Add(tab);
                        targetWindow.actionTabs.SelectedItem = tab;
                    }

                }
            }
            else
            {
                var tab = new TabItem()
                {
                    Content = frm,
                    Header = frm.Tag
                };
                targetWindow.actionTabs.Items.Add(tab);
                targetWindow.actionTabs.SelectedItem = tab;
            }
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
            if (targetWindow.actionTabs.SelectedItem != null)
            {
                var tab = targetWindow.actionTabs.SelectedItem as TabItem;
                targetWindow.actionTabs.Items.Remove(tab);

                var newTab = new TabItem()
                {
                    Content = ctrl,
                    Header = ctrl.Tag
                };
                targetWindow.actionTabs.Items.Add(newTab);
                targetWindow.actionTabs.SelectedItem = newTab;
            }
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
