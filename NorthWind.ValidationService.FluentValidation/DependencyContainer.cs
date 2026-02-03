using NorthWind.Validation.Entities.Interfaces;
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
            services.Add(new ServiceDescriptor(
                typeof(IValidationService<>),
                typeof(NorthWind.ValidationService.FluentValidation.FluentValidationService<>),
                ServiceLifetime.Scoped));
            return services;
        }
    }
}
