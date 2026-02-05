using Microsoft.Extensions.DependencyInjection.Extensions;
using NorthWind.HttpDelegatingHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Extensions.DependencyInjection;

public static class DependencyContainer
{
	public static IServiceCollection AddExceptionDelegatingHandler(
	this IServiceCollection services)
	{
		services.TryAddTransient<ExceptionDelegatingHandler>();
		return services;
	}
}
