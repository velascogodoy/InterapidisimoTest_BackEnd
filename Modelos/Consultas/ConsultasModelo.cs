using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.Consultas
{
    internal class ConsultasModelo
    {
    }
    public class ConsultaUsuarios { 
        public int id_usuario { get; set; }
        public string? nombre { get; set; }
        public string? apellido { get; set; }
        public string? tipo_identificacion { get; set; }
        public string? numero_identificacion { get; set; }
        public string? tipo_usuario { get; set; }
        public string? correo { get; set; }
        public string? telefono { get; set; }

    }
}
