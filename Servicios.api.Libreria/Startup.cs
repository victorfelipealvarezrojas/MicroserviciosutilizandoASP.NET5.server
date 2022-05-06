using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Servicios.api.Libreria.Core;
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
      //services.AddTransient<IAutorContext, AutorContext>();
      //services.AddTransient<IAutorRepository, AutorRepository>();

      //AddScoped siempre que un cliente haga un request al controller se inicia y se destruye en el response
      services.AddScoped(typeof(IMongoRepository<>), typeof(MongoRepository<>));

      services.AddControllers();

      services.AddCors(option =>
      {
        option.AddPolicy("CorsRule", rule =>
        {
          rule.AllowAnyHeader().AllowAnyMethod().WithOrigins("*");
        });
      });
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseRouting();

      app.UseCors("CorsRule");

      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }
  }
}
