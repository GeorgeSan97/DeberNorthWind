using NorthWind.Sales.Backend.DataContexts.EFCore.Options;
using NorthWind.Sales.Backend.IoC;
using NorthWind.Sales.Backend.SmtpGateways.Options;

namespace NorthWind.Sales.WebApi;

//  El código expone dos métodos de extensión para configurar los servicios y agregar los
//  middlewares y endpoints de la Web API.
internal static class Startup
{

  //  Agregar soporte para documentación Swagger.
  public static WebApplication CreateWebApplication(this WebApplicationBuilder builder)
  {
    // Esto registra los servicios necesarios para generar la documentación automática Swagger de la API.
    // Configurar APIExplorer para descubrir y exponer los metadatos de los endpoints de la aplicación.    
    builder.Services.AddEndpointsApiExplorer();

    //  Habilita la documentación de la API.
    builder.Services.AddSwaggerGen();

		//  Registrar servicios con Inyección de Dependencias.
		//  Registrar los servicios de la aplicación.
		//  Esto utiliza el contenedor de IoC (DependencyContainer) para registrar todas las dependencias
		//  del dominio NorthWind Sales, incluyendo:
		//  Use Cases, Repositories, Data Contexts, Presenters.
		//  Aquí "DBOptions" representa un objeto que contiene el "ConnectionString" y se carga
		//  desde "appsettings.json".
		builder.Services.AddNorthWindSalesServices(
		dbObtions =>
	    builder.Configuration.GetSection(DBOptions.SectionKey)
	    .Bind(dbObtions),
		smtpOptions => builder.Configuration.GetSection(SmtpOptions.SectionKey)
        .Bind(smtpOptions));

		//  Configurar CORS.
		//  Esto permite que cualquier cliente (como un frontend en Angular, React o Blazor WebAssembly)
		//  pueda consumir la API sin restricciones de origen, método o cabecera.
		//  Habilita el acceso desde otros dominios (útil para frontend).
		builder.Services.AddCors(options =>
    {
      options.AddDefaultPolicy(config =>
          {
            config.AllowAnyMethod();
            config.AllowAnyHeader();
            config.AllowAnyOrigin();
          });
    });

    //  Construye la instancia "WebApplication" con todos los servicios configurados.
    return builder.Build();
  }

  //  Este método se encarga de:
  //  -Habilitar Swagger solo en desarrollo
  //  -Mapear los endpoints de la aplicación
  public static WebApplication ConfigureWebApplication(this WebApplication app)
  {
		app.UseExceptionHandler(builder => { });
		//  Solo cuando el entorno es "Development", se activa Swagger para ver la documentación
		//  de la API y la interfaz UI de Swagger en el navegador.
		if (app.Environment.IsDevelopment())
        {
          app.UseSwagger();
          //  Activa la interfaz (UI) de Swagger para probar la API en desarrollo
          app.UseSwaggerUI();
        }

    //  Registra todos los servicios necesarios usando Clean Architecture (casos de uso,
    //  repositorios, presenters, etc.)
    //  Mapea los controladores implementados, como el de crear órdenes "CreateOrders"
    app.MapNorthWindSalesEndpoints();

    //  Agregar el Middleware CORS
    //  Habilita CORS en tiempo de ejecución para aceptar solicitudes de cualquier origen.
    app.UseCors();
    return app;
  }
}
