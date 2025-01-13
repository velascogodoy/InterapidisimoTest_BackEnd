using MySql.Data.MySqlClient;
using Servicios.Conexiones;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Servicios.InicioSesion
{
    public interface IIniciarSesionServicio
    {
        Task<dynamic> IniciarSesion_public(string correo, string pwd);
    }
    public class IniciarSesionServicio : IIniciarSesionServicio
    {
        private ConexionBDMySqlServicio mConexion;
        public IniciarSesionServicio() {
            mConexion = new ConexionBDMySqlServicio();
        }
        public async Task<dynamic> IniciarSesion_public(string correo, string pwd)
        {
            return await IniciarSesion(correo, pwd);
        }
        private async Task<dynamic> IniciarSesion(string correo, string pwd) 
        {
            string resultado = "";
            MySqlDataReader mySqlReader = null;
            string consulta = "SELECT correo, pwd,nombreUsuario,apellidoUsuario,idTipoUsuario FROM usuarios WHERE correo = @correo";
            var conexion = await mConexion.obtenerConexion();
            try
            {
                if (conexion != null)
                {
                    using (MySqlCommand mySqlcmd = new MySqlCommand(consulta, conexion))
                    {
                        // Agregar parámetro de correo
                        mySqlcmd.Parameters.AddWithValue("@correo", correo); 
                        // Usar la versión asincrónica
                        mySqlReader = (MySqlDataReader?)await mySqlcmd.ExecuteReaderAsync();
                        // Si se encuentra el correo
                        if (await mySqlReader.ReadAsync())
                        {
                            // Obtener la contraseña almacenada
                            string getPassword = mySqlReader.GetString(1); 

                            if (getPassword == pwd) // Verificar la contraseña
                            {
                                string nombreusuario = mySqlReader.GetString(2);
                                string ApellidoUsuario = mySqlReader.GetString(3);
                                int tipoUsuario = mySqlReader.GetInt32(4);

                                return new { Success = true, 
                                    Message = "Inicio de sesión exitoso",
                                    nombreUsuario = nombreusuario+" "+ ApellidoUsuario,
                                    TipoUsuario = tipoUsuario
                                };
                            }
                            else
                            {
                                return new { Success = false, Message = "Contraseña incorrecta" };
                            }
                        }
                        else
                        {
                            return new { Success = false, Message = "El correo no existe" };
                        }
                    }
                }
                return resultado;
            }
            catch (MySqlException ex)
            {
                return new { resultado, ex.Message };
            }
            finally
            {
                if (mySqlReader != null && !mySqlReader.IsClosed)
                {
                    await mySqlReader.CloseAsync(); // Cerrar el MySqlDataReader de manera asincrónica
                }
                if (conexion != null)
                {
                    await mConexion.cerrarConexion(conexion); // Llamar a la función para cerrar la conexión
                }
            }
        }
    }
}
