using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Net.Http;
using SimpleRecommendations.Models;
using Newtonsoft.Json;

namespace SimpleRecommendations.Controllers
{
    public class HomeController : Controller
    {
        public async Task <IActionResult> Index(string SeedItemId)
        {
            if (string.IsNullOrEmpty(SeedItemId)) return View(new List<RecommendedItem>());

            //api details
            var defaultModelUri = "https://sportshopxmlgqkb7ew5gyws.azurewebsites.net/api/models/default/recommend";
            var recommenderKey = "M3d4d2k3amhzN21seQ==";

            //construct API url
            var parameters = new Dictionary<string, string> {
                { "recommendationCount", "10" },
                { "itemId", SeedItemId }
            };
            var apiUri = QueryHelpers.AddQueryString(defaultModelUri, parameters);

            //setup HttpClient
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(defaultModelUri);
            httpClient.DefaultRequestHeaders.Add("x-api-key", recommenderKey);

            //make request
            var response = await httpClient.GetAsync(apiUri);
            var responseContent = await response.Content.ReadAsStringAsync();
            var recommendedItems = JsonConvert.DeserializeObject<List<RecommendedItem>>(responseContent);

            return View(recommendedItems);
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
