using DiffingAPITask.BusinessLogic.Services;
using DiffingAPITask.BusinessLogic.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace DiffingAPITask.API.Extensions
{
    public static class CustomServicesExtensions
    {
        public static IServiceCollection AddDiffServices(this IServiceCollection services)
        {
            services.AddScoped<IDiffService, DiffService>();
            services.AddScoped<IDataValidationService, DataValidationService>();

            return services;
        }
    }
}