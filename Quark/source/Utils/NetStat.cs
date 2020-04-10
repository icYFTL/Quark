using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Quark.source.Utils
{
    public static class NetStat
    {
        private static HttpWebRequest webRequest;

        private static void StartWebRequest(string url)
        {
            webRequest.BeginGetResponse(new AsyncCallback(FinishWebRequest), url);
        }

        private static void FinishWebRequest(IAsyncResult result)
        {
            webRequest.EndGetResponse(result);
        }
        public async static Task<bool> is_connected()
        {
            try
            {
                var request = WebRequest.Create("http://google.com");
                request.Timeout = 3000;
                var response = (HttpWebResponse)await Task.Factory
                    .FromAsync<WebResponse>(request.BeginGetResponse,
                                            request.EndGetResponse,
                                            null);
                return response.StatusCode == HttpStatusCode.OK;
            }
            catch
            {
                return false;
            }
        }
    }
}
