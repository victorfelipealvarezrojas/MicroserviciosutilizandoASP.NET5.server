/**
 * ::::DOCUMENTO MONGO GENERICO::::::
 * > Es la reperesentacion de un registro dentro de una COLECCION mongo db
 * > Contendra todas las interfaces que pasaran por mi repository generico
 * > Todos los documentos tendranb estos elementos
 */

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Servicios.api.Libreria.Core.Entities
{
  //documento generico que tendran los siguentes elementos x defecto, de esta forma no los declaro dentro de las entidades
  public class Document : IDocument
  {
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }

    public DateTime CreateDate => DateTime.Now;
  }
}
