using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace WorkerServiceSascar
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceProvider _serviceProvider;
        public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            
            while (!stoppingToken.IsCancellationRequested)
            {

                using (IServiceScope scope = _serviceProvider.CreateScope())
                {
                    Services.IReportSVC _reportSVC =
                        scope.ServiceProvider.GetRequiredService<Services.IReportSVC>();

                    var config = _reportSVC.GetConfigApp();
                    //if (DateTime.Now.ToString("HH:mm") == config["horarioRotina"].ToString())
                    //{
                        _reportSVC.Generate(DateTime.Now.AddDays(-1), DateTime.Now);
                    //}
                }


                await Task.Delay((1000*60), stoppingToken);
            }
        }
    }
}
