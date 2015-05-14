using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakeMoney
{
    public class StockAnalyzer
    {
        public static IList<StockDatas> AnalysisAndSort(List<StockDatas> allStocks, int num)
        {
            //
            // 对所有股票按照趋势进行排名
            //
            var listWithout30 = allStocks.Where(stock => !stock.StockName.Code.StartsWith("30") && stock.Stocks.Count > 0 && stock.Stocks[0].High < 15).ToList();

            //Parallel.ForEach(allStocks, datas =>
            foreach (var datas in listWithout30)
            {
                datas.AnalyzeFactor();
            }//);

            listWithout30.Sort((stock1, stock2) =>
            {
                if (stock1.PosibleFactor > stock2.PosibleFactor)
                {
                    return -1;
                }
                else if (stock1.PosibleFactor == stock2.PosibleFactor)
                {
                    return 0;
                }
                else
                {
                    return 1;
                }
            });

            if (listWithout30.Count <= num)
            {
                return listWithout30;
            }
            else
            {
                var stocks = new List<StockDatas>();
                for (int i = 0; i < num; i++)
                {
                    stocks.Add(listWithout30[i]);
                }
                return stocks;
            }
        }

        public class StockFactorComparer : IComparer
        {
            public int Compare(object x, object y)
            {
                var stock1 = (StockDatas) x;
                var stock2 = (StockDatas) y;

                if (stock1.PosibleFactor > stock2.PosibleFactor)
                {
                    return 1;
                }
                else if (stock1.PosibleFactor == stock2.PosibleFactor)
                {
                    return 0;
                }
                else
                {
                    return -1;
                }
            }
        }
    }
}
