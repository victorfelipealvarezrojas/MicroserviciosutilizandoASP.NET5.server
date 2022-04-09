using MongoDB.Driver;
using Servicios.api.Libreria.Core.ContextMongoDB;
using Servicios.api.Libreria.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Servicios.api.Libreria.Repository
{
  public class AutorRepository : IAutorRepository
  {

    private readonly IAutorContext autorContext;

    public AutorRepository(IAutorContext autorContext)
    {
      this.autorContext = autorContext;
    }

    public async Task<IEnumerable<Autor>> GetAutores()
    {
      return await this.autorContext.Autores.Find(autorContext => true).ToListAsync();
    }
  }
}
