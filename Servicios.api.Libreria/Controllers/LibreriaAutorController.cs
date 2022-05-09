using Microsoft.AspNetCore.Mvc;
using Servicios.api.Libreria.Core.Entities;
using Servicios.api.Libreria.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Servicios.api.Libreria.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class LibreriaAutorController : ControllerBase
  {
    private readonly IMongoRepository<AutorEntity> _autorGenericRepository;

    public LibreriaAutorController(IMongoRepository<AutorEntity> autorGenericRepository)
    {
      this._autorGenericRepository = autorGenericRepository;
    }

    [HttpGet("autorGenerico")]
    public async Task<ActionResult<IEnumerable<AutorEntity>>> Get()
    {
      return Ok(await _autorGenericRepository.GetAll());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AutorEntity>> GetById(string Id)
    {
      var autor = await _autorGenericRepository.GetById(Id);
      return Ok(autor);
    }

    [HttpPost]
    public async Task Post(AutorEntity autor)
    {
      await _autorGenericRepository.InsertDocument(autor);
    }

    [HttpPut("{id}")]
    public async Task Put(string Id, AutorEntity autor)
    {
      autor.Id = Id;
      await _autorGenericRepository.UpdateDocument(autor);
    }

    [HttpDelete("{id}")]
    public async Task Delete(string Id)
    {
      await _autorGenericRepository.DeleteById(Id);
    }

    [HttpPost("paginationby")]
    public async Task<ActionResult<PaginationEntity<AutorEntity>>> PostPaginationBy(PaginationEntity<AutorEntity> pagination)
    {
      var resultados = await _autorGenericRepository.PaginationBy(
          filter => filter.Nombre == pagination.Filter,
          pagination
      );

      return Ok(resultados);
    }


    [HttpPost("pagination")]
    public async Task<ActionResult<PaginationEntity<AutorEntity>>> PostPagination(PaginationEntity<AutorEntity> pagination)
    {
      var resultados = await _autorGenericRepository.PaginationByFilter
      (
         //funciona para el otro tipo de filtro filter => filter.Nombre == pagination.Filter,
         pagination
      );

      return Ok(resultados);
    }
  }
}