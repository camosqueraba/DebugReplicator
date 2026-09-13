using DebugReplicator.Model.DTOs;
using System;
using System.IO;


namespace DebugReplicator.Controller
{
    public class GestorCarpetasArchivos
    {
        public static bool CopiarDirectorio(string sourceDir, string destinationDir, bool recursive)
        {
            try
            {
                var dir = new DirectoryInfo(sourceDir);

                if (!dir.Exists)
                    throw new DirectoryNotFoundException($"Source directory not found: {dir.FullName}");

                DirectoryInfo[] dirs = dir.GetDirectories();

                Directory.CreateDirectory(destinationDir);

                foreach (FileInfo file in dir.GetFiles())
                {
                    string targetFilePath = Path.Combine(destinationDir, file.Name);
                    file.CopyTo(targetFilePath, true);
                }

                if (recursive)
                {
                    foreach (DirectoryInfo subDir in dirs)
                    {
                        string newDestinationDir = Path.Combine(destinationDir, subDir.Name);
                        CopiarDirectorio(subDir.FullName, newDestinationDir, true);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                LOGRobotica.Controllers.LogApplication.LogWrite($"GestorCarpetasArchivos -> CopiarDirectorio: {ex.Message}");
                return false;
            }            
        }

        public static bool CopiarCarpeta(string directorioOrigen, string directorioDestino, bool recursive)
        {
            try
            {                
                var dir = new DirectoryInfo(directorioOrigen);

                if (!dir.Exists)
                {
                    LOGRobotica.Controllers.LogApplication.LogWrite("GestorCarpetasArchivos -> CopiarCarpeta: " + $"directorio origen no existe {dir.FullName}");
                    return false;
                }


                DirectoryInfo[] dirs = dir.GetDirectories();

                Directory.CreateDirectory(directorioDestino);

                foreach (FileInfo file in dir.GetFiles())
                {
                    string targetFilePath = Path.Combine(directorioDestino, file.Name);
                    file.CopyTo(targetFilePath, true);
                }

                if (recursive)
                {
                    foreach (DirectoryInfo subDir in dirs)
                    {
                        string newDestinationDir = Path.Combine(directorioDestino, subDir.Name);
                        CopiarDirectorio(subDir.FullName, newDestinationDir, true);
                    }
                }
                
                return true;
            }
            catch (Exception ex)
            {
                LOGRobotica.Controllers.LogApplication.LogWrite("GestorCarpetasArchivos -> CopiarCarpeta: Exception " + ex.Message);
                return false;
            }
            
        }


        public static string ObtenerNombreArchivo(string path)
        {
            string fileName = string.Empty;

            if (!string.IsNullOrWhiteSpace(path))
                fileName = Path.GetFileName(path);

            return fileName;
        }

        public static string ObtenerNombreCarpeta(string rutaCarpetaOrigen)
        {
            string nombreCarpeta = string.Empty;

            if (Directory.Exists(rutaCarpetaOrigen))
            {
                DirectoryInfo infoCarpeta = new DirectoryInfo(rutaCarpetaOrigen);
                nombreCarpeta = infoCarpeta.Name;
            }
                
            return nombreCarpeta;
        }

        public static bool CompruebaTipoArchivoPorExtension(string path, string extension)
        {
            bool esArchivo = false;
            if (!string.IsNullOrWhiteSpace(path))
            {
                string extensionArchivo = Path.GetExtension(path);
                
                if (!string.IsNullOrWhiteSpace(extensionArchivo) && extensionArchivo.Equals(extension, StringComparison.OrdinalIgnoreCase))
                    esArchivo = true;
            }
            return esArchivo;
        }

        public static ResultadoProceso CopiarCarpetaBaseADestino(string rutaCarpetaBase, string rutaCarpetaDestino, string nombreNuevaCarpeta = "")
        {
            ResultadoProceso resultadoProceso = new ResultadoProceso();
            bool copiado = false;

            string nombreCarpetaCopiada = ObtenerNombreArchivo(rutaCarpetaBase);

            if (!string.IsNullOrWhiteSpace(nombreNuevaCarpeta))
                nombreCarpetaCopiada = nombreNuevaCarpeta;

            string nombreNuevaCarpetaDestino = Path.Combine(rutaCarpetaDestino, nombreCarpetaCopiada);

            copiado = CopiarDirectorio(rutaCarpetaBase, nombreNuevaCarpetaDestino, true);


            resultadoProceso.Completado = copiado;
            resultadoProceso.ResultadoContenido = nombreNuevaCarpetaDestino;

            return resultadoProceso;
        }
    }
}
