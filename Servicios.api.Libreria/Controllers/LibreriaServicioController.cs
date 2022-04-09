using Microsoft.AspNetCore.Mvc;
using Servicios.api.Libreria.Core.Entities;
using Servicios.api.Libreria.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Servicios.api.Libreria.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class LibreriaServicioController : ControllerBase
  {
    private readonly IAutorRepository autorRepository;

    public LibreriaServicioController(IAutorRepository autorRepository)
    {
      this.autorRepository = autorRepository;
    }

    [HttpGet("autores")]
    public async Task<ActionResult<IEnumerable<Autor>>> GetAutores()
    {
      var response = await this.autorRepository.GetAutores();

      return Ok(response);
    }
  }
}
