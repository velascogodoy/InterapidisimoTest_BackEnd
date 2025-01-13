using Modelos.Consultas;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Servicios.Conexiones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios.CRUD
{
    public interface IConsultasServicios
    {
        Task<dynamic> ConsultaUsuarios_public();
    }
    public class ConsultasServicios : IConsultasServicios
    {
        private ConexionBDMySqlServicio mConexion;

        public ConsultasServicios() 
        {
            mConexion = new ConexionBDMySqlServicio();
        }
        public async Task<dynamic> ConsultaUsuarios_public() 
        {
            return await ConsultaUsuarios();
        }
        private async Task<dynamic> ConsultaUsuarios() 
        {
            List<ConsultaUsuarios> usuarios = new List<ConsultaUsuarios>();
            string consultaUsuarios = @"SELECT * FROM mydb.usuarios us
                                        INNER JOIN tipo_identificacion tpI On us.idTipoIdentificacion = tpI.idTipo_Identificacion
                                        INNER JOIN tipo_usuario tpUs on us.idTipoUsuario = tpUs.idTipo_Usuario";

            var conexion = await mConexion.obtenerConexion();

            if (conexion == null)
            {
                return new
                {
                    Success = false,
                    Message = "Error en la conexión con la BD - E001"
                };
            }
            try
            {
                using (MySqlCommand mySqlcmdUsuarios = new MySqlCommand(consultaUsuarios, conexion))
                {
                    using (MySqlDataReader reader = (MySqlDataReader)await mySqlcmdUsuarios.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            usuarios.Add(new ConsultaUsuarios
                            {
                                id_usuario = reader.GetInt32("idUsuarios"),
                                nombre = reader.GetString("nombreUsuario"), 
                                apellido = reader.GetString("apellidoUsuario"),
                                tipo_identificacion = reader.GetString("nombreIdentificacion"),
                                numero_identificacion = reader.GetString("numeroIdentificacion"),
                                tipo_usuario = reader.GetString("nombreTipoUsuario"),
                                correo = reader.GetString("correo"),
                                telefono = reader.GetString("telefono")
                            });
                        }
                    }
                }
                // Convertir la lista de usuarios a JSON
                return new
                {
                    Success = true,
                    Data = JsonConvert.SerializeObject(usuarios)
                };
            }
            catch (MySqlException ex)
            {
                return new
                {
                    Success = false,
                    Message = "Error de base de datos - E002",
                    Exception = ex.Message
                };
            }
            finally
            {
                if (conexion != null)
                {
                    // Llamar a la función para cerrar la conexión
                    await mConexion.cerrarConexion(conexion);
                }
            }
        }
    }
}
