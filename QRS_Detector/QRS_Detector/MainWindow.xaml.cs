using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Controls.DataVisualization.Charting;
using System.Collections.ObjectModel;
using System.Windows.Controls.DataVisualization;
using Microsoft.Win32;
using System.IO;
using System.Text;
using WPF.Bootstrap.Controls;
using System.Windows.Media;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace QRS_Detector
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<DataPoint> Points { get; private set; }
        public ObservableCollection<DataPoint> Scatter { get; private set; }
        public EKG signal = new EKG();
        public Signal sig = null;
        public double chartWidth;
        public SolidColorBrush myAzure = new SolidColorBrush(Color.FromRgb(0x84, 0xce, 0xff));
        public SolidColorBrush myMediumDark = new SolidColorBrush(Color.FromRgb(0x2d, 0x2d, 0x2d));
        public SolidColorBrush myPurple = new SolidColorBrush(Color.FromRgb(0x68, 0x21, 0x7a));

        public MainWindow()
        {
            InitializeComponent();
            Points = new ObservableCollection<DataPoint>();
            Scatter = new ObservableCollection<DataPoint>();
            this.MinWidth = 600;
            this.MinHeight = 550;
            this.Loaded += new System.Windows.RoutedEventHandler(this.AfterLoaded);
        }

        private void AfterLoaded(object sender, EventArgs e)
        {
            Console.WriteLine(Chart1.ActualWidth);
            chartWidth = Chart1.ActualWidth;
            setChart(myMediumDark);
        }

        private void Load_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var okienko = new OpenFileDialog();
                okienko.Filter = "Pliki (txt)|*.txt";
                if (okienko.ShowDialog() == true)
                {
                    tbPath.Text = okienko.FileName;
                    signal = new EKG();
                    string readText = "";
                    if (File.Exists(tbPath.Text))
                    {
                        readText = File.ReadAllText(tbPath.Text);
                    }
                    signal.readSignalsFromText(readText);
                    sig = signal.averaging();

                    Chart1.Series.Clear();
                    Points = new ObservableCollection<DataPoint>(sig.GetSignal());
                    setLineSeries(Points, myAzure);
                    Scatter = new ObservableCollection<DataPoint>();
                    setScatterSeries(Scatter, 15d, myPurple);
                    setChartWidth(5d);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd wczytania");
                Console.WriteLine(ex.Message);
            }
        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            Scatter.Add(new DataPoint(0.2, 0.4));
            Scatter.Add(new DataPoint(0.6, 0.3));
        }

        private void Button2_Click(object sender, RoutedEventArgs e)
        {
            var copy = sig.GetSignal().Clone();
            sig.FFT();
            sig.BandPassFilter(1000, 5, 15);
            sig.IFFT();
            sig.Derivative();
            sig.Square();
            sig.Frame(1000);
            var thresh = sig.mean();
            sig.aboveThreshold(thresh);
            var delay = ((int)(0.15 * 1000 / 2));
            sig.findFrames(delay);
            sig.findQRS();

            dataGrid.Items.Clear();
            for (int i = 0; i < sig.R.Count(); ++i)
            {
                dataGrid.Items.Add(new { Q = sig.Q[i].Time, R = sig.R[i].Time, S = sig.S[i].Time });
            }

            Chart1.Series.Clear();
            Points = new ObservableCollection<DataPoint>(copy);
            setLineSeries(Points, myAzure);

            if (sig.R != null)
            {
                Scatter = new ObservableCollection<DataPoint>(sig.R);
                setScatterSeries(Scatter, 15d, myPurple);
                Scatter = new ObservableCollection<DataPoint>(sig.Q);
                setScatterSeries(Scatter, 15d, myPurple);
                Scatter = new ObservableCollection<DataPoint>(sig.S);
                setScatterSeries(Scatter, 15d, myPurple);
            }
            setChartWidth(5d);
        }

        private void Plus_Click(object sender, RoutedEventArgs e)
        {
            Chart1.Width += 50;
        }

        private void Minus_Click(object sender, RoutedEventArgs e)
        {
            if (Chart1.Width >= Chart1.ActualHeight + 50)
                Chart1.Width -= 50;
            else
                Chart1.Width = Chart1.ActualHeight;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var okienko = new SaveFileDialog();
                okienko.Filter = "Pliki (txt)|*.txt";
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

        void setChart(SolidColorBrush gridColor)
        {
            var gridStyle = new Style(typeof(Line));
            gridStyle.Setters.Add(new Setter(Line.StrokeProperty, gridColor));
            var axisY = new LinearAxis { Orientation = AxisOrientation.Y, Title = "Voltage [mV]", ShowGridLines = true, GridLineStyle = gridStyle };
            var axisX = new LinearAxis { Orientation = AxisOrientation.X, Title = "Time [s]", ShowGridLines = true, GridLineStyle = gridStyle };

            var HideLegendStyle = new Style(typeof(Legend));
            HideLegendStyle.Setters.Add(new Setter(Legend.WidthProperty, 0.0));
            HideLegendStyle.Setters.Add(new Setter(Legend.HeightProperty, 0.0));
            HideLegendStyle.Setters.Add(new Setter(Legend.VisibilityProperty, Visibility.Collapsed));

            Chart1.Axes.Add(axisX);
            Chart1.Axes.Add(axisY);
            Chart1.LegendStyle = HideLegendStyle;
        }

        void setChartWidth(double XMaxValue)
        {
            Console.WriteLine("actualyWidth: " + chartWidth);
            if (Points.Count != 0)
            {
                var width = (double)Points[Points.Count - 1].Time / XMaxValue * (chartWidth) - 50;
                if (width > chartWidth)
                    Chart1.Width = width;
                else
                    Chart1.Width = svChart.ActualWidth;
            }
        }

        void setLineSeries(ObservableCollection<DataPoint> Points, SolidColorBrush plotColor)
        {
            var style = new Style(typeof(Polyline));
            style.Setters.Add(new Setter(Polyline.StrokeThicknessProperty, 1d));

            var pointStyle = new Style(typeof(LineDataPoint));
            pointStyle.Setters.Add(new Setter(LineDataPoint.TemplateProperty, null));
            pointStyle.Setters.Add(new Setter(BackgroundProperty, plotColor));

            var series = new LineSeries
            {
                PolylineStyle = style,
                ItemsSource = Points,
                DependentValuePath = "mV",
                IndependentValuePath = "Time",
                DataPointStyle = pointStyle,
                LegendItemStyle = null,
            };

            Chart1.Series.Add(series);
        }

        void setScatterSeries(ObservableCollection<DataPoint> Points, double size, SolidColorBrush plotColor)
        {
            var pointStyle = new Style(typeof(ScatterDataPoint));
            pointStyle.Setters.Add(new Setter(BorderBrushProperty, myAzure));
            pointStyle.Setters.Add(new Setter(WidthProperty, size));
            pointStyle.Setters.Add(new Setter(HeightProperty, size));
            pointStyle.Setters.Add(new Setter(BackgroundProperty, myPurple));

            var series = new ScatterSeries
            {
                ItemsSource = Points,
                DependentValuePath = "mV",
                IndependentValuePath = "Time",
                DataPointStyle = pointStyle,
                LegendItemStyle = null,
            };

            Chart1.Series.Add(series);
        }

        private async void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                if (e.ClickCount == 2)
                {
                    AdjustWindowSize();
                }
                else
                {
                    Application.Current.MainWindow.DragMove();
                }
        }


        private async void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }


        private async void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            AdjustWindowSize();
        }


        private async void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }


        private async void AdjustWindowSize()
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
                Uri resourceUri = new Uri("Img/max.png", UriKind.Relative);
                StreamResourceInfo streamInfo = Application.GetResourceStream(resourceUri);

                BitmapFrame temp = BitmapFrame.Create(streamInfo.Stream);
                var brush = new ImageBrush();
                brush.ImageSource = temp;
                MaxButton.Background = brush;

            }
            else
            {
                this.WindowState = WindowState.Maximized;
                Uri resourceUri = new Uri("Img/min.png", UriKind.Relative);
                StreamResourceInfo streamInfo = Application.GetResourceStream(resourceUri);

                BitmapFrame temp = BitmapFrame.Create(streamInfo.Stream);
                var brush = new ImageBrush();
                brush.ImageSource = temp;
                MaxButton.Background = brush;
            }

        }
    }
}
