using Servicios.api.Seguridad.Core.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System;

namespace Servicios.api.Seguridad.Core.JwtLogic
{
  public class JwtGenerator : IJwtGenarator
  {
    public string CreateToken(Usuario usuario)
    {
      var claims = new List<Claim>
      {
        new Claim("username", usuario.UserName),
        new Claim("nombre", usuario.Nombre),
        new Claim("apellido", usuario.Apellido)
      };

      var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("irCLQmiiQmwlb3DxgijUQyDRGgu1zUlk"));
      var credential = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

      var tokenDescription = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(claims),
        Expires = DateTime.Now.AddDays(3),
        SigningCredentials = credential
      };

      var tokenHandler = new JwtSecurityTokenHandler();
      var token = tokenHandler.CreateToken(tokenDescription);

      return tokenHandler.WriteToken(token);
    }
  }
}

