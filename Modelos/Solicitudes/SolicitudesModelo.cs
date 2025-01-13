using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.Solicitudes
{
    internal class SolicitudesModelo
    {
    }
    public class SolicituInsertarUsuario
    { 
        public string? nombre_usuario {  get; set; }
        public string? apellido_usuario { get; set; }
        public int id_tipo_identificacion { get; set; }
        public string? numero_identificacion { get; set; }
        public int id_tipo_usuario { get; set; }
        public string? correo { get; set; }
        public string? telefono { get; set; }
        public string? pwd { get; set; }
    }

    public class solicitudInsertarMaterias { 
        public string? nombre_materia { get; set; }
        public int creditos {  get; set; }
        public string? codigo_institucional_materia { get; set; }
    }
    public class solicitudInsertarMateriasProfesor {
        public int id_materia { get; set; }
        public int id_profesor { get; set; }
    }
    public class solicitudInsertarMateriasEstudiante
    {
        public int id_materia_profesor { get; set; }
        public int id_estudiante { get; set; }
    }
}
