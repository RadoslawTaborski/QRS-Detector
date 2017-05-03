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
        private List<DataPoint> copy;
        private Complex[] fft;
        private List<Frame> frames;
        public List<DataPoint> Q;
        public List<DataPoint> R;
        public List<DataPoint> S;

        public Signal()
        {
            signal = new List<DataPoint>();
        }

        public Signal(Signal sig)
        {
            signal = new List<DataPoint>(sig.GetSignal());
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

        public void aboveThreshold(double thresh)
        {
            var result = 0d;
            for (var i = 0; i < signal.Count(); ++i)
            {
                if(signal[i].mV>thresh)
                    signal[i].mV=1;
                else
                    signal[i].mV = 0;
            }
            result /= signal.Count();
        }

        public void findFrames(int delay)
        {
            frames = new List<Frame>();
            var left = new List<int>();
            var right = new List<int>();
            for (var i = 1; i < signal.Count()-1; ++i)
            {
                if (signal[i].mV - signal[i - 1].mV == 1)
                    left.Add(i);
                if (signal[i+1].mV - signal[i].mV == -1)
                    right.Add(i);
            }
            if (right.Count() != 0 && left.Count() != 0)
            {
                if (right[1] < left[1])
                    right.Remove(1);
                if (left.Last() > right.Last())
                    left.Remove(left.Count() - 1);
            }
            for (var i=0; i<left.Count(); ++i)
            {
                frames.Add(new Frame() { left=left[i]-delay, right=right[i]-delay });
            }
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

            for (int i = 0; i < signal.Count(); i++) //TODO: signal.Count
            {
                fft[i] = new Complex(signal[i].mV, 0);
            }

            FourierTransform.FFT(fft, FourierTransform.Direction.Forward);
        }

        public void BandPassFilter(double Fs, double a, double b)
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

            for (var i = 0; i < signal.Count(); ++i) //TODO: signal.Count
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

        public void Frame(double Fs)
        {
            if (copy == null)
                copy = (List<DataPoint>)Extensions.Clone(signal);
            var tmp = Extensions.Clone(signal);
            var delay = Math.Round(0.15 * Fs);
            for (var k=0; k<tmp.Count;++k)
            {
                var sum = 0d;
                for (var i = 0; i <= delay; ++i)
                {
                    if (k - i >= 0)
                        sum += tmp[k - i].mV;
                }
                signal[k].mV = 1/delay*sum;
            }

        }

        public void findQRS()
        {
            Q = new List<DataPoint>();
            R = new List<DataPoint>();
            S = new List<DataPoint>();

            foreach (var frame in frames)
            {
                var sublist = copy.GetRange(frame.left, frame.right-frame.left);
                var item = sublist.MaxBy(x => x.mV);
                R.Add(item);
                var index=sublist.IndexOf(item);
                var subsublist = sublist.GetRange(0, index);
                item = subsublist.MinBy(x => x.mV);
                Q.Add(item);
                subsublist = sublist.GetRange(index, sublist.Count() - index);
                item = subsublist.MinBy(x => x.mV);
                S.Add(item);
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

    }
}
