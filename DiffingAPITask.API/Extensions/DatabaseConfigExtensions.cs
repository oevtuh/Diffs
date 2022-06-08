using System;
using DiffingAPITask.Data.EF;
using DiffingAPITask.Data.Repositories;
using DiffingAPITask.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DiffingAPITask.API.Extensions
{
    public static class DatabaseConfigExtensions
    {
        public static IServiceCollection AddDatabaseServices(this IServiceCollection services)
        {
            var dbName = Guid.NewGuid().ToString();
            services.AddDbContext<ApplicationContext>(options => options.UseInMemoryDatabase(dbName));

            services.AddScoped<IDataItemRepository, DataItemRepository>();
            
            return services;
        }
    }
}