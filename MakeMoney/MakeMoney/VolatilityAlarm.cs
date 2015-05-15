using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakeMoney
{
    /// <summary>
    /// 股市趋势波动告警
    /// 当股市的波动超过了预设的门限，发送告警信息
    /// </summary>
    class VolatilityAlarm
    {
        /// <summary>
        /// 告警门限值
        /// </summary>
        public float Threshold = 0.2F;

        public RealTimeStatistics Statistics { get; set; }

        public IList<StockLiveData> StockLiveDatas = new List<StockLiveData>();

        public void Calculate(float currentPrice)
        {
            if (this.Statistics == null)
            {
                this.Statistics = new RealTimeStatistics();
            }

            var difference = this.Statistics.AvgPrice - currentPrice;
            var upDownDiff = this.Statistics.CalcDifferencMinMax();
            if (upDownDiff > 0)
            {
                var ratio = difference / upDownDiff;
                if (ratio <= Threshold)
                {
                    // 最新价格与平均值的差值小于了最高点最低值差值的20%比例，进行告警

                }
            }

            this.Statistics.Count += 1;
            this.Statistics.Sum += currentPrice;

            if (this.Statistics.Min > currentPrice)
            {
                this.Statistics.Min = currentPrice;
            }

            if (this.Statistics.Max < currentPrice)
            {
                this.Statistics.Max = currentPrice;
            }

            this.Statistics.CalcAvg();
        }
    }
}
