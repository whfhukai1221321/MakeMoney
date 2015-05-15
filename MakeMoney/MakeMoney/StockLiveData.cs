using System;

namespace MakeMoney
{
    class StockLiveData
    {
        public StockLiveData()
        {
            this.BuyPrices = new float[5];
            this.SellPrices = new float[5];
        }

        /// <summary>
        /// 股票代码
        /// </summary>
        public string Code { get; set; }
        
        /// <summary>
        /// 股票名称
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// 行情时间
        /// </summary>
        public DateTime QuotationTime { get; set; }
        
        /// <summary>
        /// 最新价
        /// </summary>
        public float Price { get; set; }
        
        /// <summary>
        /// 昨日收盘价
        /// </summary>
        public float PreClosePrice { get; set; }

        /// <summary>
        /// 今开盘价
        /// </summary>
        public float OpenPrice { get; set; }

        /// <summary>
        /// 涨跌额
        /// </summary>
        public float UpDownPrice { get; set; }

        /// <summary>
        /// 最低价
        /// </summary>
        public float MinPrice { get; set; }

        /// <summary>
        /// 最高价
        /// </summary>
        public float MaxPrice { get; set; }

        /// <summary>
        /// 涨跌幅(%)
        /// </summary>
        public float PriceLimit { get; set; }

        /// <summary>
        /// 成交量(手)
        /// </summary>
        public int Volume { get; set; }

        /// <summary>
        /// 成交额(万元)
        /// </summary>
        public float TurnOver { get; set; }

        /// <summary>
        /// 竞买价
        /// </summary>
        public float BidPrice { get; set; }

        /// <summary>
        /// 竞卖价
        /// </summary>
        public float RedemptionPrice { get; set; }

        /// <summary>
        /// 委比(%)
        /// </summary>
        public float CommissionRatio { get; set; }

        /// <summary>
        /// 买一 - 买五(元/手)
        /// </summary>
        public float[] BuyPrices { get; set; }

        /// <summary>
        /// 卖一 - 卖五(元/手)
        /// </summary>
        public float[] SellPrices { get; set; }
    }
}
