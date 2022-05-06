/**
 * ::::DOCUMENTO MONGO::::::
 * > Es la reperesentacion de un registro dentro de una cileccion mongo db
 * > Contendra todas las interfaces que pasaran por mi repository generico
 */

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace Servicios.api.Libreria.Core.Entities
{
  public interface IDocument
  {
    //entidades genericas que contendran todos los elementos que pasaran por mi interface repository
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    string Id { get; set; }

    DateTime CreateDate { get; }
  }
}
