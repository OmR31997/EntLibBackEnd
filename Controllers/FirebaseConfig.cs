using Microsoft.AspNetCore.Mvc;

namespace EntLibBackendAPI.Controllers
{
    public class FirebaseConfig : Controller
    {
        [HttpGet("firebase-config")]
        public IActionResult FireBaseConfig()
        {
            var config = new
            {
                ApiKey = Environment.GetEnvironmentVariable("FIREBASE_API_KEY") ?? "Not Configured",
                AuthDomain = Environment.GetEnvironmentVariable("FIREBASE_AUTH_DOMAIN") ?? "Not Configured",
                AppId = Environment.GetEnvironmentVariable("FIREBASE_APP_ID") ?? "Not Configured",
                DatabaseUrl = Environment.GetEnvironmentVariable("FIREBASE_DATABASE_URL") ?? "Not Configured",
                MeasurementId = Environment.GetEnvironmentVariable("FIREBASE_MEASUREMENTID") ?? "Not Configured",
                MessagingSenderId = Environment.GetEnvironmentVariable("FIREBASE_MESSAGING_SENDER_ID") ?? "Not Configured",
                StorageBucket = Environment.GetEnvironmentVariable("FIREBASE_STORAGE_BUCKET") ?? "Not Configured",
                ProjectId = Environment.GetEnvironmentVariable("FIREBASE_PROJECT_ID") ?? "Not Configured",

            };
            return Ok(config);
        }
    }
}
