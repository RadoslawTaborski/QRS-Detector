using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AForge.Math;
using MoreLinq;

namespace QRS_Detector
{
    public class Signal
    {
        private List<DataPoint> signal;
        public List<DataPoint> copy;
        public List<DataPoint> copy2;
        public List<List<DataPoint>> peaki;
        private Complex[] fft;
        private List<Frame> frames;
        public List<DataPoint> Q;
        public List<DataPoint> R;
        public List<DataPoint> S;
        public List<DataPoint> Start;
        public List<DataPoint> End;
        public double Fs=0;

        public Signal()
        {
            signal = new List<DataPoint>();
            peaki = new List<List<DataPoint>>();
            Q = new List<DataPoint>();
            R = new List<DataPoint>();
            S = new List<DataPoint>();
            Start = new List<DataPoint>();
            End = new List<DataPoint>();
        }

        public Signal(Signal sig)
        {
            signal = new List<DataPoint>(sig.GetSignal());
            peaki = new List<List<DataPoint>>();
            Q = new List<DataPoint>();
            R = new List<DataPoint>();
            S = new List<DataPoint>();
            Start = new List<DataPoint>();
            End = new List<DataPoint>();
        }

        public List<DataPoint> GetSignal()
        {
            return signal;
        }

        public void SetSignal(List<DataPoint> value)
        {
            signal = value;
        }

        internal void AddDataPoint(DataPoint dataPoint)
        {
            signal.Add(dataPoint);
        }

        public void setFrequency()
        {
            if (signal.Count >= 1)
                Fs = (1 / (signal[1].Time-signal[0].Time));
        }

        public double maxAbsValue()
        {
            var max = 0d;
            for (var i = 0; i < signal.Count(); ++i)
            {
                if (Math.Abs(signal[i].mV) > max)
                    max = Math.Abs(signal[i].mV);
            }
            return max;
        }

        public double mean()
        {
            var result = 0d;
            for (var i = 0; i < signal.Count(); ++i)
            {
                result += signal[i].mV;
            }
            result /= signal.Count();

            return result;
        }

        public void HighPassFilter()
        {
            var tmpSig = new List<DataPoint>(signal);
            for (int i = 32; i < signal.Count; ++i)
            {
                signal[i].mV = 2 * signal[i - 1].mV - signal[i - 2].mV + tmpSig[i].mV - 2 * tmpSig[i - 6].mV + tmpSig[i - 12].mV;
            }
        }

        public void LowPassFilter()
        {
            var tmpSig = new List<DataPoint>(signal);
            for (int i = 12; i < signal.Count; ++i)
            {
                signal[i].mV = 2 * signal[i - 1].mV - signal[i - 2].mV + tmpSig[i].mV - 2 * tmpSig[i - 6].mV + tmpSig[i - 12].mV;
                Console.Write(signal[i].mV + " ");
            }
        }

        public void FFT()
        {
            double logLength = Math.Ceiling(Math.Log((double)signal.Count, 2.0));
            Console.WriteLine(logLength);
            int paddedLength = (int)Math.Pow(2.0, Math.Min(Math.Max(1.0, logLength), 14.0));
            Console.WriteLine(paddedLength);
            fft = new Complex[paddedLength];

            for (int i = 0; i < signal.Count(); i++) 
            {
                fft[i] = new Complex(signal[i].mV, 0);
            }

            FourierTransform.FFT(fft, FourierTransform.Direction.Forward);
        }

        public void BandPassFilter(double a, double b)
        {
            if (fft != null)
            {
                double[] f = new double[fft.Count()];
                for (int i = 0; i < fft.Count(); ++i)
                {
                    f[i] = Fs * i / fft.Count();
                    if (f[i] < a || f[i] > b && f[i] < Fs - b || f[i] > Fs - a)
                    {
                        fft[i].Re = 0;
                        fft[i].Im = 0;
                    }
                }
            }
        }

        public void IFFT()
        {
            if (copy == null)
                copy = (List<DataPoint>)Extensions.Clone(signal);
            FourierTransform.FFT(fft, FourierTransform.Direction.Backward);

            for (var i = 0; i < signal.Count(); ++i) 
            {
                signal[i].mV = fft[i].Re;
            }

        }

        public void Derivative()
        {
            if (copy == null)
                copy = (List<DataPoint>)Extensions.Clone(signal);
            var tmp = Extensions.Clone(signal);
            signal[0].mV = 0;
            for (var i = 1; i < signal.Count(); ++i)
            {
                signal[i].mV = tmp[i].mV - tmp[i - 1].mV;
            }
        }

        public void Square()
        {
            if (copy == null)
                copy = (List<DataPoint>)Extensions.Clone(signal);
            for (var i = 0; i < signal.Count(); ++i)
            {
                signal[i].mV *= signal[i].mV;
            }
            var max = this.maxAbsValue();
            for (var i = 0; i < signal.Count(); ++i)
            {
                signal[i].mV /= max;
            }
        }

        public void Frame()
        {
            if (copy == null)
                copy = (List<DataPoint>)Extensions.Clone(signal);
            var tmp = Extensions.Clone(signal);
            var delay = Math.Round(0.15 * Fs);
            for (var k = 0; k < tmp.Count; ++k)
            {
                var sum = 0d;
                for (var i = 0; i <= delay; ++i)
                {
                    if (k - i >= 0)
                        sum += tmp[k - i].mV;
                }
                signal[k].mV = 1 / delay * sum;
            }
        }

        public void Conv(List<DataPoint> frame)
        {
            var startTime = signal[0].Time;
            var temp = new List<DataPoint>();
            var suma = 0d;
            int j = 0;
            var fs = signal[1].Time - signal[0].Time;
            for (var a = 0; a < signal.Count + frame.Count - 1; ++a)
            {
                suma = 0;
                j = a;
                for (var i = 0; i <= a; ++i)
                {
                    if ((j >= 0) && (j < frame.Count) && (i < signal.Count))
                    {
                        suma += signal[i].mV * frame[j].mV;
                    }
                    j--;
                }
                temp.Add(new DataPoint(startTime+a * fs, suma));
            }

            var max=temp.Max(x=>x.mV);
            for(var i=0; i < temp.Count; ++i)
            {
                temp[i].mV /= max;
            }
            signal = temp;
            copy2 = Extensions.Clone(temp);
        }

        public void findRisingEdge(double thresh)
        {       
            int item = signal.Count;
            var copy = Extensions.Clone(signal);
            signal.Select(signal => { signal.mV = 0; return signal; }).ToList();
            bool start = false;
            for (var i = 0; i < copy.Count(); ++i)
            {
                if (copy[i].mV > thresh && !start)
                {
                    List<DataPoint> sublist;
                    if (i + (int)Math.Round(0.15 * Fs) < copy.Count)
                        sublist = copy.GetRange(i, (int)Math.Round(0.15 * Fs));
                    else
                        sublist = copy.GetRange(i, copy.Count - i);
                    item = i + (!sublist.Any() ? -1 : sublist.Select((value, index) => new { Value = value, Index = index })
                                                        .Aggregate((a, b) => (a.Value.mV > b.Value.mV) ? a : b)
                                                        .Index);

                    for (var j = i; j <= item; ++j)
                    {
                        signal[j].mV = 1;
                    }
                    start = true;
                    i += sublist.Count - 1;
                }
                else if (copy[i].mV < thresh && start)
                {
                    start = false;
                    --i;
                }
            }
        }

        public void findFrames(int delay)
        {
            frames = new List<Frame>();
            var left = new List<int>();
            var right = new List<int>();
            for (var i = 1; i < signal.Count() - 1; ++i)
            {
                if (signal[i].mV - signal[i - 1].mV == 1)
                    left.Add(i);
                if (signal[i + 1].mV - signal[i].mV == -1)
                    right.Add(i);
            }
            if (right.Count() != 0 && left.Count() != 0)
            {
                if (right[1] < left[1])
                    right.Remove(1);
                if (left.Last() > right.Last())
                    left.Remove(left.Count() - 1);
            }
            for (var i = 0; i < left.Count(); ++i)
            {
                if(right[i]-left[i]>0.06*Fs)
                frames.Add(new Frame() { left = left[i] - delay, right = right[i] - delay });
            }
        }

        public void framesToSignal(double min, double max)
        {
            signal.Select(signal => { signal.mV = min; return signal; }).ToList();
            foreach (var item in frames)
            {
                if(item.left>0)
                for(var i=item.left; i <= item.right; ++i)
                {
                    signal[i].mV = max;
                }
            }
        }

        public void findQRS()
        {
            foreach (var frame in frames)
            {
                if (frame.left > 0 && frame.right < copy.Count)
                {
                    var sublist = copy.GetRange(frame.left, frame.right - frame.left);
                    var item = sublist.MaxBy(x => x.mV);
                    Start.Add(sublist[0]);
                    End.Add(sublist[sublist.Count - 1]);
                    R.Add(item);
                    var index = sublist.IndexOf(item);
                    var subsublist = sublist.GetRange(0, index);
                    item = subsublist.MinBy(x => x.mV);
                    Q.Add(item);
                    subsublist = sublist.GetRange(index, sublist.Count() - index);
                    item = subsublist.MinBy(x => x.mV);
                    S.Add(item);
                }
            }
        }

        public void deleteFailQRS()
        {
            if (R.Count != 0 && R != null)
            {
                var max = R.Max(x => x.mV);
                int index = R.FindIndex(t => t.mV == max);
                var distance = R[index].mV - Q[index].mV;
                Console.WriteLine("DIST: "+distance);
                for (int i = 0; i < R.Count; ++i)
                {
                    if (R[i].mV-Q[i].mV < 0.20 * distance)
                    {
                        Start.RemoveAt(i);
                        Q.RemoveAt(i);
                        R.RemoveAt(i);
                        S.RemoveAt(i);
                        End.RemoveAt(i);
                        i--;
                    }
                }
            }
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }

        public static Signal operator +(Signal lhs, Signal rhs)
        {
            if (lhs.GetSignal().Count() != rhs.GetSignal().Count())
                throw new Exception("Nie można dodać sygnałów o różnych długościach");

            var sig = new Signal(lhs);
            var i = 0;
            foreach (var item in rhs.GetSignal())
            {
                sig.GetSignal()[i].mV += item.mV;
                ++i;
            }

            return sig;
        }

        public static Signal operator /(Signal lhs, int div)
        {
            var sig = new Signal(lhs);

            for (var i = 0; i < sig.GetSignal().Count(); ++i)
            {
                sig.GetSignal()[i].mV /= div;
            }

            return sig;
        }

        public static Signal operator /(Signal lhs, double div)
        {
            var sig = new Signal(lhs);

            for (var i = 0; i < sig.GetSignal().Count(); ++i)
            {
                sig.GetSignal()[i].mV /= div;
            }

            return sig;
        }

    }

}
