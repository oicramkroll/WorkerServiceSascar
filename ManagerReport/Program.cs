using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;

namespace ManagerReport
{
    class Program
    {
        private static bool keepRunning = true;
        static void Main(string[] args)
        {
            var serviceCollection = new ServiceCollection();
            ConfigureService(serviceCollection);
            var serviceProvider = serviceCollection.BuildServiceProvider();
            var eventService = serviceProvider.GetService<Services.IReportByDates>();
            Console.WriteLine("Iniciando a aplicação");
            Console.WriteLine("Precione Ctrl+c para sair da aplicação");

            Console.CancelKeyPress += delegate (object sender, ConsoleCancelEventArgs e) {
                e.Cancel = true;
                keepRunning = false;
            };
            
            while (keepRunning)
            {
                eventService.Generate();
            }
            Console.WriteLine("Fechando aplicação...");
            
            
        }
        public static void ConfigureService(IServiceCollection services) {
            services.AddScoped<Services.IReportByDates, Services.ReportByDatesService>();
        }
    }
}
