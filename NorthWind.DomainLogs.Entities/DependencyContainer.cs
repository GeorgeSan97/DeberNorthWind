using NorthWind.DomainLogs.Entities.Interfaces;
using NorthWind.DomainLogs.Entities.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyContainer
{
	public static IServiceCollection AddDomainLogsServices(
	this IServiceCollection services)
	{
		services.AddScoped<IDomainLogger, DomainLogger>();
		return services;
	}
}
