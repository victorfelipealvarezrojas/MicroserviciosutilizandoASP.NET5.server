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
    private readonly IMongoRepository<AutorEntity> _autorRepository;

    public LibreriaServicioController(IMongoRepository<AutorEntity> autorRepository)
    {
      this._autorRepository = autorRepository;
    }

    [HttpGet("autores")]
    public async Task<ActionResult<IEnumerable<AutorEntity>>> GetAutores()
    {
      var response = await this._autorRepository.GetAll();

      return Ok(response);
    }
  }
}
