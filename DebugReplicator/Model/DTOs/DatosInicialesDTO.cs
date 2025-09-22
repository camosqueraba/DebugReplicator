using DebugReplicator.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebugReplicator.Model.DTOs
{
    public class DatosInicialesDTO : BaseViewModel
    {
        public string RutaCarpetaOrigen { get; set; }
        public string RutaCarpetaDestino { get; set; }
        public string RutaCarpetaReplicada { get; set; }
        public string NombreCarpetaReplicada { get; set; }
        public int NumeroReplicas { get; set; }
    }
}
