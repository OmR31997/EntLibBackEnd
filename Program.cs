using DotNetEnv;
using Newtonsoft.Json;

namespace EntLibBackendAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Load .env file
            if (builder.Environment.IsDevelopment())
            {
                try
                {
                    Env.Load();
                    Console.WriteLine("Loaded .env file successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading .env file: {ex.Message}");
                }
            }

            // Firebase credential initialization
            string? firebaseKeyPath = Environment.GetEnvironmentVariable("FireBasePath") ?? "/etc/secrets/firebase-config.json";

            // API Urls Initialization
            string? urlApiPath = Environment.GetEnvironmentVariable("ApiUrlPath") ?? "/etc/secrets/urls-path.json";

            if (string.IsNullOrEmpty(firebaseKeyPath) ||  string.IsNullOrEmpty(urlApiPath))
            {
                Console.WriteLine("Environment variable 'FireBasePath' or 'FireBasePath' is not set or is empty.");
            }
            else
            {
                try
                {
                    string json_firebase = File.ReadAllText(firebaseKeyPath);
                    string json_api = File.ReadAllText(urlApiPath);

                    var firebaseConfig = JsonConvert.DeserializeObject<Dictionary<string, string>>(json_firebase);
                    var urlConfig = JsonConvert.DeserializeObject<Dictionary<string, string>>(json_api);

                    if (firebaseConfig != null)
                    {
                        foreach (var key in firebaseConfig)
                        {
                            Environment.SetEnvironmentVariable(key.Key, key.Value, EnvironmentVariableTarget.Process);
                        }

                        Console.WriteLine("Firebase credentials loaded and environment variables set successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Firebase configuration JSON is null or invalid.");
                    }

                    if(urlConfig != null)
                    {
                        foreach (var key in urlConfig)
                        {
                            Environment.SetEnvironmentVariable(key.Key, key.Value, EnvironmentVariableTarget.Process);
                        }
                    }
                }
                catch (FileNotFoundException ex)
                {
                    Console.WriteLine($"File not found: {firebaseKeyPath} - {ex.Message}");
                }
                catch (JsonReaderException ex)
                {
                    Console.WriteLine($"JSON Parsing Error: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unexpected error: {ex.Message}");
                }
            }

            // Add services to the container
            builder.Services.AddControllers();

            // Configure CORS Policy
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAngularApp", policy =>
                    policy.WithOrigins("http://localhost:4200") // Angular app URL
                          .AllowAnyHeader()
                          .AllowAnyMethod());
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();


            app.UseSwagger();
            app.UseSwaggerUI();
            
            // Disable HTTPS redirection in development
            if (!app.Environment.IsDevelopment())
            {
                // Get the port from the Render environment
                var httpsPort = Environment.GetEnvironmentVariable("PORT");
                if (!string.IsNullOrEmpty(httpsPort))
                {
                    // Set ASPNETCORE_URLS environment variable to include HTTP and HTTPS ports
                    var urls = $"https://0.0.0.0:{httpsPort};http://0.0.0.0:{httpsPort}";
                    Environment.SetEnvironmentVariable("ASPNETCORE_URLS", urls);
                    app.UseHttpsRedirection();
                }
            }


            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors("AllowAngularApp");
            app.UseAuthorization();
            app.MapGet("/", () => Results.Redirect("/swagger"));
            app.MapControllers();
            app.Run();
        }
    }
}
