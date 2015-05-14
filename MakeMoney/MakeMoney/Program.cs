using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MakeMoney
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("正在获取股票种类数据...");
            var stockNameList = CodeNameExtractor.GetStockNames();

            Console.WriteLine("正在获取股票日线数据...");
            var stocks = DataExtractor.Fetch(stockNameList);

            Console.WriteLine("获取成功，正在分析排名");
            var sortedStocks = StockAnalyzer.AnalysisAndSort(stocks, 10);

            Console.WriteLine("分析结果如下，(候选前10名排名):");

            foreach (var sortedStock in sortedStocks)
            {
                Console.WriteLine("\t" + sortedStock.StockName.Name + "[" + sortedStock.StockName.Code + "], Factor = " + sortedStock.PosibleFactor);
            }

            Console.Read();
        }
    }
}
