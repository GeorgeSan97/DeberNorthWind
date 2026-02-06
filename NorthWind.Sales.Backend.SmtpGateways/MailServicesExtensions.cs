using Microsoft.Extensions.DependencyInjection;
using NorthWind.Entities.Interfaces;
using NorthWind.Sales.Backend.SmtpGateways.Options;

namespace NorthWind.Sales.Backend.SmtpGateways
{
    public static class MailServicesExtensions
    {
        public static IServiceCollection AddMailServices(
            this IServiceCollection services,
            Action<SmtpOptions> configureSmtpOptions)
        {
            services.Configure(configureSmtpOptions);

            services.AddScoped<IMailService, MailService>();
            return services;
        }
    }
}