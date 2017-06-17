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
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Threading;

namespace QRS_Detector
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<DataPoint> Points { get; private set; }
        public ObservableCollection<DataPoint> Scatter { get; private set; }
        public EKG signal = new EKG();
        public static Signal sig = null;
        public double chartWidth;
        public SolidColorBrush myAzure = new SolidColorBrush(Color.FromRgb(0x84, 0xce, 0xff));
        public SolidColorBrush myBlack = new SolidColorBrush(Color.FromRgb(0, 0, 0));
        public SolidColorBrush myWhite = new SolidColorBrush(Color.FromRgb(0xf0, 0xf0, 0xf0));
        public SolidColorBrush myMediumDark = new SolidColorBrush(Color.FromRgb(0x2d, 0x2d, 0x2d));
        public SolidColorBrush myPurple = new SolidColorBrush(Color.FromRgb(0x68, 0x21, 0x7a));
        public List<Task<Signal>> tasks = new List<Task<Signal>>();
        int blocks = 0;
        public bool isDrawed=false;
        public bool isDetected = false;

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
            Button1.IsEnabled=false;
            Button2.IsEnabled = false;
            lSave.IsEnabled = false;
        }

        //******************************************************************************************************************************/

        Func<object, Signal> action = (object obj) =>
        {
            var counter = (int)obj;
            var sig2 = new Signal();
            if (counter * 16384 + 16384 < sig.GetSignal().Count)
                sig2.SetSignal(sig.GetSignal().GetRange(counter * 16384, 16384).Clone());
            else
                sig2.SetSignal(sig.GetSignal().GetRange(counter * 16384, sig.GetSignal().Count - counter * 16384).Clone());
            var copy = sig2.GetSignal().Clone();
            sig2.FFT();
            sig2.BandPassFilter(1000, 5, 15);
            sig2.IFFT();
            sig2.Derivative();
            sig2.Square();
           // var copy4 = sig2.GetSignal().Clone();
            var frame = new List<DataPoint>();
            var value = Math.Round(0.15 * 1000);
            for (var i = 0; i < value; ++i)
            {
                frame.Add(new DataPoint(i, 1 / value));
            }
            sig2.Conv(frame);

           // var copy2 = sig2.GetSignal().Clone();
            var thresh = sig2.mean();
            sig2.findRisingEdge(thresh, 1000);
            var delay = ((int)(0.15 * 1000 / 2));
            sig2.findFrames(30);
            var min = sig.GetSignal().Min(item => item.mV);
            var max = sig.GetSignal().Max(item => item.mV);
            sig2.framesToSignal(min, max);
           // var copy3 = sig2.GetSignal().Clone();
            sig2.findQRS();
            Console.WriteLine("end: " + counter);
            return sig2;
        };

        private async void Button1_Click(object sender, RoutedEventArgs e)
        {
            if (!isDrawed)
            {
                Points = new ObservableCollection<DataPoint>(sig.GetSignal());
                setLineSeries(Points, myAzure);
                Scatter = new ObservableCollection<DataPoint>();
                setScatterSeries(Scatter, 15d, myPurple);
                setChartWidth(5d);
                drawPoints();
                isDrawed = true;
            }
        }

        public void drawPoints()
        {
            for (var j = 0; j < blocks; ++j)
            {
                var sig2 = tasks[j].Result;

                //Points = new ObservableCollection<DataPoint>(sig2.copy2);
                //setLineSeries(Points, myWhite);
                //Points = new ObservableCollection<DataPoint>(sig2.GetSignal());
                //setLineSeries(Points, myWhite);
                //Points = new ObservableCollection<DataPoint>(tmp.GetSignal());
                //setLineSeries(Points, myBlack);

                if (sig2.R != null)
                {
                    Scatter = new ObservableCollection<DataPoint>(sig2.R);
                    setScatterSeries(Scatter, 8d, myPurple);
                    Scatter = new ObservableCollection<DataPoint>(sig2.Q);
                    setScatterSeries(Scatter, 8d, myWhite);
                    Scatter = new ObservableCollection<DataPoint>(sig2.S);
                    setScatterSeries(Scatter, 8d, myBlack);
                }
            }
            setChartWidth(5d);
        }

        private async void Button2_Click(object sender, RoutedEventArgs e)
        {
            if (!isDetected)
            {
                blocks = (int)Math.Ceiling((decimal)sig.GetSignal().Count / 16384);
                Console.WriteLine(blocks);
                for (var j = 0; j < blocks; ++j)
                {
                    int counter = j;
                    tasks.Add(Task<Signal>.Factory.StartNew(action, counter));
                }
                Task.WaitAll(tasks.ToArray());

                for (var j = 0; j < blocks; ++j)
                {
                    var sig2 = tasks[j].Result;
                    for (int i = 0; i < sig2.R.Count(); ++i)
                    {
                        dataGrid.Items.Add(new { Q = sig2.Q[i].Time, R = sig2.R[i].Time, S = sig2.S[i].Time });
                        sig.Start.Add(sig2.Start[i]);
                        sig.End.Add(sig2.End[i]);
                        sig.Q.Add(sig2.Q[i]);
                        sig.R.Add(sig2.R[i]);
                        sig.S.Add(sig2.S[i]);
                    }
                }
                if (isDrawed)
                    drawPoints();
                isDetected = true;
                lSave.IsEnabled = true;
            }
        }

        private async void Load_Click(object sender, RoutedEventArgs e)
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

                    dataGrid.Items.Clear();
                    Chart1.Series.Clear();
                    isDrawed = false;
                    isDetected = false;
                    tasks.Clear();
                    blocks = 0;
                    Button1.IsEnabled = true;
                    Button2.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd wczytania");
                Console.WriteLine(ex.Message);
            }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var okienko = new SaveFileDialog();
                okienko.Filter = "Pliki (txt)|*.txt";
                if (okienko.ShowDialog() == true && okienko.FileName != "")
                {
                    tbPath2.Text = okienko.FileName;
                    using (StreamWriter sw = new StreamWriter(tbPath2.Text))
                    {
                        sw.WriteLine("---------------------------------------------WYNIKI DETEKCJI ZESPOŁÓW QRS W SYGNALE EKG------------------------------------------");
                        sw.WriteLine("");
                        sw.WriteLine("---------------------------------------------------------------------------------------------------------------------------------");
                        sw.WriteLine("|\t\t|\t\tPoczątek\t\t|\t\t\tQ\t\t\t|\t\t\tR\t\t\t|\t\t\tS\t\t\t|\t\tKoniec\t\t\t|");
                        sw.WriteLine("---------------------------------------------------------------------------------------------------------------------------------");
                        sw.WriteLine("|\tlp.\t|\t[s]\t\t|\t[mV]\t|\t[s]\t\t|\t[mV]\t|\t[s]\t\t|\t[mV]\t|\t[s]\t\t|\t[mV]\t|\t[s]\t\t|\t[mV]\t|");
                        sw.WriteLine("---------------------------------------------------------------------------------------------------------------------------------");
                        for(int i=0; i<sig.Start.Count;++i)
                        {
                            var one = sig.Start[i];
                            var two = sig.Q[i];
                            var three = sig.R[i];
                            var four = sig.S[i];
                            var five = sig.End[i];
                            sw.WriteLine("|\t"+(i+1)+"\t|\t"+ String.Format("{0:N2}", one.Time) +"\t|\t"+ String.Format("{0:N3}", one.mV) +"\t|\t"+ String.Format("{0:N2}", two.Time) +"\t|\t"+ String.Format("{0:N3}", two.mV) +"\t|\t"+ String.Format("{0:N2}", three.Time) +"\t|\t"+ String.Format("{0:N3}", three.mV) +"\t|\t"+ String.Format("{0:N2}", four.Time) +"\t|\t"+ String.Format("{0:N3}", four.mV) +"\t|\t"+ String.Format("{0:N2}", five.Time) +"\t|\t"+ String.Format("{0:N3}", five.mV) + "\t|");
                        }
                        sw.WriteLine("---------------------------------------------------------------------------------------------------------------------------------");
                    }
                }
                MessageBox.Show("Zapisano");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd zapisu "+ex.Message);
            }
        }

        private void Plus_Click(object sender, RoutedEventArgs e)
        {
            Chart1.Width += (int)Math.Round(0.1*Chart1.ActualWidth);
        }

        private void Minus_Click(object sender, RoutedEventArgs e)
        {
            if (Chart1.Width >= Chart1.ActualHeight + (int)Math.Round(0.1 * Chart1.ActualWidth))
                Chart1.Width -= (int)Math.Round(0.1 * Chart1.ActualWidth);
            else
                Chart1.Width = Chart1.ActualHeight;
        }

        //******************************************************************************************************************************/

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
            pointStyle.Setters.Add(new Setter(BackgroundProperty, plotColor));

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

        //******************************************************************************************************************************/

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
