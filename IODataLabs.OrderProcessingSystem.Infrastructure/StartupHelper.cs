﻿using IODataLabs.OrderProcessingSystem.Infrastructure.DataContext;
using IODataLabs.OrderProcessingSystem.Infrastructure.SeedData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IODataLabs.OrderProcessingSystem.Infrastructure
{
    public static class StartupHelper
    {
        public static void InjectInfrastructureDependencies(this IHostApplicationBuilder builder)
        {
            builder.Services.AddDbContext<OrderProcessingSystemDbContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("OrderProcessingSystemDbConnection"));
            });

            //Note : Comment this when running Add-Migration Command from Package Manager Console. Then uncomment after migration file is generated
            // Auto migration setup
            using (var serviceProvider = builder.Services.BuildServiceProvider())
            {
                var dbContext = serviceProvider.GetRequiredService<OrderProcessingSystemDbContext>();
                dbContext.Database.Migrate();
                DbInitializer.Initialize(dbContext);
            }
        }
    }
}
