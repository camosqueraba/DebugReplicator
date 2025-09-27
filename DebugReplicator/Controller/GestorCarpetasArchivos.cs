using System;
using System.IO;


namespace DebugReplicator.Controller
{
    public class GestorCarpetasArchivos
    {
        public static bool CopiarDirectorio(string sourceDir, string destinationDir, bool recursive)
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

        public static bool CrearCarpeta(string path)
        {
            if (!Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                    return true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error creating directory: {ex.Message}");
                    return false;
                }
            }
            return true;
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
    }
}
