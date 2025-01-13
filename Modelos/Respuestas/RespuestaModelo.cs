using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelos.Respuestas
{
    public class RespuestaModelo
    {
        public int http_status { get; set; }
        public string? message { get; set; }
        public object? result { get; set; }
        public DateTime server_time { get; set; }
    }
}
