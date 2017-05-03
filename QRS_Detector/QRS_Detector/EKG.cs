using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QRS_Detector
{
    public class EKG
    {
        public Signal[] Signals {get; set; }

        public EKG()
        {
            Signals = new Signal[15];
            for(int i=0; i<15; ++i)
            {
                Signals[i] = new Signal();
            }
        }

        public void readSignalsFromText(string data)
        {
            string[] separators = { " ", "\r", "\t", "\n" };
            string[] words = data.Split(separators, StringSplitOptions.RemoveEmptyEntries);
            /*foreach (var word in words)
                Console.Write(word+ " ");*/
            for (int j = 0; j < words.Length/16; ++j)
            {
                //Console.WriteLine(words[16 * j]);
                double x = Convert.ToDouble(words[16*j], CultureInfo.GetCultureInfo("en-us"));
                //Console.WriteLine(x);
                for (int i = 1; i < 16 ; i = ++i )
                {                
                    double y = Convert.ToDouble(words[16*j + i], CultureInfo.GetCultureInfo("en-us"));
                    //Console.WriteLine(y);
                    Signals[i-1].AddDataPoint(new DataPoint(x, y));
                }
            }
        }

        public void display()
        {
            foreach (var data in Signals)
            {
                foreach(var dat in data.GetSignal())
                    Console.WriteLine(dat);
                Console.WriteLine(" ");
            }
        }

        public Signal averaging()
        {
            var sig = new Signal(Signals[0]);

            foreach(var signal in Signals.Skip(1))
            {
                sig = sig + signal;                
            }
            sig = sig / Signals.Count();
            
            return sig;
        }


    }
}
