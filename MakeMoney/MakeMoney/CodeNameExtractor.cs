using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace MakeMoney
{
    internal class CodeNameExtractor
    {
        public static IList<StockName> GetStockNames()
        {
            var codeContent = URLHelper.GetPageContent("http://quote.eastmoney.com/stocklist.html");

            const string regexPattern = "(?:<li><a\\s+target=\"_blank\"\\s+href=\"http://quote.eastmoney.com/[^.]*\\.html\">([^<]+)\\((.+)\\)</a></li>)";

            var suffix = "ss";  // 上海 sh，深圳 sz hahah
            var match = Regex.Match(codeContent, regexPattern);
            var stockNameList = new List<StockName>();
            var ssStart = codeContent.IndexOf("<div class=\"sltit\"><a name=\"sh\"/>上海股票</div>", StringComparison.Ordinal);
            var szStart = codeContent.IndexOf("<div class=\"sltit\"><a name=\"sz\"/>深圳股票</div>", StringComparison.Ordinal);
            while (match.Success)
            {
                if (match.Index > ssStart && match.Index < szStart)
                {
                    suffix = "ss";
                }
                else
                {
                    suffix = "sz";
                }

                var newStockName = new StockName();
                newStockName.Name = match.Groups[1].Value;
                newStockName.Code = match.Groups[2].Value;
                newStockName.Suffix = suffix;
                stockNameList.Add(newStockName);
                match = match.NextMatch();
            }

            return stockNameList;
        }
    }

    public class StockName
    {
        public string Name { get; set; }

        public string Code { get; set; }

        public string Suffix { get; set; }
    }
}
