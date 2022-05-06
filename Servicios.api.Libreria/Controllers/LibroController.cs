using Microsoft.AspNetCore.Mvc;
using Servicios.api.Libreria.Core.Entities;
using Servicios.api.Libreria.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Servicios.api.Libreria.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class LibroController : Controller
  {
    private readonly IMongoRepository<LibroEntity> _libroRepository;

    public LibroController(IMongoRepository<LibroEntity> libroRepository)
    {
      this._libroRepository = libroRepository;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<LibroEntity>>> Get()
    {
      return Ok(await _libroRepository.GetAll());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<LibroEntity>> GetById(string Id)
    {
      var autor = await _libroRepository.GetById(Id);
      return Ok(autor);
    }

    [HttpPost]
    public async Task Post(LibroEntity libro)
    {
      await _libroRepository.InsertDocument(libro);
    }

    [HttpPost("pagination")]
    public async Task<ActionResult<PaginationEntity<LibroEntity>>> PostPagination(PaginationEntity<LibroEntity> pagination)
    {
      var resultados = await _libroRepository.PaginationByFilter
      (
         pagination
      );

      return Ok(resultados);
    }
  }
}
