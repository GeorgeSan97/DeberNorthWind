using NorthWind.Validation.Entities.Interfaces;
using NorthWind.ValidationService.FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DependencyContainer
    {
        public static IServiceCollection AddValidationService(
            this IServiceCollection services)
        {
            services.AddScoped(typeof(IValidationService<>),
            typeof(FluentValidationService<>));
            return services;
        }
    }
}