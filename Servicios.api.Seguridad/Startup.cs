using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Servicios.api.Seguridad.Core.Application;
using Servicios.api.Seguridad.Core.Entities;
using Servicios.api.Seguridad.Core.Persistence;

namespace Servicios.api.Seguridad
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

      services.AddDbContext<SeguridadContexto>(option =>
      {
        option.UseSqlServer(Configuration.GetConnectionString("ConexionDB"));
      });

      // core identity se encarga de la seguridad y entity framework core persistyencia de c# y bd
      // agrego metodos para crear nuevos usuarios por medio de identity core
      var builder = services.AddIdentityCore<Usuario>();
      var identityBuilder = new IdentityBuilder(builder.UserType, builder.Services); // me permite junstarla core e entity
      identityBuilder.AddEntityFrameworkStores<SeguridadContexto>();// se creo a base del core identity
      identityBuilder.AddSignInManager<SignInManager<Usuario>>();
      services.AddTransient<ISystemClock, SystemClock>(); // lo neceito para registro de hortas del core identity

      services.AddMediatR(typeof(Register.UsuarioRegisterCommand).Assembly); // me permite usarlo en los controller por DI IMEdiator

      services.AddAutoMapper(typeof(Register.UsuarioRegisterHandler)); 

      services.AddControllers().AddFluentValidation(x => x.RegisterValidatorsFromAssemblyContaining<Register>());
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
