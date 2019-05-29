using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace LondonSpider
{
    public static class Spider
    {
        [FunctionName(nameof(Spider))]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "{imageSearch:alpha}")] HttpRequest req, string imageSearch,
            ILogger log)
        {
            var scraper = new ImageScraper();
            scraper.ImageSearch = imageSearch;
            scraper.Start();
            return new OkResult();
        }
    }
}
