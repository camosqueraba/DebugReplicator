using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebugReplicator.Model.DTOs
{
    public class ResultadoProceso
    {
        public bool Completado {  get; set; }
        public string ResultadoContenido { get; set; }
        public List<ErrorProceso> Errores { get; set; } = new List<ErrorProceso>();
    }
}
