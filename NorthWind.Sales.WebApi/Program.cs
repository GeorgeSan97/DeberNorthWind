namespace NorthWind.Sales.WebApi;

public class Program
{
  public static void Main(string[] args)
  {
    //  "WebApplication.CreateBuilder(args)": Inicializa el builder de la aplicaci�n web, que:
    //  -Carga la configuraci�n(appsettings.json, variables de entorno, etc.)
    //  -Permite registrar servicios(builder.Services)
    //  -Configura logging, host, etc.

    //  ".CreateWebApplication()": Aqu� se configuran los servicios(inyecci�n de dependencias), incluyendo:
    //  -Casos de uso(UseCases)
    //  -Repositorios(Repositories)
    //  -DataContexts(DbContext)
    //  -Presentadores(Presenters)
    //  -Swagger y CORS

    //  ".ConfigureWebApplication()": Aqu� se configura el pipeline de middlewares, como:
    //  -Swagger(si est� en entorno de desarrollo)
    //  -Mapeo de endpoints(como controladores o minimal APIs)
    //  -CORS

    //  ".Run()". Inicia el servidor web y pone la aplicaci�n a la escucha de solicitudes HTTP
    WebApplication.CreateBuilder(args)
      .CreateWebApplication()
      .ConfigureWebApplication()
      .Run();
  }
}
