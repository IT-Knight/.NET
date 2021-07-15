using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace TGBOT
{
    public static class Google
    {
        private static HttpClient client = new HttpClient();

        public static async Task<List<string>> Search(string search_query, string language = "ru")
        {
            string url = "https://www.googleapis.com/customsearch/v1?key=AIzaSyCRBYtLfCffnY0MGAxx-KP-P2fZuIM6UYc&"
                          + $"cx=7688bbfdd48437471&q={search_query}&hl={language}&num=10";

            string response = await client.GetStringAsync(url);

            dynamic json_mess = JsonConvert.DeserializeObject(response);
            var json = new List<Result>();

            foreach (var item in json_mess.items) 
                {  
                json.Add(new Result { title = item.title, link = item.link, snippet = item.snippet });
                }

            var results = new List<string>();

            foreach (var obj in json.Select((x, i) => new { item = x, Index = i })) 
                {
                string title = obj.item.title, link = obj.item.link, snippet = obj.item.snippet ?? "";

                    string res = String.Format("{0}. {1} \n<a href=\"{2}\">{3}</a>\n {4}",
                                                obj.Index + 1,
                                                WebUtility.HtmlEncode(title.Substring(0, Math.Min(title.Length, 58))),
                                                link,
                                                link.Substring(0, Math.Min(link.Length, 58)).Replace(" › ", " &gt ").Replace("...", ""),
                                                WebUtility.HtmlEncode(snippet.Substring(0, Math.Min(snippet.Length, 170)))
                                                );
                    results.Add(res);
                }

            return results;
        }
    }
    public class Result
    {
        public string title;
        public string link;
        public string snippet;
        //public string source;
        //public string query;
        //public int? index;
    }
}
