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
            var eventService = serviceProvider.GetService<Services.IReportSVC>();
            Console.WriteLine("Aplicação Iniciada");

            Console.CancelKeyPress += delegate (object sender, ConsoleCancelEventArgs e) {
                e.Cancel = true;
                keepRunning = false;
            };
            
            while (keepRunning)
            {
                eventService.GenerateByDateInterval();
            }
            Console.WriteLine("Fechando aplicação...");
            
            
        }
        public static void ConfigureService(IServiceCollection services) {
            services.AddScoped<Services.IReportSVC, Services.ReportSVCService>();
            //TODO:Injeção de dependencia para o banco de dados
        }
    }
}
