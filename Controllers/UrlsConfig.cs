using Microsoft.AspNetCore.Mvc;

namespace EntLibBackendAPI.Controllers
{
    public class UrlsConfig : Controller
    {
        [HttpGet("urls-config")]
        public IActionResult UrlConfig()
        {
            var urls = new
            {
                MovieUrl = Environment.GetEnvironmentVariable("MOVIE_API") ?? "Not Configured",
                ShowUrl = Environment.GetEnvironmentVariable("SHOW_API") ?? "Not Configured",
                FeedUrl = Environment.GetEnvironmentVariable("FEED_API") ?? "Not Configured",
                UserUrl = Environment.GetEnvironmentVariable("USER_API") ?? "Not Configured",
            };
            return Ok(urls);
        }
    }
}
