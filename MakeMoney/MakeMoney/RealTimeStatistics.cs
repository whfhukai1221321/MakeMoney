using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakeMoney
{
    class RealTimeStatistics
    {
        public double AvgPrice { get; set; }

        public int Count { get; set; }

        public double Sum { get; set; }

        public float Max { get; set; }

        public float Min { get; set; }

        public double CalcAvg()
        {
            if (this.Count > 0)
            {
                this.AvgPrice = this.Sum / this.Count;
            }

            return this.AvgPrice;
        }

        public double CalcDifferencMinMax()
        {
            return this.Max - this.Min;
        }
    }
}
