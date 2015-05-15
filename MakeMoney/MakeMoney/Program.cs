using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;

namespace MakeMoney
{
    class Program
    {
        static void Main(string[] args)
        {
            string stockUrl = "http://www.webxml.com.cn/WebServices/ChinaStockWebService.asmx";
            var result = WebServiceHelper.InvokeWebService(stockUrl, "getStockImageByteByCode", new object[] {"sz000988"});
            if (result != null)
            {
                var resultBytes = result as byte[];
                if (resultBytes == null)
                {
                    Debug.Assert(false, "resultBytes cannot be null");
                }

                using (MemoryStream ms = new MemoryStream(resultBytes))
                {
                    using (Image img = Image.FromStream(ms))
                    {
                        img.Save("C:\\stock_trend.bmp");
                    }
                }
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
