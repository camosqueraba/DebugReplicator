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
        public string CarpetaOrigen { get; set; }
        public string CarpetaDestino { get; set; }
        public string NombreCarpetaReplicada { get; set; }
    }
}
