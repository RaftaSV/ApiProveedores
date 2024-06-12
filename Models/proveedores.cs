using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Models
{
    public class Proveedores
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string ? Id { get; set; }
    
        public DateOnly FechaIncorporacion { get; set; }
        public string ? NombreEmpresa { get; set; }
        public string ? NombreVisitador { get; set; }
        public string ? Telefono { get; set; }
        public string ? Direccion { get; set; }

        public string ? DiaVisita { get; set; }
    }
}