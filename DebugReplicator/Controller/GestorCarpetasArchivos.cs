using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebugReplicator.Controller
{
    public class GestorCarpetasArchivos
    {
        public static void CopiarDirectorio(string sourceDir, string destinationDir, bool recursive)
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
            return true; // Directory already exists
        }

        public static string ObtenerNombreArchivo(string path)
        {
            string fileName = string.Empty;

            if (!string.IsNullOrWhiteSpace(path))
                fileName = Path.GetFileName(path);
            return fileName;
        }
    }
}
