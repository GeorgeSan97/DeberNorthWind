using NorthWind.Events.Entities.Interfaces;
using NorthWind.Events.Entities.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection;
public static class DependencyContainer
{
	public static IServiceCollection AddEventServices(
 this IServiceCollection services)
	{
		services.AddScoped(typeof(IDomainEventHub<>),
		typeof(DomainEventHub<>));
		return services;
	}
}
