using DebugReplicator.Model.DTOs;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DebugReplicator.Controller
{
    public class Replicador
    {
        public bool ReplicarCarpetaDebug(string urlCarpetaDebug, string baseNombreCarpetaReplicada,
                                            int indiceInicialCarpeta, int indiceFinalCarpeta,
                                            string urlCarpertaDestino, string[] archivosIndexar )
        {
            bool ReplicarCarpeta = false;
            /*
            if (Directory.Exists(urlCarpetaDebug) && Directory.Exists(urlCarpertaDestino))
            {

            }
            */

            string nombreCarpetaOrigen = FileSystem.GetName(urlCarpetaDebug);
            string nombreNuevaCarpetaDestino = Path.Combine(urlCarpertaDestino, baseNombreCarpetaReplicada);

            GestorCarpetasArchivos.CopyDirectory(urlCarpetaDebug, nombreNuevaCarpetaDestino, true);

            string nombrePrimeraCarpetaIndexada = baseNombreCarpetaReplicada + indiceInicialCarpeta;
            //FileSystem.RenameDirectory(nombreNuevaCarpetaDestino, baseNombreCarpetaReplicada);

            for (int indice = indiceInicialCarpeta; indice <= indiceFinalCarpeta; indice++)
            {
                string nombreCarpetaIndexada = nombreNuevaCarpetaDestino + indice;
                FileSystem.CreateDirectory(nombreCarpetaIndexada);
                GestorCarpetasArchivos.CopyDirectory(nombreNuevaCarpetaDestino, nombreCarpetaIndexada, true);

            }

            return ReplicarCarpeta;
        }


        public bool ReplicarExe(string UrlExe)
        {
            bool replicarExe = false;

            if (!string.IsNullOrEmpty(UrlExe) && File.Exists(UrlExe))
            {

            }
            
            return replicarExe;
        }

        public ResultadoProceso CopiarCarpetaBaseADestino(string urlCarpetaBase, string carpetaDestino)
        {
            ResultadoProceso resultadoProceso = new ResultadoProceso();
            bool copiado = false;

            string nombreCarpetaOrigen = FileSystem.GetName(urlCarpetaBase);
            string nombreNuevaCarpetaDestino = Path.Combine(carpetaDestino, nombreCarpetaOrigen);

            GestorCarpetasArchivos.CopyDirectory(urlCarpetaBase, nombreNuevaCarpetaDestino, true);
            copiado = true;

            resultadoProceso.Resultado = copiado;
            resultadoProceso.ResultadoContenido = nombreNuevaCarpetaDestino;

            return resultadoProceso;
        }
    }
}
