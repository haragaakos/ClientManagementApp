namespace ClientManagementApp.Services
{
    public class JsonGenerator : IHostedService, IDisposable
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;
        private Timer _timer;

        public JsonGenerator(IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            _configuration = configuration;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            int interval = _configuration.GetValue<int>("JsonGenerationInterval", 60000); //1 perc alapértelmezésként
            _timer = new Timer(GenerateJson, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(interval));
            return Task.CompletedTask;
        }
        private async void GenerateJson(object state)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var clients = await context.Clients.Include(c => c.Addresses).ToListAsync();

                foreach (var client in clients)
                {
                    var jsonData = JsonConvert.SerializeObject(client, new JsonSerializerSettings
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                    });
                    var logEntry = new ClientJsonLog
                    {
                        ClientId = client.Id,
                        JsonData = jsonData,
                        Timestamp = DateTime.Now
                    };
                    context.ClientJsonLogs.Add(logEntry);
                    client.IsJsonGenerated = true;
                }

                await context.SaveChangesAsync();
            }
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }
        public void Dispose() => _timer?.Dispose();
    }
}
