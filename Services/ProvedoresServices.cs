using MongoDB.Driver;
using Microsoft.Extensions.Options;
using Models;

namespace Services
{
    public class ProveedoresServices
    {
        private readonly IMongoCollection<Proveedores> _proveedorescollention;

        public ProveedoresServices(IOptions<ProveedoresDbSettings> ProveedoresDbSettings)
        {
            var client = new MongoClient
            (
                ProveedoresDbSettings.Value.ConnectionString
            );
            var database = client.GetDatabase
            (
                ProveedoresDbSettings.Value.DatabaseName
            );

            _proveedorescollention = database.GetCollection<Proveedores>
            (
                ProveedoresDbSettings.Value.CollectionName
            );
        }

        public async Task<List<Proveedores>> listaProveedores()
        {
           return await _proveedorescollention.Find(proveedor => true).ToListAsync();
        }

        public async Task<Proveedores> ObtenerProveedor(string id)
        {
            try {
                return await _proveedorescollention.Find<Proveedores>(proveedor => proveedor.Id == id).FirstOrDefaultAsync();
            } catch (Exception ex) {

                throw new Exception("Error al obtener el proveedor", ex);
            }
        }

        public async Task<Proveedores> CrearProveedor(Proveedores proveedor)
        {
            try {
                await _proveedorescollention.InsertOneAsync(proveedor);
                return proveedor;
            } catch (Exception ex) {
                throw new Exception("Error al crear el proveedor", ex);
            }
        }

        public async Task<MessageClass> ActualizarProveedor(string id, Proveedores proveedor)
        {
           Proveedores verificarProveedor;

            try {
                verificarProveedor = await _proveedorescollention.Find<Proveedores>(proveedor => proveedor.Id == id).FirstOrDefaultAsync();
            } catch (Exception) {
              return new MessageClass { Message = "Error al buscar el proveedor" };
            }
            try {
                var update = Builders<Proveedores>.Update
                    .Set("FechaIncorporacion", proveedor.FechaIncorporacion)
                    .Set("NombreEmpresa", proveedor.NombreEmpresa)
                    .Set("NombreVisitador", proveedor.NombreVisitador)
                    .Set("Telefono", proveedor.Telefono)
                    .Set("Direccion", proveedor.Direccion)
                    .Set("DiaVisita", proveedor.DiaVisita);
                    await _proveedorescollention.UpdateOneAsync(proveedor => proveedor.Id == id, update);
                    return new MessageClass { Message = "Proveedor " + proveedor.NombreEmpresa + " actualizado" };

            } catch (Exception ex) {
                return new MessageClass { Message = "Error al actualizar el proveedor " + ex.Message };
            }
        }

        public async Task<MessageClass> EliminarProveedor(string id)
        {
           Proveedores verificarProveedor;

            try {
                verificarProveedor = await _proveedorescollention.Find<Proveedores>(proveedor => proveedor.Id == id).FirstOrDefaultAsync();
            } catch (Exception) {
              return new MessageClass { Message = "Error al buscar el proveedor " };
            }

            try {
                await _proveedorescollention.DeleteOneAsync(proveedor => proveedor.Id == id);
                return new MessageClass { Message = "Proveedor " + verificarProveedor.NombreEmpresa + " eliminado" };
            } catch (Exception ex) {
                return new MessageClass { Message = "Error al eliminar el proveedor " + ex.Message };
            }
        }

    }

}