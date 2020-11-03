using System.Net.Http;

namespace XZMHui.Core.UrlRewriter
{
    public class ProxyHttpClient
    {
        public HttpClient Client { get; private set; }

        public ProxyHttpClient(HttpClient httpClient)
        {
            Client = httpClient;
        }
    }
}