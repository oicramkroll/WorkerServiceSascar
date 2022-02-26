using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace WorkerServiceSascar
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseWindowsService()
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddDbContext<CarDbContext>(options =>
                        {
                            options.UseMySql(
                                hostContext.Configuration.GetSection("ConnectionStrings:mySql").Value,
                                new MySqlServerVersion(new System.Version(8, 0, 27)));
                        }
                    );

                    services.AddHostedService<Worker>();
                    services.AddScoped<Services.IReportSVC, Services.ReportSVCService>();
                    services.AddScoped<Data.IUnitOfWork, Data.UnitOfWork>();
                    services.AddScoped(typeof(Data.IGenericRepository<>),typeof(Data.GenericRepository<>));

                });
    }
}
