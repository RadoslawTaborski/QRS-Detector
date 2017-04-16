using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Controls.DataVisualization.Charting;
using System.Collections.ObjectModel;
using AForge.Math;
using System.Windows.Controls.DataVisualization;
using Microsoft.Win32;
using System.IO;
using System.Text;

namespace QRS_Detector
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<DataPoint> Points { get; private set; }

        public MainWindow()
        {
            InitializeComponent();
            this.MinWidth = 550;
            this.MinHeight = 540;
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var okienko = new OpenFileDialog();
                okienko.Filter = "Pliki (wav)|*.wav";
                if (okienko.ShowDialog() == true)
                {
                    Chart1.Series.Clear();
                    Chart1.Axes.Clear();

                    tbPath.Text = okienko.FileName;
                    Points = new ObservableCollection<DataPoint>();
                    byte[] bytes = File.ReadAllBytes(okienko.FileName);

                    var tabCom = new Complex[1024];

                    /*for (int i = 0; i < 1024; i++)
                    {
                        tabCom[i] = new Complex(wav.Data[0, i], 0);
                    }*/

                   // FourierTransform.FFT(tabCom, FourierTransform.Direction.Forward);

                    /*for (int i = 0; i < 512; i++)
                    {
                        Points.Add(new DataPoint() { X = (wav.SampleRate * i) / 511, Y = tabCom[i].Magnitude * 1000 });
                    }*/

                    var style = new Style(typeof(Polyline));
                    style.Setters.Add(new Setter(Polyline.StrokeThicknessProperty, 1d));

                    var pointStyle = new Style(typeof(LineDataPoint));
                    pointStyle.Setters.Add(new Setter(LineDataPoint.TemplateProperty, null));

                    var HideLegendStyle = new Style(typeof(Legend));
                    HideLegendStyle.Setters.Add(new Setter(Legend.WidthProperty, 0.0));
                    HideLegendStyle.Setters.Add(new Setter(Legend.HeightProperty, 0.0));
                    HideLegendStyle.Setters.Add(new Setter(Legend.VisibilityProperty, Visibility.Collapsed));

                    var series = new LineSeries
                    {
                        PolylineStyle = style,
                        ItemsSource = Points,
                        DependentValuePath = "Y",
                        IndependentValuePath = "X",
                        DataPointStyle = pointStyle,
                        LegendItemStyle = null,
                    };

                    var axisY = new LinearAxis { Orientation = AxisOrientation.Y, Title = "Amplitude", ShowGridLines = true, };
                    var axisX = new LinearAxis { Orientation = AxisOrientation.X, Title = "Frequence[Hz]", ShowGridLines = true, };

                    Chart1.Series.Add(series);
                    Chart1.Axes.Add(axisX);
                    Chart1.Axes.Add(axisY);
                    Chart1.LegendStyle = HideLegendStyle;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Błąd wczytania");
            }
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var okienko = new SaveFileDialog();
                okienko.Filter = "Pliki (wav)|*.wav";
                if (okienko.ShowDialog() == true && okienko.FileName != "")
                {
                   /* tbPath2.Text = okienko.FileName;
                    File.WriteAllBytes(okienko.FileName, wav.HeaderByte);
                    AppendAllBytes(okienko.FileName, wav.DataByte);*/
                }
                MessageBox.Show("Zapisano");
            }
            catch (Exception)
            {
                MessageBox.Show("Błąd zapisu");
            }
        }

        public static void AppendAllBytes(string path, byte[] bytes)
        {
            using (var stream = new FileStream(path, FileMode.Append))
            {
                stream.Write(bytes, 0, bytes.Length);
            }
        }
    }
}
