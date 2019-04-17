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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Expression.Drawing;
using Microsoft.Expression.Shapes;

namespace Teste
{
    /// <summary>
    /// Interaction logic for ProgressCircle.xaml
    /// </summary>
    public partial class ProgressCircle : UserControl
    {
        public static readonly DependencyProperty IndicatorBrushProperty = DependencyProperty.Register("IndicatorBrush", typeof(Brush), typeof(ProgressCircle));
        public Brush IndicatorBrush
        {
            get { return (Brush)this.GetValue(IndicatorBrushProperty); }
            set { this.SetValue(IndicatorBrushProperty, value); }
        }
        public static readonly DependencyProperty BackgroundBrushProperty = DependencyProperty.Register("BackgroundBrush", typeof(Brush), typeof(ProgressCircle));
        public Brush BackgroundBrush
        {
            get { return (Brush)this.GetValue(BackgroundBrushProperty); }
            set { this.SetValue(BackgroundBrushProperty, value); }
        }
        public static readonly DependencyProperty ProgressBorderBrushProperty = DependencyProperty.Register("ProgressBorderBrush", typeof(Brush), typeof(ProgressCircle));
        public Brush ProgressBorderBrush
        {
            get { return (Brush)this.GetValue(ProgressBorderBrushProperty); }
            set { this.SetValue(ProgressBorderBrushProperty, value); }
        }
        public static readonly DependencyProperty StrokeColorProperty = DependencyProperty.Register("StrokeColor", typeof(SolidColorBrush), typeof(ProgressCircle));
        public SolidColorBrush StrokeColor
        {
            get { return (SolidColorBrush)this.GetValue(StrokeColorProperty); }
            set { this.SetValue(StrokeColorProperty, value); }
        }
        public static readonly DependencyProperty StrokeThicknessProperty = DependencyProperty.Register("StrokeThickness", typeof(int), typeof(ProgressCircle));
        public int StrokeThickness
        {
            get { return (int)this.GetValue(StrokeThicknessProperty); }
            set { this.SetValue(StrokeThicknessProperty, value); }
        }
        public static readonly DependencyProperty ArcThicknessProperty = DependencyProperty.Register("ArcThickness", typeof(int), typeof(ProgressCircle));
        public int ArcThickness
        {
            get { return (int)this.GetValue(ArcThicknessProperty); }
            set { this.SetValue(ArcThicknessProperty, value); }
        }
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(int), typeof(ProgressCircle));
        public int Value
        {
            get { return (int)this.GetValue(ValueProperty); }
            set { this.SetValue(ValueProperty, value); }
        }
        public static readonly DependencyProperty TextSizeProperty = DependencyProperty.Register("TextSize", typeof(int), typeof(ProgressCircle));
        public int TextSize
        {
            get { return (int)this.GetValue(TextSizeProperty); }
            set { this.SetValue(TextSizeProperty, value); }
        }
        public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(ProgressCircle));
        public string Text
        {
            get { return (string)this.GetValue(TextProperty); }
            set { this.SetValue(TextProperty, value); }
        }
        public ProgressCircle()
        {
            InitializeComponent();
        }
    }
    [ValueConversion(typeof(int), typeof(double))]
    public class ValueToAngleConverter : IValueConverter
    {
        public object Convert(object value, Type t, object parameter, System.Globalization.CultureInfo culture)
        {
            return (double)(((int)value * 0.01) * 360);
        }

        public object ConvertBack(object value, Type t, object parameter, System.Globalization.CultureInfo culture)
        {
            return (double)((double)value / 360) * 100;
        }
    }
}
