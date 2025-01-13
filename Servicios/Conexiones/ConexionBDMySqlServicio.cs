using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Servicios.Conexiones
{
    internal class ConexionBDMySqlServicio
    {
        public async Task<MySqlConnection> obtenerConexion()
        {
            string cadenaConexion = "";
            //Se mapea la ruta sobre el proyecto del appSettings
            string directorioActual = Directory.GetCurrentDirectory();
            string rutaJson = Path.Combine(directorioActual, "appsettings.json");          

            try
            {
                //Se valida si el Archivo AppSetings Existe
                if (!File.Exists(rutaJson)){
                    return null;
                }
                // Se deserializa el appSetings para llamar las propiedades
                string contenidoJson = File.ReadAllText(rutaJson);
                dynamic resJSON = JsonConvert.DeserializeObject(contenidoJson);
                //Se mapea la cadena de conexion MySQL en una variable 
                cadenaConexion = resJSON.stringConectionMYSQLPruebas;
                
                //Se realiza el llamado a la clase para la conexion a MYSQL
                MySqlConnection conexionBD = new MySqlConnection(cadenaConexion);

                //Se Valida que la conexion este cerrada para abrirla
                if (conexionBD.State == ConnectionState.Closed)
                {
                    await conexionBD.OpenAsync();
                }
                return conexionBD;
            }
            catch (MySqlException ex) {
                Console.WriteLine("Error: " + ex.Message);
                return null;
            }            
        }
        public async Task cerrarConexion(MySqlConnection conexion)
        {
            try
            {
                //Se valida si la conexion esta abierta para cerrarla
                if (conexion != null && conexion.State == ConnectionState.Open)
                {
                    await conexion.CloseAsync();
                }
            }
            catch (MySqlException ex)
            {
                Console.WriteLine("Error al cerrar la conexión: " + ex.Message);
            }
        }
    }
}
