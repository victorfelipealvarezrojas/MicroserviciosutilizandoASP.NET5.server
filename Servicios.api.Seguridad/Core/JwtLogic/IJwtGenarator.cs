﻿using Servicios.api.Seguridad.Core.Entities;

namespace Servicios.api.Seguridad.Core.JwtLogic
{
  public interface IJwtGenarator
  {
    string CreateToken(Usuario usuario);

  }
}
