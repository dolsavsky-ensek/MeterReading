using MeterReading.Repositories;
using MeterReading.Repositories.Contracts;
using MeterReading.Services;
using MeterReading.Services.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MeterReading.DependencyInjection
{
    public static class ServiceModules
    {
        public static IServiceCollection AddServiceModules(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<IReadingsService, ReadingsService>();
            services.AddScoped<ICSVProcessingService, CSVProcessingService>();
            services.AddScoped<IValidationService, ValidationService>();
            services.AddScoped<IReadingsRepo, ReadingsRepo>();

            return services;
        }
    }
}
