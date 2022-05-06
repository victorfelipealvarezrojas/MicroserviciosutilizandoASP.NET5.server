using Microsoft.AspNetCore.Identity;
using Servicios.api.Seguridad.Core.Entities;
using System.Linq;
using System.Threading.Tasks;

namespace Servicios.api.Seguridad.Core.Persistence
{
  public class SeguridadData
  {
    // beans fonf into program.cs
    public static async Task InsertarUsuario(SeguridadContexto contexto, UserManager<Usuario> usuarioManager)
    {
      if (!usuarioManager.Users.Any())
      {
        var usuario = new Usuario
        {
          Nombre = "Victor",
          Apellido = "Alvarez Rojas",
          Direccion = "Santiago",
          UserName = "valvarez",
          Email = "valvarez@gmail.com"
        };

        await usuarioManager.CreateAsync(usuario, "Password123$");
      }
    }
  }
}
