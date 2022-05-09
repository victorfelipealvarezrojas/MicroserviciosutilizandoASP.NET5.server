using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Servicios.api.Seguridad.Core.Dto;
using Servicios.api.Seguridad.Core.Entities;
using Servicios.api.Seguridad.Core.JwtLogic;
using Servicios.api.Seguridad.Core.Persistence;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Servicios.api.Seguridad.Core.Application
{
  public class Login
  {
    public class UsuarioLoginCommond : IRequest<UsuarioDto>
    {
      public string Email { get; set; }
      public string Password { get; set; }
    }

    public class UsuarioLoginValidation : AbstractValidator<UsuarioLoginCommond>
    {
      public UsuarioLoginValidation()
      {
        RuleFor(x => x.Email).NotEmpty();
        RuleFor(x => x.Password).NotEmpty();
      }
    }

    public class UsuarioLoginHandler : IRequestHandler<UsuarioLoginCommond, UsuarioDto>
    {
      private readonly SeguridadContexto _context;
      private readonly UserManager<Usuario> _userManager;
      private readonly IMapper _mapper; // se registra en starup
      private readonly IJwtGenarator _jwtGenarator;
      private readonly SignInManager<Usuario> _signInManager;

      public UsuarioLoginHandler(
        SeguridadContexto context,
        UserManager<Usuario> userManager,
        IMapper mapper,
        IJwtGenarator jwtGenarator,
        SignInManager<Usuario> signInManager
      )
      {
        _context = context;
        _userManager = userManager;
        _mapper = mapper;
        _jwtGenarator = jwtGenarator;
        _signInManager = signInManager;
      }

      public async Task<UsuarioDto> Handle(UsuarioLoginCommond request, CancellationToken cancellationToken)
      {
        var usuario = await _userManager.FindByEmailAsync(request.Email);

        if (usuario == null)
          throw new Exception("Usuario no existe");

        var result = await _signInManager.CheckPasswordSignInAsync(usuario, request.Password, false);

        if (result.Succeeded)
        {
          var usuarioDTO = _mapper.Map<Usuario, UsuarioDto>(usuario);
          usuarioDTO.Token = _jwtGenarator.CreateToken(usuario);
          return usuarioDTO;
        }

        throw new Exception("Login Incorrecto");
      }
    }
  }
}
