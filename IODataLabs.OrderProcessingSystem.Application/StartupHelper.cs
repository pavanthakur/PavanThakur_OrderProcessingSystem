using Microsoft.Extensions.DependencyInjection;
using MediatR;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using IODataLabs.OrderProcessingSystem.Infrastructure;
using Microsoft.Extensions.Hosting;

namespace IODataLabs.OrderProcessingSystem.Application
{
    public static class StartupHelper
    {
        //public static void RegisterDbContext(this IServiceCollection serviceCollection, string connectionString)
        //{
        //    serviceCollection.AddDbContext<OrderProcessingSystemDbContext>(db => db.UseSqlServer(connectionString));
        //    //builder.Services.AddDbContext<OrderProcessingDbContext>(db => db.UseSqlServer(builder.Configuration.GetConnectionString("Connection")));
        //}

        public static void InjectApplicationDependencies(this IHostApplicationBuilder builder)
        {

            //builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

            //Finally InjectInfrastructureDependencies
            builder.InjectInfrastructureDependencies();
        }
    }
}
