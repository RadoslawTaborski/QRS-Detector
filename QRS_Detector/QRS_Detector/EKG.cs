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
        public List<DataPoint>[] Signals {get; set; }

        public EKG()
        {
            Signals = new List<DataPoint>[15];
            for(int i=0; i<15; ++i)
            {
                Signals[i] = new List<DataPoint>();
            }
        }

        public void readSignalFromText(string data)
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
                    Signals[i-1].Add(new DataPoint(x, y));
                }
            }
        }

        public void display()
        {
            foreach (var data in Signals)
            {
                foreach(var dat in data)
                    Console.WriteLine(dat);
                Console.WriteLine(" ");
            }
        }
    }
}
