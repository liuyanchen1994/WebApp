using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace WebApp.Services
{
    /// <summary>
    /// 必应自定义搜索
    /// </summary>
    public class BingCustomSearchServices
    {
        private static readonly int VideoConfigId = 67853218;
        private static readonly int TechQuestionConfigId = 110082640;
        private static readonly string ApiEndpoint = "https://api.cognitive.microsoft.com/bingcustomsearch/v7.0/search";

        public CognitiveOptions Options { get; private set; }
        public BingCustomSearchServices(IOptions<CognitiveOptions> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }


        public List<BingSearchWebPage> SearchQuestion(string query, string mkt = "en-US", int offset = 0, int count = 30)
        {
            return Search(query, mkt, TechQuestionConfigId, offset, count);
        }

        public List<BingSearchWebPage> SearchVideo(string query, string mkt = "en-US", int offset = 0, int count = 30)
        {
            return Search(query, mkt, VideoConfigId, offset, count);
        }

        public List<BingSearchWebPage> Search(string query, string mkt, int? configId = 0, int offset = 0, int count = 12)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return default;
            }
            var url = ApiEndpoint +
                "?q=" + HttpUtility.UrlEncode(query) +
                "&mkt=" + mkt +
                "&customconfig=" + configId +
                "&offset=" + offset +
                "&count=" + count +
                "&safeSearch=Off";
            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", Options.BingCustomSearchKey);
                try
                {
                    var httpResponseMessage = client.GetAsync(url).Result;
                    var responseContent = httpResponseMessage.Content.ReadAsStringAsync().Result;
                    BingCustomSearchResponse response = JsonConvert.DeserializeObject<BingCustomSearchResponse>(responseContent);

                    return response.WebPages.Value;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Source + e.Message);
                }
            }
            return default;
        }
    }
    public class BingCustomSearchResponse
    {
        public string _type { get; set; }
        public WebPages WebPages { get; set; }
    }

    public class WebPages
    {
        public string WebSearchUrl { get; set; }
        public int? TotalEstimatedMatches { get; set; }
        public List<BingSearchWebPage> Value { get; set; }
    }

    public class BingSearchWebPage
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public string DisplayUrl { get; set; }
        public string Snippet { get; set; }
        public DateTime DateLastCrawled { get; set; }
        public string CachedPageUrl { get; set; }
        public OpenGraphImage OpenGraphImage { get; set; }
    }
    public class OpenGraphImage
    {
        public string ContentUrl { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }
}
