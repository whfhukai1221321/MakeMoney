using System;
using System.IO;
using System.Net;
using System.Text;

namespace MakeMoney
{
    public static class URLHelper
    {
        public static string GetPageContent(string url)
        {
            var httpUrl = new Uri(url);
            var buffer = new char[256];
            var stringBuilder = new StringBuilder();

            var httpRequest = (HttpWebRequest)WebRequest.Create(httpUrl);
            var httpResponse = (HttpWebResponse)httpRequest.GetResponse();

            Stream respStream = httpResponse.GetResponseStream();

            if (respStream != null)
            {
                try
                {
                    var encoding = Encoding.GetEncoding("gb2312");
                    StreamReader respStreamReader = new StreamReader(respStream, encoding);

                    var byteRead = respStreamReader.Read(buffer, 0, 256);

                    while (byteRead != 0)
                    {
                        string strResp = new string(buffer, 0, byteRead);
                        stringBuilder.Append(strResp);
                        byteRead = respStreamReader.Read(buffer, 0, 256);
                    }
                }
                finally
                {
                    respStream.Close();
                }
            }

            return stringBuilder.ToString();
        }
    }
}
