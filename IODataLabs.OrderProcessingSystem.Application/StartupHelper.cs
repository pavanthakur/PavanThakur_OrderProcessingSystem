using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using IODataLabs.OrderProcessingSystem.Infrastructure;
using Microsoft.Extensions.Hosting;
using FluentValidation;
using Microsoft.AspNetCore.DataProtection.Repositories;
using IODataLabs.OrderProcessingSystem.Application.Interfaces;
using IODataLabs.OrderProcessingSystem.Application.Services;

namespace IODataLabs.OrderProcessingSystem.Application
{
    public static class StartupHelper
    {
        public static void InjectApplicationDependencies(this IHostApplicationBuilder builder)
        {
            // Use the correct extension method for adding validators
            builder.Services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            // Register Application Services 
            builder.Services.AddScoped<IOrderService, OrderService>(); // Scoped to ensure a new instance per request
            builder.Services.AddScoped<ICustomerService, CustomerService>();

            //Finally InjectInfrastructureDependencies
            builder.InjectInfrastructureDependencies();
        }
    }
}
