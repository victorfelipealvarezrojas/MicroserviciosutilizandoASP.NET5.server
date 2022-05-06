using MongoDB.Bson.Serialization.Attributes;

namespace Servicios.api.Libreria.Core.Entities
{
  [BsonCollection("Autor")]
  public class AutorEntity : Document
  {
    [BsonElement("nombre")]
    public string Nombre { get; set; }

    [BsonElement("apellidos")]
    public string Apellidos { get; set; }

    [BsonElement("gradoAcademico")]
    public string GradoAcademico { get; set; }

    [BsonElement("nombreCompleto")]
    public string NombreCompleto { get; set; }
  }
}
