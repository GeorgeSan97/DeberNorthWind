using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NorthWind.Entities.Interfaces;
using NorthWind.Sales.Backend.SmtpGateways.Options;

namespace NorthWind.Sales.Backend.SmtpGateways
{
    internal class MailService : IMailService
    {
        private readonly SmtpOptions _options;
        private readonly ILogger<MailService> _logger;

        public MailService(IOptions<SmtpOptions> options, ILogger<MailService> logger)
        {
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task SendMailToAdministrator(string subject, string body)
        {
            if (string.IsNullOrWhiteSpace(subject)) throw new ArgumentException("subject is required", nameof(subject));
            if (string.IsNullOrWhiteSpace(body)) throw new ArgumentException("body is required", nameof(body));

            // Determina remitente y destinatario; adapta nombres si SmtpOptions usa otros campos.
            var fromAddress = !string.IsNullOrWhiteSpace(_options.From)
                ? new MailAddress(_options.From, _options.FromDisplayName)
                : throw new InvalidOperationException("SmtpOptions.From no está configurado.");

            var toAddress = !string.IsNullOrWhiteSpace(_options.AdministratorEmail)
                ? new MailAddress(_options.AdministratorEmail)
                : throw new InvalidOperationException("SmtpOptions.AdministratorEmail no está configurado.");

            using var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = _options.IsBodyHtml
            };

            try
            {
                using var client = new SmtpClient(_options.SmtpHost, _options.SmtpHostPort)
                {
                    EnableSsl = _options.EnableSsl,
                    DeliveryMethod = SmtpDeliveryMethod.Network
                };

                if (!string.IsNullOrWhiteSpace(_options.SmtpUserName))
                {
                    client.Credentials = new NetworkCredential(_options.SmtpUserName, _options.SmtpPassword);
                }

                // Timeouts y otras opciones (si existen)
                if (_options.TimeoutMilliseconds > 0)
                {
                    client.Timeout = _options.TimeoutMilliseconds;
                }

                _logger.LogDebug("Enviando email a administrador: {To}", toAddress.Address);
                await client.SendMailAsync(message);
                _logger.LogInformation("Email enviado correctamente a {To}", toAddress.Address);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error enviando email a administrador");
                throw;
            }
        }
    }
}