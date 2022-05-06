using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Servicios.api.Seguridad.Core.Entities;
using Servicios.api.Seguridad.Core.Persistence;
using System;

namespace Servicios.api.Seguridad
{
  public class Program
  {
    public static void Main(string[] args)
    {
      // CreateHostBuilder(args).Build().Run();
      var hostServer = CreateHostBuilder(args).Build();

      using (var context = hostServer.Services.CreateScope())
      {
        var services = context.ServiceProvider;

        try
        {
          var userManager = services.GetRequiredService<UserManager<Usuario>>();
          var _contextEntityF = services.GetRequiredService<SeguridadContexto>();

          SeguridadData.InsertarUsuario(_contextEntityF, userManager).Wait();
        }
        catch (Exception e)
        {
          var loggin = services.GetRequiredService<ILogger<Program>>();
          loggin.LogError(e, "Error en beans registrar usuario.");
        }
      }

      hostServer.Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
              webBuilder.UseStartup<Startup>();
            });
  }
}
