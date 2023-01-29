using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace RestAPI_Library
{
    public static class APIHelper
    {
        public static HttpClient ApiClient { get; set; }

        public static void Init()
        {
            ApiClient = new HttpClient();

            ApiClient.DefaultRequestHeaders.Accept.Clear();
            ApiClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            
            ApiClient.BaseAddress = new Uri("https://xkcd.com/");
        }
    }
}
