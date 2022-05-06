/**
 * ::::::::::::::::::::::::::::::::::::::::::
 * ::::::::PATRON REPOSITORY GENERICO::::::::
 * ::::::::::::::::::::::::::::::::::::::::::
 * >clase generiaca que contendra toods los metodos para todos los documentos de mongo
 */
using Servicios.api.Libreria.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Servicios.api.Libreria.Repository
{
  //Interface de tipo <TDocument> generica que pertenece a IDocument(where TDocument : IDocument) como condicion
  public interface IMongoRepository<TDocument> where TDocument : IDocument
  {
    Task<IEnumerable<TDocument>> GetAll();

    Task<TDocument> GetById(string Id);

    Task InsertDocument(TDocument document);

    Task UpdateDocument(TDocument document);

    Task DeleteById(string Id);

    Task<PaginationEntity<TDocument>> PaginationBy(
        Expression<Func<TDocument, bool>> fitrerExpression, //ex... me opermite definir un metodo/funcionalidad q se implementara a futuro
        PaginationEntity<TDocument> pagination
    );

    Task<PaginationEntity<TDocument>> PaginationByFilter(PaginationEntity<TDocument> pagination);
  }
}
