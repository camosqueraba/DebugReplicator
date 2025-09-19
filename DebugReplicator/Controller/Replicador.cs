using DebugReplicator.Model;
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
                                            string urlCarpertaDestino, string[] archivosIndexar)
        {
            bool ReplicarCarpeta = false;
            /*
            if (Directory.Exists(urlCarpetaDebug) && Directory.Exists(urlCarpertaDestino))
            {

            }
            */

            string nombreCarpetaOrigen = FileSystem.GetName(urlCarpetaDebug);
            string nombreNuevaCarpetaDestino = Path.Combine(urlCarpertaDestino, baseNombreCarpetaReplicada);

            GestorCarpetasArchivos.CopiarDirectorio(urlCarpetaDebug, nombreNuevaCarpetaDestino, true);

            string nombrePrimeraCarpetaIndexada = baseNombreCarpetaReplicada + indiceInicialCarpeta;
            //FileSystem.RenameDirectory(nombreNuevaCarpetaDestino, baseNombreCarpetaReplicada);

            for (int indice = indiceInicialCarpeta; indice <= indiceFinalCarpeta; indice++)
            {
                string nombreCarpetaIndexada = nombreNuevaCarpetaDestino + indice;
                FileSystem.CreateDirectory(nombreCarpetaIndexada);
                GestorCarpetasArchivos.CopiarDirectorio(nombreNuevaCarpetaDestino, nombreCarpetaIndexada, true);

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

        public ResultadoProceso CopiarCarpetaBaseADestino(string rutaCarpetaBase, string rutaCarpetaDestino)
        {
            ResultadoProceso resultadoProceso = new ResultadoProceso();
            bool copiado = false;

            string nombreCarpetaOrigen = GestorCarpetasArchivos.ObtenerNombreArchivo(rutaCarpetaBase);
            string nombreNuevaCarpetaDestino = Path.Combine(rutaCarpetaDestino, nombreCarpetaOrigen);

            GestorCarpetasArchivos.CopiarDirectorio(rutaCarpetaBase, nombreNuevaCarpetaDestino, true);
            copiado = true;

            resultadoProceso.Completado = copiado;
            resultadoProceso.ResultadoContenido = nombreNuevaCarpetaDestino;

            return resultadoProceso;
        }

        public ResultadoProceso ReplicarDebug(string urlCarpetaOrigen, string urlCarpetaDestino, string nombreBaseCarpetaReplica,
            List<IndexedFileModel> archivosIndexados, int numeroReplicas)
        {
            ResultadoProceso resultadoProceso = new ResultadoProceso();
            
            resultadoProceso = ValidarRequisitos(urlCarpetaOrigen, urlCarpetaDestino,
                                                  nombreBaseCarpetaReplica, numeroReplicas, archivosIndexados );
            if (!resultadoProceso.Completado)
                return resultadoProceso;

            resultadoProceso = CopiarCarpetaBaseADestino(urlCarpetaOrigen, urlCarpetaDestino);
            if (!resultadoProceso.Completado)
                return resultadoProceso;

            string rutaCarpetaBase = resultadoProceso.ResultadoContenido;
            string rutaCarpetaBaseDuplicada = Path.Combine(urlCarpetaDestino, nombreBaseCarpetaReplica); 

            for (int indiceReplica = 1; indiceReplica <= numeroReplicas; indiceReplica++)
            {
                bool resultadoCopiar = GestorCarpetasArchivos.CopiarCarpeta(rutaCarpetaBase, rutaCarpetaBaseDuplicada + $"_{indiceReplica}", true);

                if (!resultadoCopiar)
                {
                    resultadoProceso.Completado = false;
                    resultadoProceso.Errores.Add(new ErrorProceso()
                    {
                        MensajeError = $"No se pudo copiar carpeta desde {nombreBaseCarpetaReplica} a {urlCarpetaDestino}"
                    });
                    return resultadoProceso;
                }                
            }
           
            return resultadoProceso;
        }

        public ResultadoProceso CrearCarpetaReplica(string urlCarpetaDestino, string nombreBaseCarpetaReplica, int indiceCarpeta)
        {
            ResultadoProceso resultadoProceso = new ResultadoProceso();
            resultadoProceso.Completado = true;

            string nombreCarpetaReplica = $"{nombreBaseCarpetaReplica}{indiceCarpeta}";
            string rutaCarpetaReplica = Path.Combine(urlCarpetaDestino, nombreCarpetaReplica);            

            bool isCarpetaCreada = GestorCarpetasArchivos.CrearCarpeta(rutaCarpetaReplica);

            if (!isCarpetaCreada)
            {
                resultadoProceso.Completado = false;
                ErrorProceso errorProceso = new ErrorProceso
                {
                    ExisteError = true,
                    MensajeError = $"No se pudo crear la carpeta: {rutaCarpetaReplica}"
                };
                resultadoProceso.Errores.Add(errorProceso);
                return resultadoProceso;
            }

            resultadoProceso.ResultadoContenido = rutaCarpetaReplica;

            return resultadoProceso;
        }

        private ResultadoProceso ValidarRequisitos(string urlCarpetaOrigen, string urlCarpetaDestino, string nombreBaseCarpetaReplica,
            int numeroReplicas, List<IndexedFileModel> archivosIndexados = null)
        {
            ResultadoProceso resultadoProceso = new ResultadoProceso();
            resultadoProceso.Completado = true;

            ErrorProceso errorProceso = new ErrorProceso();

            if (string.IsNullOrEmpty(urlCarpetaOrigen) || !Directory.Exists(urlCarpetaOrigen))
            {
                resultadoProceso.Completado = false;
                errorProceso.ExisteError = true;
                errorProceso.MensajeError = "La carpeta de origen no es válida.";
                resultadoProceso.Errores.Add(errorProceso);

            }

            if (string.IsNullOrEmpty(urlCarpetaDestino) || !Directory.Exists(urlCarpetaDestino))
            {
                resultadoProceso.Completado = false;
                errorProceso.ExisteError = true;
                errorProceso.MensajeError = "La carpeta de destino no es válida.";
                resultadoProceso.Errores.Add(errorProceso);
            }
            
            if (string.IsNullOrEmpty(nombreBaseCarpetaReplica))
            {
                resultadoProceso.Completado = false;
                errorProceso.ExisteError = true;
                errorProceso.MensajeError = "El nombre base de la carpeta replicada no puede estar vacío.";
                resultadoProceso.Errores.Add(errorProceso);
            }
            
            if (archivosIndexados != null && archivosIndexados.Count == 0)
            {
                resultadoProceso.Completado = false;
                errorProceso.ExisteError = true;
                errorProceso.MensajeError = "No se han proporcionado archivos para indexar.";
                resultadoProceso.Errores.Add(errorProceso);
            }
            
            if (numeroReplicas <= 0)
            {
                resultadoProceso.Completado = false;
                errorProceso.ExisteError = true;
                errorProceso.MensajeError = "El número de réplicas debe ser mayor que cero.";
                resultadoProceso.Errores.Add(errorProceso);
            }            
            
            return resultadoProceso;
        }
    }
}
