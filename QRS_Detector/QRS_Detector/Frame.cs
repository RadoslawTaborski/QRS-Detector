using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRS_Detector
{
    public class Frame
    {
        public int left { get; set; }
        public int right { get; set; }

        public override string ToString()
        {
            return left + " " + right;
        }

    }
}
