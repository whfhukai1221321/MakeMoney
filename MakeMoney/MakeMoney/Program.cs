using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Text;

namespace MakeMoney
{
    class Program
    {
        static void Main(string[] args)
        {
            var url = "http://hq.sinajs.cn/list=sz000988,sh601028,sh000988";
            var content = URLHelper.GetPageContent(url);
            if (!string.IsNullOrEmpty(content))
            {

            }

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
