using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace MakeMoney
{
    /// <summary>
    /// 从网络抽取股票数据
    /// </summary>
    internal class DataExtractor
    {
        private const string UrlBase = "http://table.finance.yahoo.com/table.csv?s={0}.{1}";

        public static List<StockDatas> Fetch(IList<StockName> quotes)
        {
            var stockDataList = new List<StockDatas>();

            var path = "d:\\stockData.dat";

            if (!File.Exists(path))
            {
                //Parallel.ForEach(quotes, name =>
                foreach(var name in quotes)
                {

                    try
                    {
                        var stockDatas = new StockDatas(name);
                        FetchStockData(stockDatas);

                        lock (stockDataList)
                        {
                            stockDataList.Add(stockDatas);
                        }

                        Console.WriteLine("获取 " + name.Name + "[" + name.Code + "] 数据成功... index = " + stockDataList.Count);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("获取 " + name.Name + "[" + name.Code + "] 数据失败 : " + ex.Message);
                    }
                }
                //);

                using (var fileStream = new FileStream(path, FileMode.CreateNew))
                {
                    var writer = new BinaryWriter(fileStream);
                    writer.Write(stockDataList.Count);
                    foreach (var stock in stockDataList)
                    {
                        var nameBytes = Encoding.UTF8.GetBytes(stock.StockName.Name.ToCharArray());
                        var codeBytes = Encoding.UTF8.GetBytes(stock.StockName.Code.ToCharArray());
                        var suffixBytes = Encoding.UTF8.GetBytes(stock.StockName.Suffix.ToCharArray());

                        writer.Write(nameBytes.Length);
                        writer.Write(nameBytes);
                        writer.Write(codeBytes.Length);
                        writer.Write(codeBytes);
                        writer.Write(suffixBytes.Length);
                        writer.Write(suffixBytes);

                        writer.Write(stock.Stocks.Count);

                        foreach (var stockItem in stock.Stocks)
                        {
                            writer.Write(stockItem.Open);
                            writer.Write(stockItem.Close);
                            writer.Write(stockItem.High);
                            writer.Write(stockItem.Low);
                            writer.Write(stockItem.AdjClose);
                            writer.Write(stockItem.Volume);
                            writer.Write(stockItem.Date.Ticks);
                        }
                    }
                }
            }
            else
            {
                using (var fileStream = new FileStream(path, FileMode.Open))
                {
                    var reader = new BinaryReader(fileStream);
                    var count = reader.ReadInt32();
                    
                    for (int i = 0; i < count; i++)
                    {
                        var stockName = new StockName();

                        var nameSize = reader.ReadInt32();
                        var nameBytes = reader.ReadBytes(nameSize);
                        stockName.Name = Encoding.UTF8.GetString(nameBytes);

                        var codeSize = reader.ReadInt32();
                        var codeBytes = reader.ReadBytes(codeSize);
                        stockName.Code = Encoding.UTF8.GetString(codeBytes);

                        var suffixSize = reader.ReadInt32();
                        var suffixBytes = reader.ReadBytes(suffixSize);
                        stockName.Suffix = Encoding.UTF8.GetString(suffixBytes);

                        var stockDatas = new StockDatas(stockName);

                        var stockItemCount = reader.ReadInt32();
                        for (int j = 0; j < stockItemCount; j++)
                        {
                            var stock = new Stock();
                            stock.Open = reader.ReadSingle();
                            stock.Close = reader.ReadSingle();
                            stock.High = reader.ReadSingle();
                            stock.Low = reader.ReadSingle();
                            stock.AdjClose = reader.ReadSingle();
                            stock.Volume = reader.ReadInt64();
                            stock.Date = DateTime.FromBinary(reader.ReadInt64());
                            stockDatas.Stocks.Add(stock);
                        }

                        Console.WriteLine("获取 " + stockName.Name + "[" + stockName.Code + "] 数据成功...");
                        stockDataList.Add(stockDatas);
                    }
                }
            }

            return stockDataList;
        }

        private static void FetchStockData(StockDatas stockDatas)
        {
            var url = String.Format(UrlBase, stockDatas.StockName.Code, stockDatas.StockName.Suffix);
            var stockDataContent = URLHelper.GetPageContent(url);

            if (string.IsNullOrEmpty(stockDataContent))
            {
                return;
            }

            var rows = stockDataContent.Split('\n');

            if (rows.Length > 1)
            {
                //
                // 忽略第一行列名
                //

                for (int i = 1; i < rows.Length; i++)
                {
                    var row = rows[i];

                    if (!string.IsNullOrEmpty(row))
                    {
                        var cols = row.Split(',');

                        var stock = new Stock();
                        stock.Date = DateTime.Parse(cols[0]); // 日期
                        stock.Open = float.Parse(cols[1]); // 开盘价格
                        stock.High = float.Parse(cols[2]); // 最高价格
                        stock.Low = float.Parse(cols[3]); // 最低价格
                        stock.Close = float.Parse(cols[4]); // 收盘价格
                        stock.Volume = long.Parse(cols[5]); // 成交量
                        stock.AdjClose = float.Parse(cols[6]); // ???
                        stockDatas.Stocks.Add(stock);
                    }
                }
            }
        }
    }

    public class StockDatas
    {
        public StockDatas(StockName stockName)
        {
            this.StockName = stockName;
            this.Stocks = new List<Stock>();
        }

        public StockName StockName { get; set; }

        public IList<Stock> Stocks { get; private set; }

        public double PosibleFactor { get; private set; }

        public void AnalyzeFactor()
        {
            if (this.Stocks.Count == 0)
            {
                this.PosibleFactor = -1;
                return ;
            }

            var curDate = DateTime.Now;
            var startDate = curDate.AddDays(-20);

            var index = 0;
            var baseFactor = (this.Stocks[index].High + this.Stocks[index].Low)/2;
            var facDelta = 0f;
            var sum = 0;
            for (int j = 1; j < this.Stocks.Count && this.Stocks[j].Date >= startDate; j++)
            {
                var curFactor = (this.Stocks[j].High + this.Stocks[j].Low) / 2;
                facDelta += (baseFactor - curFactor);
                sum++;
            }

            // 得出平均差价趋势总累计
            if (sum > 0)
            {
                facDelta /= sum;

                this.PosibleFactor = Math.Pow(facDelta, 2);
            }
            else
            {
                this.PosibleFactor = -1;
            }

        }
    }
}
