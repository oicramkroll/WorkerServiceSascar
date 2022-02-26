using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
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
            services.AddScoped<Data.IUnitOfWork, Data.UnitOfWork>();
            services.AddScoped(typeof(Data.IGenericRepository<>), typeof(Data.GenericRepository<>));
            services.AddDbContext<CarDbContext>(options =>
            {
                var dir = AppContext.BaseDirectory;
                JObject config;
                using (StreamReader r = new StreamReader($"{dir}/appsettings.json"))
                {
                    string json = r.ReadToEnd();
                    config = JObject.Parse(json);
                }
                options.UseMySql(
                    config["ConnectionStrings"]["mySql"].ToString(),
                    new MySqlServerVersion(new System.Version(8, 0, 27)));
            });
        }
    }
}
