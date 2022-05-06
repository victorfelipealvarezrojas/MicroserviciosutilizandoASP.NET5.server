using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Servicios.api.Libreria.Core;
using Servicios.api.Libreria.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Servicios.api.Libreria.Repository
{
  public class MongoRepository<TDocument> : IMongoRepository<TDocument> where TDocument : IDocument
  {
    //me permite identificar la coleccion con la cual trabajare
    private readonly IMongoCollection<TDocument> _collection;

    public MongoRepository(IOptions<MongoSettings> options)
    {
      var _db = new MongoClient(options.Value.ConnectionString).GetDatabase(options.Value.DataBase);

      //obtengo instancia de la coleccion
      _collection = _db.GetCollection<TDocument>(GetCollectionName(typeof(TDocument)));
    }

    //obtengo el nombre de la coleccion que estoy trabajando
    private protected string GetCollectionName(Type documentType)
    {
      //retorna el nombre de la coleccion
      return ((BsonCollectionAttribute)documentType
                                      .GetCustomAttributes(typeof(BsonCollectionAttribute), true)
                                      .FirstOrDefault()).CollectionName;
    }

    public async Task<IEnumerable<TDocument>> GetAll()
    {
      return await _collection.Find(p => true).ToListAsync();
    }

    public async Task<TDocument> GetById(string Id)
    {
      var filter = Builders<TDocument>.Filter.Eq(document => document.Id, Id);

      return await _collection.Find(filter).SingleOrDefaultAsync();
    }

    public async Task InsertDocument(TDocument document)
    {
      await _collection.InsertOneAsync(document);
    }

    public async Task UpdateDocument(TDocument document)
    {
      FilterDefinition<TDocument> filter = Builders<TDocument>.Filter.Eq(document => document.Id, document.Id);
      await _collection.FindOneAndReplaceAsync(filter, document);
    }

    public async Task DeleteById(string Id)
    {
      var filter = Builders<TDocument>.Filter.Eq(document => document.Id, Id);
      await _collection.FindOneAndDeleteAsync(filter);
    }

    //Expression me permite definir un metodo que a futuro sera implementado
    //fitrerExpression representa el flitro q llega desde el cliente
    //por ser generico utilizo un Expression<Func...
    public async Task<PaginationEntity<TDocument>> PaginationBy(Expression<Func<TDocument, bool>> fitrerExpression, PaginationEntity<TDocument> pagination)
    {
      // fitro asc. por el campo que llega en pagination.Sort
      var sort = Builders<TDocument>.Sort.Ascending(pagination.Sort);

      if (pagination.SortDirection == "desc")
      {
        sort = Builders<TDocument>.Sort.Descending(pagination.Sort);
      }

      if (string.IsNullOrEmpty(pagination.Filter))
      {
        pagination.Data = await _collection.Find(p => true)
                                           .Sort(sort)
                                           .Skip((pagination.Page - 1) * pagination.PageSize)
                                           .Limit(pagination.PageSize).ToListAsync();

      }
      else
      {
        pagination.Data = await _collection.Find(fitrerExpression)
                                     .Sort(sort)
                                     .Skip((pagination.Page - 1) * pagination.PageSize) // pagina a visualizar * elementos por pagina
                                     .Limit(pagination.PageSize).ToListAsync();
      }

      long totalDocuments = await _collection.CountDocumentsAsync(FilterDefinition<TDocument>.Empty);
      var totalPage = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(totalDocuments / pagination.PageSize)));
      pagination.PageQuantity = totalPage;

      return pagination;
    }

    public async Task<PaginationEntity<TDocument>> PaginationByFilter(PaginationEntity<TDocument> pagination)
    {
      var sort = Builders<TDocument>.Sort.Ascending(pagination.Sort);
      if (pagination.SortDirection == "desc")
      {
        sort = Builders<TDocument>.Sort.Descending(pagination.Sort);
      }

      var totalDocuments = 0;

      if (pagination.FilterValue == null)
      {
        pagination.Data = await _collection.Find(p => true)
                                           .Sort(sort)
                                           .Skip((pagination.Page - 1) * pagination.PageSize)
                                           .Limit(pagination.PageSize).ToListAsync();

        totalDocuments = (await _collection.Find(p => true).ToListAsync()).Count();
      }
      else
      {
        var valueFilter = ".*" + pagination.FilterValue.Valor + ".*";//expresion regular
        var filter = Builders<TDocument>.Filter.Regex(pagination.FilterValue.Propiedad, new BsonRegularExpression(valueFilter, "i")); //mayus y minus

        pagination.Data = await _collection.Find(filter)
                                     .Sort(sort)
                                     .Skip((pagination.Page - 1) * pagination.PageSize)
                                     .Limit(pagination.PageSize).ToListAsync();

        totalDocuments = (await _collection.Find(filter).ToListAsync()).Count();
      }

      //long totalDocuments = await _collection.CountDocumentsAsync(FilterDefinition<TDocument>.Empty);
      var rounded = Math.Ceiling(totalDocuments / Convert.ToDecimal(pagination.PageSize));
      var totalPage = Convert.ToInt32(rounded);
      pagination.PageQuantity = totalPage;
      pagination.TotalRows = Convert.ToInt32(totalDocuments);

      return pagination;
    }
  }
}
