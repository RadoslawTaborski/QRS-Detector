using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRS_Detector
{
    public class DataPoint: ICloneable
    {
        public double Time { get; set; }
        public double mV { get; set; }

        public DataPoint(double x, double y)
        {
            Time = x;
            mV = y;
        }

        public override string ToString()
        {
            return Time + " " + mV;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
