using Microsoft.AspNetCore.Mvc;
using Modelos.Respuestas;
using Modelos.Solicitudes;
using Servicios.CRUD;
using Servicios.InicioSesion;

namespace APP_Interapidisimo_Academia.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InterapidisimoUniversidadController : Controller
    {
        private readonly IIniciarSesionServicio IniciarSesionServicio;
        private readonly IRegistrosServicios RegistrosServicios;
        private readonly IConsultasServicios ConsultasServicios;
        public InterapidisimoUniversidadController(
            IIniciarSesionServicio _IniciarSesionServicio,
            IRegistrosServicios _RegistrosServicios,
            IConsultasServicios _ConsultasServicios
            ) 
        {
            IniciarSesionServicio = _IniciarSesionServicio;
            RegistrosServicios = _RegistrosServicios;
            ConsultasServicios = _ConsultasServicios;
        }
        [HttpGet]
        [Route("[Action]")]
        public async Task<IActionResult> InicioSesion()
        {
            //Se instancia el tipo de respuesta estandar para las consulta al servicio
            RespuestaModelo resIni = new RespuestaModelo();
            resIni.server_time = DateTime.Now;
            //se mapea el usuario y contraseña por au
            string authHeader = Request.Headers["Authorization"]!;
            string correo = "";
            string password = "";
            try
            {
                if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Basic "))
                {
                    string encodedCredentials = authHeader.Substring("Basic ".Length).Trim();
                    string credentials = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(encodedCredentials));
                    string[] parts = credentials.Split(':', 2);
                    correo = parts[0];
                    password = parts[1];
                }

                dynamic resSesion = await IniciarSesionServicio.IniciarSesion_public(correo, password);
                resIni.http_status = 200;
                resIni.message = "Inicio de Sesion";
                resIni.result = resSesion;
                return StatusCode(StatusCodes.Status200OK, resIni);
            }
            catch (Exception ex) {
                resIni.http_status =404;
                resIni.message = ex.Message;
                return StatusCode(StatusCodes.Status400BadRequest, resIni);
            }
        }

        [HttpPost]
        [Route("[Action]")]
        public async Task<IActionResult> CrearUsuario([FromBody] SolicituInsertarUsuario reqUsuario) 
        {
            //Se instancia el tipo de respuesta estandar para las consulta al servicio
            RespuestaModelo resUsu = new RespuestaModelo();
            resUsu.server_time = DateTime.Now;
            try
            {
                dynamic resCrearUsuario = await RegistrosServicios.InsertarUsuario_public(reqUsuario);
                resUsu.http_status = 200;
                resUsu.message = "Creacion de Usuario";
                resUsu.result = resCrearUsuario;
                return StatusCode(StatusCodes.Status200OK, resUsu);
            }
            catch (Exception ex) {
                resUsu.http_status = 404;
                resUsu.message = ex.Message;
                return StatusCode(StatusCodes.Status400BadRequest, resUsu);
            }
        }

        [HttpPost]
        [Route("[Action]")]
        public async Task<IActionResult> CrearMateria([FromBody] solicitudInsertarMaterias reqMateria)
        {
            //Se instancia el tipo de respuesta estandar para las consulta al servicio
            RespuestaModelo resMat = new RespuestaModelo();
            resMat.server_time = DateTime.Now;
            try
            {
                dynamic resCrearMateria = await RegistrosServicios.InsertarMaterias_Public(reqMateria);
                resMat.http_status = 200;
                resMat.message = "Creacion de Materia";
                resMat.result = resCrearMateria;
                return StatusCode(StatusCodes.Status200OK, resMat);
            }
            catch (Exception ex)
            {
                resMat.http_status = 404;
                resMat.message = ex.Message;
                return StatusCode(StatusCodes.Status400BadRequest, resMat);
            }
        }

        [HttpPost]
        [Route("[Action]")]
        public async Task<IActionResult> InsertarMateriaProfesor([FromBody] solicitudInsertarMateriasProfesor reqMateriaPro)
        {
            //Se instancia el tipo de respuesta estandar para las consulta al servicio
            RespuestaModelo resMatPro = new RespuestaModelo();
            resMatPro.server_time = DateTime.Now;
            try
            {
                dynamic resCrearMateria = await RegistrosServicios.InsertarMaterias_Profesor_public(reqMateriaPro);
                resMatPro.http_status = 200;
                resMatPro.message = "Asignar Materia a profesor";
                resMatPro.result = resCrearMateria;
                return StatusCode(StatusCodes.Status200OK, resMatPro);
            }
            catch (Exception ex)
            {
                resMatPro.http_status = 404;
                resMatPro.message = ex.Message;
                return StatusCode(StatusCodes.Status400BadRequest, resMatPro);
            }
        }

        [HttpPost]
        [Route("[Action]")]
        public async Task<IActionResult> InsertarMateriaEstudiante([FromBody] solicitudInsertarMateriasEstudiante reqMateriaEstu)
        {
            //Se instancia el tipo de respuesta estandar para las consulta al servicio
            RespuestaModelo resMatEst = new RespuestaModelo();
            resMatEst.server_time = DateTime.Now;
            try
            {
                dynamic resMatEstu = await RegistrosServicios.InsertarMaterias_Estudiantes_public(reqMateriaEstu);
                resMatEst.http_status = 200;
                resMatEst.message = "Asignar Materia al estudiante";
                resMatEst.result = resMatEstu;
                return StatusCode(StatusCodes.Status200OK, resMatEst);
            }
            catch (Exception ex)
            {
                resMatEst.http_status = 404;
                resMatEst.message = ex.Message;
                return StatusCode(StatusCodes.Status400BadRequest, resMatEst);
            }
        }

        [HttpGet]
        [Route("[Action]")]
        public async Task<IActionResult> InsertarTipos([FromQuery] string tipoData, int consulta)
        {
            //Se instancia el tipo de respuesta estandar para las consulta al servicio
            RespuestaModelo resIde = new RespuestaModelo();
            resIde.server_time = DateTime.Now;
            try
            {
                dynamic resCrearUsuario = await RegistrosServicios.InsertarTipos_public(tipoData, consulta);
                resIde.http_status = 200;
                resIde.message = consulta == 1 ?"Creacion de Identificacion": consulta == 2 ? "Creacion Rol de Usuario":"";
                resIde.result = resCrearUsuario;
                return StatusCode(StatusCodes.Status200OK, resIde);
            }
            catch (Exception ex)
            {
                resIde.http_status = 404;
                resIde.message = ex.Message;
                return StatusCode(StatusCodes.Status400BadRequest, resIde);
            }
        }

        [HttpGet]
        [Route("[Action]")]
        public async Task<IActionResult> ConsultarUsuario()
        {
            //Se instancia el tipo de respuesta estandar para las consulta al servicio
            RespuestaModelo resConUsu = new RespuestaModelo();
            resConUsu.server_time = DateTime.Now;
            try
            {
                dynamic resConsultUsuario = await ConsultasServicios.ConsultaUsuarios_public();
                resConUsu.http_status = 200;
                resConUsu.message = "Consulta de usuarios";
                resConUsu.result = resConsultUsuario;
                return StatusCode(StatusCodes.Status200OK, resConUsu);
            }
            catch (Exception ex)
            {
                resConUsu.http_status = 404;
                resConUsu.message = ex.Message;
                return StatusCode(StatusCodes.Status400BadRequest, resConUsu);
            }
        }

    }
}
