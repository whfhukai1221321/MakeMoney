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
            // getStockImageByCode GET 股票GIF分时走势图
            // getStockImageByteByCode 获得中国股票GIF分时走势图字节数组
            // getStockImage_kByCode 直接获得中国股票GIF日/周/月 K 线图（545*300pixel/72dpi）
            // getStockImage_kByteByCode 获得中国股票GIF日/周/月 K 线图字节数组
            // getStockInfoByCode 获得中国股票及时行情
            var stockUrl = "http://www.webxml.com.cn/WebServices/ChinaStockWebService.asmx";
            var result = WebServiceHelper.InvokeWebService(stockUrl, "getStockImageByteByCode", new object[] { "theStockCode=sz000988" });
            if (result != null)
            {
                var resultBytes = result as byte[];
                if (resultBytes == null)
                {
                    Debug.Assert(false, "resultBytes cannot be null");
                }

                using (var ms = new MemoryStream(resultBytes))
                {
                    using (var img = Image.FromStream(ms))
                    {
                        img.Save("C:\\stock_trend.bmp");
                    }
                }
            }

            var stockInfo = WebServiceHelper.InvokeWebService(stockUrl, "getStockInfoByCode", new object[] { "sz000988" });
            var stockInfoArray = stockInfo as string[];
            if (stockInfoArray != null)
            {
                foreach (var s in stockInfoArray)
                {
                    Debug.WriteLine(s);
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
