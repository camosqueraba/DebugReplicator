namespace DebugReplicator.Model.DTOs
{
    public class ErrorProceso
    {
        public bool ExisteError { get; set; }
        public string MensajeError { get; set; } = string.Empty;
    }
}