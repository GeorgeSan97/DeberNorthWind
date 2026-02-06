using Microsoft.Extensions.DependencyInjection;
using NorthWind.Entities.Interfaces; // <- Asegura que apunte a la interfaz correcta
using NorthWind.Sales.Backend.SmtpGateways;

namespace Microsoft.Extensions.DependencyInjection;
public static class DependencyContainer
{
    public static IServiceCollection AddMailServices(
        this IServiceCollection services,
        Action<SmtpOptions> configureSmtpOptions)
    {
        services.Configure(configureSmtpOptions);

        // Registrar explícitamente la interfaz del dominio y la implementación concreta.
        services.AddSingleton<IMailService, MailService>();

        return services;
    }

    public static IServiceCollection AddNorthWindSalesServices(
        this IServiceCollection services,
        Action<SmtpOptions> configureSmtpOptions)
    {
        services
            .AddUseCasesServices()
            .AddRepositories()
            .AddDataContexts(configureDBOptions)
            .AddPresenters()
            .AddValidationService()
            .AddValidators()
            .AddValidationExceptionHandler()
            .AddUpdateExceptionHandler()
            .AddUnhandledExceptionHandler()
            .AddMailServices(configureSmtpOptions)   // <- Registrar IMailService antes
            .AddEventServices();                     // <- Luego registrar handlers/event hub

        return services;
    }
}
