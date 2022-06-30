using Quirrel.Utils;

namespace Quirrel
{
    public class Worker : BackgroundService
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<Worker> logger;
        private readonly IServiceProvider serviceProvider;
        public Worker(IConfiguration configuration, ILogger<Worker> logger, IServiceProvider serviceProvider)
        {
            this.configuration = configuration;
            this.logger = logger;
            this.serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var GoogleApiHandler = ActivatorUtilities.CreateInstance<GoogleApiHandler>(serviceProvider);
            logger.LogInformation($"Worker started... {DateTime.Now}");
            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    GoogleApiHandler.ListGoogleDrive();
                    await Task.Delay(2000, stoppingToken);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


        }
    }
}
