using Microsoft.AspNetCore.Http;
using System.Linq;

namespace Servicios.api.Seguridad.Core.JwtLogic
{
  public class UsuarioSession : IUsuarioSession
  {
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UsuarioSession(IHttpContextAccessor httpContextAccessor)
    {
      _httpContextAccessor = httpContextAccessor;
    }

    public string GetUsuarioSession()
    {
      // esta infoemacion llega en el header de la peticion desde el  cliente
      return _httpContextAccessor.HttpContext.User?.Claims?.FirstOrDefault(x => x.Type == "username")?.Value;
    }
  }
}
