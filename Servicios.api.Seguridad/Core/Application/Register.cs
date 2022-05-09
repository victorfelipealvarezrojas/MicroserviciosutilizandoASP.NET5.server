using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Servicios.api.Seguridad.Core.Dto;
using Servicios.api.Seguridad.Core.Entities;
using Servicios.api.Seguridad.Core.JwtLogic;
using Servicios.api.Seguridad.Core.Persistence;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Servicios.api.Seguridad.Core.Application
{
  public class Register
  {
    public class UsuarioRegisterCommand : IRequest<UsuarioDto>
    {
      public string Nombre { get; set; }

      public string Apellido { get; set; }

      public string UserName { get; set; }

      public string Email { get; set; }

      public string Password { get; set; }
    }

    public class UsuarioRegisterValidation : AbstractValidator<UsuarioRegisterCommand>
    {
      public UsuarioRegisterValidation()
      {
        RuleFor(x => x.Nombre).NotEmpty();
        RuleFor(x => x.Apellido).NotEmpty();
        RuleFor(x => x.UserName).NotEmpty();
        RuleFor(x => x.Email).NotEmpty();
        RuleFor(x => x.Password).NotEmpty();
      }
    }

    public class UsuarioRegisterHandler : IRequestHandler<UsuarioRegisterCommand, UsuarioDto>
    {
      private readonly SeguridadContexto _context;
      private readonly UserManager<Usuario> _userManager;
      private readonly IMapper _mapper; // se registra en starup
      private readonly IJwtGenarator _jwtGenarator;

      public UsuarioRegisterHandler(
          SeguridadContexto context,
          UserManager<Usuario> userManager,
          IMapper mapper,
          IJwtGenarator jwtGenarator
        )
      {
        _context = context;
        _userManager = userManager;
        _mapper = mapper;
        _jwtGenarator = jwtGenarator;
      }

      public async Task<UsuarioDto> Handle(UsuarioRegisterCommand request, CancellationToken cancellationToken)
      {
        var existe = await _context.Users.Where(user => user.Email == request.Email).AnyAsync();

        if (existe)
          throw new Exception("El Mail del usuario se encuentra registrado");

        existe = await _context.Users.Where(user => user.UserName == request.UserName).AnyAsync();

        if (existe)
          throw new Exception("El nombre de usuario se encuentra registrado");

        var usuario = new Usuario
        {
          Nombre = request.Nombre,
          Apellido = request.Apellido,
          Email = request.Email,
          UserName = request.UserName
        };

        // para guardar uso los metodos del identity core
        var resultado = await _userManager.CreateAsync(usuario, request.Password);

        if (resultado.Succeeded)
        {
          var usuarioDTO = _mapper.Map<Usuario, UsuarioDto>(usuario);
          usuarioDTO.Token = _jwtGenarator.CreateToken(usuario);
          return usuarioDTO;
        }

        throw new Exception("No se pudo registrar el usuario");

      }
    }
  }
}
