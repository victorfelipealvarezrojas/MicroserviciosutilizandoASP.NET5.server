using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Servicios.api.Libreria.Core;
using Servicios.api.Libreria.Core.ContextMongoDB;
using Servicios.api.Libreria.Repository;

namespace Servicios.api.Libreria
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {

      services.Configure<MongoSettings>(
      options =>
      {
        options.ConnectionString = Configuration.GetSection("MongoDb:ConnectionString").Value;
        options.DataBase = Configuration.GetSection("MongoDb:Database").Value;
      });
      services.AddSingleton<MongoSettings>();

      //DEPENDENCY INJECTION
      //AddTransient genera intancias por cada metodo individual que se va ejecutando, al momento de que el client consuma la API
      services.AddTransient<IAutorContext, AutorContext>();
      services.AddTransient<IAutorRepository, AutorRepository>();

      services.AddControllers();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseRouting();

      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }
  }
}
