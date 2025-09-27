using DebugReplicator.ViewModel;

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
