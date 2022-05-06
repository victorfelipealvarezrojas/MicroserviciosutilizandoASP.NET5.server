﻿using MediatR;
using Microsoft.AspNetCore.Mvc;
using Servicios.api.Seguridad.Core.Dto;
using System.Threading.Tasks;
using static Servicios.api.Seguridad.Core.Application.Register;

namespace Servicios.api.Seguridad.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class UsuarioController : ControllerBase
  {
    private readonly IMediator _mediator;

    public UsuarioController(IMediator mediator)
    {
      _mediator = mediator;
    }

    [HttpPost("registrar")]
    public async Task<ActionResult<UsuarioDto>> Registrar(UsuarioRegisterCommand parmetros)
    {
      return await _mediator.Send(parmetros);
    }
  }
}
