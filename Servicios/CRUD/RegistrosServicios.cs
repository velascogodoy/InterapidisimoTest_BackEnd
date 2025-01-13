using Modelos.Solicitudes;
using MySql.Data.MySqlClient;
using Servicios.Conexiones;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Servicios.CRUD
{
    public interface IRegistrosServicios
    {
        Task<dynamic> InsertarUsuario_public(SolicituInsertarUsuario reqInserUsuario);
        Task<dynamic> InsertarTipos_public(string nombreIdentificacion, int consulta);
        Task<dynamic> InsertarMaterias_Public(solicitudInsertarMaterias reqInsertMateria);
        Task<dynamic> InsertarMaterias_Profesor_public(solicitudInsertarMateriasProfesor reqInsertMateriaProfesor);
        Task<dynamic> InsertarMaterias_Estudiantes_public(solicitudInsertarMateriasEstudiante reqInsertMateriaEstudiante);
    }
    public class RegistrosServicios : IRegistrosServicios
    {
        private ConexionBDMySqlServicio mConexion;

        public RegistrosServicios(
            ) 
        {
            mConexion = new ConexionBDMySqlServicio();
        }
        //Funcion Publica Insertar Usuario
        public async Task<dynamic> InsertarUsuario_public(SolicituInsertarUsuario reqInserUsuario)
        {
            return await InsertarUsuario(reqInserUsuario);
        }

        //Funcion publica Insertar TIpo de identificacion
        public async Task<dynamic> InsertarTipos_public(string nombreIdentificacion, int consulta)
        {
            return await InsertarTipos(nombreIdentificacion, consulta);
        }

        //FUNCion publica Insertar Materias 
        public async Task<dynamic> InsertarMaterias_Public(solicitudInsertarMaterias reqInsertMateria)
        {
            return await InsertarMaterias(reqInsertMateria);
        }

        //Funcion publica para asignar la materia al profesor 
        public async Task<dynamic> InsertarMaterias_Profesor_public(solicitudInsertarMateriasProfesor reqInsertMateriaProfesor)
        {
            return await InsertarMaterias_Profesor(reqInsertMateriaProfesor);
        }

        //Funcion publica para asignar la materia y el profesor al estudiante 
        public async Task<dynamic> InsertarMaterias_Estudiantes_public(solicitudInsertarMateriasEstudiante reqInsertMateriaEstudiante)
        {
            return await InsertarMaterias_Estudiantes(reqInsertMateriaEstudiante);
        }
        private async Task<dynamic> InsertarUsuario(SolicituInsertarUsuario reqInserUsuario)
        {
            string consulta = "INSERT INTO usuarios (nombreUsuario,apellidoUsuario,idTipoIdentificacion,numeroIdentificacion,idTipoUsuario,correo,telefono,pwd) VALUES (@nombre,@apellido,@idTipoId,@numId,@idTipoUsu,@correo,@tel,@pwd)";
            var conexion = await mConexion.obtenerConexion();
            try
            {
                if (conexion != null)
                {
                    using (MySqlCommand mySqlcmd = new MySqlCommand(consulta, conexion))
                    {
                        //Mapear la informacion 
                        mySqlcmd.Parameters.AddWithValue("@nombre", reqInserUsuario.nombre_usuario);
                        mySqlcmd.Parameters.AddWithValue("@apellido", reqInserUsuario.apellido_usuario);
                        mySqlcmd.Parameters.AddWithValue("@idTipoId", reqInserUsuario.id_tipo_identificacion);
                        mySqlcmd.Parameters.AddWithValue("@numId", reqInserUsuario.numero_identificacion);
                        mySqlcmd.Parameters.AddWithValue("@idTipoUsu", reqInserUsuario.id_tipo_usuario);
                        mySqlcmd.Parameters.AddWithValue("@correo", reqInserUsuario.correo);
                        mySqlcmd.Parameters.AddWithValue("@tel", reqInserUsuario.telefono);
                        mySqlcmd.Parameters.AddWithValue("@pwd", reqInserUsuario.pwd);

                        // Ejecutar la consulta
                        int filasAfectadas = await mySqlcmd.ExecuteNonQueryAsync();
                        if (filasAfectadas > 0)
                        {
                            return new { Success = true, Message = "Usuario insertado exitosamente" };
                        }
                        else
                        {
                            return new { Success = false, Message = "No se insertó el usuario" };
                        }
                    }
                }
                else {
                    return new
                    {
                        Success = false,
                        Message = "Error en la conexion con la BD - E001"
                    };
                }
            }
            catch (MySqlException ex) {
                return new { 
                    Success = false, Message = "Error de base de datos - E002", Exception = ex.Message 
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
        private async Task<dynamic> InsertarTipos(string nombreTipo,int consulta)
        {
            //Si es un 1 realiza el insert tipo de identificacion 
            //Si es un 2 realiza el insert tipo de usuario
            string consultaInsertTipo = "";

            if (consulta == 1)
            {
                consultaInsertTipo = "INSERT INTO tipo_identificacion (nombreIdentificacion) VALUES (@nombreTipo)";
            }
            else if (consulta == 2) {
                consultaInsertTipo = "INSERT INTO tipo_usuario (nombreTipoUsuario) VALUES (@nombreTipo)";
            }
            var conexion = await mConexion.obtenerConexion();

            try {
                if (conexion != null)
                {
                    using (MySqlCommand mySqlcmd = new MySqlCommand(consultaInsertTipo, conexion))
                    {
                        //Mapear la informacion 
                        mySqlcmd.Parameters.AddWithValue("@nombreTipo", nombreTipo);

                        // Ejecutar la consulta
                        int filasAfectadas = await mySqlcmd.ExecuteNonQueryAsync();
                        if (filasAfectadas > 0)
                        {
                            return new { Success = true, Message = "Dato insertada exitosamente" };
                        }
                        else
                        {
                            return new { Success = false, Message = "No se insertó el Dato" };
                        }
                    }
                }
                else
                {
                    return new
                    {
                        Success = false,
                        Message = "Error en la conexion con la BD - E001"
                    };
                }
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
        private async Task<dynamic> InsertarMaterias(solicitudInsertarMaterias reqInsertMateria)
        {
            string consultaValidacion = "SELECT COUNT(*) FROM materias";
            string consulta = "INSERT INTO materias (nombreMateria,creditos,codigoInstitucionalMateria) VALUES (@nombre,@creditos,@codMateria)";
            var conexion = await mConexion.obtenerConexion();
            try
            {
                if (conexion != null)
                {
                    //Se realiza la validacion de no insertar mas de 10 Materias
                    using (MySqlCommand validacionCmd = new MySqlCommand(consultaValidacion, conexion))
                    {                         
                        int conteoMaterias = Convert.ToInt32(await validacionCmd.ExecuteScalarAsync());
                        if (conteoMaterias >= 10)
                        {
                            return new { Success = false, Message = "Ya existen 10 materias." };
                        }
                    }

                    using (MySqlCommand mySqlcmd = new MySqlCommand(consulta, conexion))
                    {
                        //Mapear la informacion 
                        mySqlcmd.Parameters.AddWithValue("@nombre", reqInsertMateria.nombre_materia);
                        mySqlcmd.Parameters.AddWithValue("@creditos", reqInsertMateria.creditos);
                        mySqlcmd.Parameters.AddWithValue("@codMateria", reqInsertMateria.codigo_institucional_materia);


                        // Ejecutar la consulta
                        int filasAfectadas = await mySqlcmd.ExecuteNonQueryAsync();
                        if (filasAfectadas > 0)
                        {
                            return new { Success = true, Message = "Materia insertado exitosamente" };
                        }
                        else
                        {
                            return new { Success = false, Message = "No se insertó el Materia" };
                        }
                    }
                }
                else
                {
                    return new
                    {
                        Success = false,
                        Message = "Error en la conexion con la BD - E001"
                    };
                }
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
        private async Task<dynamic> InsertarMaterias_Profesor(solicitudInsertarMateriasProfesor reqInsertMateriaProfesor)
        {
            string consultaValidacion = "SELECT COUNT(*) FROM materias_profesor WHERE idProfesor = @idProfesor";
            string consulta = "INSERT INTO materias_profesor (idMateria,idProfesor) VALUES (@idMateria,@idProfesor)";
            var conexion = await mConexion.obtenerConexion();
            try
            {
                if (conexion != null)
                {
                    //Se realiza la validacion si el profeasor ya tiene asigandas las materias.
                    using (MySqlCommand validacionCmd = new MySqlCommand(consultaValidacion, conexion))
                    {
                        validacionCmd.Parameters.AddWithValue("@idProfesor", reqInsertMateriaProfesor.id_profesor);
                        int conteoMaterias = Convert.ToInt32(await validacionCmd.ExecuteScalarAsync());
                        if (conteoMaterias >= 2)
                        {
                            return new { Success = false, Message = "El profesor ya tiene asignadas dos materias." };
                        }
                    }
                    using (MySqlCommand mySqlcmd = new MySqlCommand(consulta, conexion))
                    {
                        //Mapear la informacion 
                        mySqlcmd.Parameters.AddWithValue("@idMateria", reqInsertMateriaProfesor.id_materia);
                        mySqlcmd.Parameters.AddWithValue("@idProfesor", reqInsertMateriaProfesor.id_profesor);                        

                        // Ejecutar la consulta
                        int filasAfectadas = await mySqlcmd.ExecuteNonQueryAsync();
                        if (filasAfectadas > 0)
                        {
                            return new { Success = true, Message = "Materia asignada insertado exitosamente" };
                        }
                        else
                        {
                            return new { Success = false, Message = "No se asigno la Materia" };
                        }
                    }
                }
                else
                {
                    return new
                    {
                        Success = false,
                        Message = "Error en la conexion con la BD - E001"
                    };
                }
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
        private async Task<dynamic> InsertarMaterias_Estudiantes(solicitudInsertarMateriasEstudiante reqInsertMateriaEstudiante)
        {
            string consultaProfesor = "SELECT idProfesor FROM materias_profesor WHERE idMaterias_Profesor = @idMateriasProfesor";
            string consultaValidacionProfesor = @"SELECT COUNT(*) 
                                FROM materias_estudiantes me
                                JOIN materias_profesor mp ON me.idMateriaProfesor = mp.idMaterias_Profesor
                                WHERE me.idEstudiante = @idEstudiante AND mp.idProfesor = @idProfesor";
            string ConsultaCantMaterias = "SELECT COUNT(*) FROM materias_estudiantes WHERE idEstudiante = @idEstudiante";
            string consulta = "INSERT INTO materias_estudiantes (idMateriaProfesor, idEstudiante) VALUES (@idMateriaProf, @idEstudiante)";

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
                //Consulta sobre el id del profesor
                int idProfesor;
                using (MySqlCommand mySqlcmdProfesor = new MySqlCommand(consultaProfesor, conexion))
                {
                    mySqlcmdProfesor.Parameters.AddWithValue("@idMateriasProfesor", reqInsertMateriaEstudiante.id_materia_profesor);
                    var resultado = await mySqlcmdProfesor.ExecuteScalarAsync();
                    if (resultado == null || !int.TryParse(resultado.ToString(), out idProfesor))
                    {
                        return new { Success = false, Message = "No se encontró el registro del profesor" };
                    }
                }
                //Se realizar la consulta de la cantidad de materias que no supere 3
                using (MySqlCommand validacionCmd = new MySqlCommand(ConsultaCantMaterias, conexion))
                {
                    validacionCmd.Parameters.AddWithValue("@idEstudiante", reqInsertMateriaEstudiante.id_estudiante);
                    int conteoMaterias = Convert.ToInt32(await validacionCmd.ExecuteScalarAsync());
                    if (conteoMaterias >= 3)
                    {
                        return new { Success = false, Message = "El Estudiante ya tiene asignadas 3 materias." };
                    }
                }
                //Se valida si la materia ingresada se repite el profesor 
                using (MySqlCommand validacionCmdProfe = new MySqlCommand(consultaValidacionProfesor, conexion))
                {
                    validacionCmdProfe.Parameters.AddWithValue("@idEstudiante", reqInsertMateriaEstudiante.id_estudiante);
                    validacionCmdProfe.Parameters.AddWithValue("@idProfesor", idProfesor);
                    int conteoRelaciones = Convert.ToInt32(await validacionCmdProfe.ExecuteScalarAsync());
                    if (conteoRelaciones > 0)
                    {
                        return new { Success = false, Message = "El estudiante ya tiene una relación con este profesor." };
                    }
                }
                //se ingresa el registro de la materia al estudiante
                using (MySqlCommand mySqlcmd = new MySqlCommand(consulta, conexion))
                {
                    mySqlcmd.Parameters.AddWithValue("@idMateriaProf", reqInsertMateriaEstudiante.id_materia_profesor);
                    mySqlcmd.Parameters.AddWithValue("@idEstudiante", reqInsertMateriaEstudiante.id_estudiante);
                    int filasAfectadas = await mySqlcmd.ExecuteNonQueryAsync();
                    if (filasAfectadas > 0)
                    {
                        return new { Success = true, Message = "Materia asignada insertado exitosamente" };
                    }
                    else
                    {
                        return new { Success = false, Message = "No se asignó la Materia" };
                    }
                }
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
                    await mConexion.cerrarConexion(conexion);
                }
            }
        }

    }
}
