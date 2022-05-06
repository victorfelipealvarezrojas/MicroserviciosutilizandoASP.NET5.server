/**
 * ::::ENTIDAD QUE REPRESENTA EL NOMBRE DE LA COLLECCION POR DOCUMENTO::::
 * > me permite identificar la coleccion con la cual estoy trabajando
 * > como estoy trabajando con documentos genericos x defecto no existe
 * >>ningun metodo que me indique cual es la coleccion que se va a trabajar
 */
using System;

namespace Servicios.api.Libreria.Core.Entities
{
  //me permitira identificar la coleccion con la cual trabajara el metodo
  [AttributeUsage(AttributeTargets.Class, Inherited = false)]
  public class BsonCollectionAttribute : Attribute
  {
    public string CollectionName { get; }

    public BsonCollectionAttribute(string collectionName)
    {
      this.CollectionName = collectionName;
    }
  }
}
