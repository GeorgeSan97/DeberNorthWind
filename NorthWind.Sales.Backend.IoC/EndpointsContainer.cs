using Microsoft.AspNetCore.Builder;
using NorthWind.Sales.Backend.Controllers.CreateOrder;
using NorthWind.Membership.Entities.Dtos.UserRegistration;
using NorthWind.Membership.Backend.AspNetIdentity.Options;

namespace NorthWind.Sales.Backend.IoC;

//  Clase estática usada como contenedor de dependencias/servicios de los endpoints
//  HTTP que expone la aplicación.
public static class EndpointsContainer
{
  //  Método de extensión para que desde Program.cs se pueda registrar fácilmente
  //  todos los endpoints de la capa externa, de forma limpia y modular: app.MapNorthWindSalesEndpoints();
  public static WebApplication MapNorthWindSalesEndpoints(this WebApplication app)
  {
    //  Este método está definido en "CreateOrderController" y se encarga de mapear un endpoint
    //  de tipo POST en la API para crear órdenes, como por ejemplo: app.MapPost("/orders", CreateOrder);
    //  Es parte de los adaptadores de entrada, y llama internamente al InputPort y OutputPort (Clean Architecture).
    app.UseCreateOrderController();
    app.UseMembershipEndpoints();

		return app;
  }
}
//  ----------------------------------------------------
//  ¿Cómo y donse se usaría esto?