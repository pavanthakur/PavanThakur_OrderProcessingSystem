using Microsoft.Extensions.DependencyInjection;
using MediatR;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using IODataLabs.OrderProcessingSystem.Infrastructure;
using Microsoft.Extensions.Hosting;
using FluentValidation;

namespace IODataLabs.OrderProcessingSystem.Application
{
    public static class StartupHelper
    {
        public static void InjectApplicationDependencies(this IHostApplicationBuilder builder)
        {
            // Use the correct extension method for adding validators
            builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            builder.Services.AddMediatR(Assembly.GetExecutingAssembly());

            //Finally InjectInfrastructureDependencies
            builder.InjectInfrastructureDependencies();
        }
    }
}
