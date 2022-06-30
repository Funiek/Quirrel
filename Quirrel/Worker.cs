namespace Quirrel
{
    public class Worker : BackgroundService
    {
        private readonly IConfiguration configuration;
        private readonly ILogger<Worker> logger;
        public Worker(IConfiguration configuration, ILogger<Worker> logger)
        {
            this.configuration = configuration;
            this.logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            logger.LogInformation("Worker started...");

            bool flag = false;
            while (!stoppingToken.IsCancellationRequested)
            {
                Console.WriteLine(configuration.GetSection("UserConfig")["Test"]);
                if (!flag)
                {
                    //googleApiHandler.ListGoogleDrive(configuration);
                    flag = true;
                }

                await Task.Delay(2000, stoppingToken);

            }
        }
    }
}