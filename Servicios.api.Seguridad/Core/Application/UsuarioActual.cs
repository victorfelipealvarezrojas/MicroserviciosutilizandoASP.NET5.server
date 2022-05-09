using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Servicios.api.Seguridad.Core.Dto;
using Servicios.api.Seguridad.Core.Entities;
using Servicios.api.Seguridad.Core.JwtLogic;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Servicios.api.Seguridad.Core.Application
{
  public class UsuarioActual
  {
    public class UsuarioActualCommand : IRequest<UsuarioDto> { }

    public class UsuarioActualHandler : IRequestHandler<UsuarioActualCommand, UsuarioDto>
    {

      private readonly UserManager<Usuario> _userManager;
      private readonly IUsuarioSession _usuarioSession;
      private readonly IJwtGenarator _jwtGenarator;
      private readonly IMapper _mapper; // se registra en starup


      public UsuarioActualHandler(
        UserManager<Usuario> userManager,
        IUsuarioSession usuarioSession,
        IJwtGenarator jwtGenarator,
        IMapper mapper
      )
      {
        _userManager = userManager;
        _usuarioSession = usuarioSession;
        _jwtGenarator = jwtGenarator;
        _mapper = mapper;
      }

      public async Task<UsuarioDto> Handle(UsuarioActualCommand request, CancellationToken cancellationToken)
      {
        var usuario = await _userManager.FindByNameAsync(_usuarioSession.GetUsuarioSession());

        if(usuario != null){
          var usuarioDTO = _mapper.Map<Usuario, UsuarioDto>(usuario);
          usuarioDTO.Token = _jwtGenarator.CreateToken(usuario);
          return usuarioDTO;
        }

        throw new Exception("No se encontro el usuario");
      }
    }
  }
}
