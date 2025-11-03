using DebugReplicator.Model;
using DebugReplicator.Model.DTOs;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;


namespace DebugReplicator.Controller
{
    public class Replicador
    {        
        private static string RutaCarpetaBase {get; set;} 
        public static ResultadoProceso ReplicarDebug(string urlCarpetaOrigen, string urlCarpetaDestino, string nombreBaseCarpetaReplica,
            int numeroReplicas)
        {
            ResultadoProceso resultadoProceso = new ResultadoProceso();

            resultadoProceso = ValidarRequisitos(urlCarpetaOrigen, urlCarpetaDestino,
                                                  nombreBaseCarpetaReplica, numeroReplicas);
            if (!resultadoProceso.Completado)
                return resultadoProceso;


            resultadoProceso = CopiarCarpetaBaseADestino(urlCarpetaOrigen, urlCarpetaDestino, nombreBaseCarpetaReplica);
            if (!resultadoProceso.Completado)
                return resultadoProceso;

            string rutaCarpetaBase = resultadoProceso.ResultadoContenido;
            string rutaCarpetaBaseDuplicada = Path.Combine(urlCarpetaDestino, nombreBaseCarpetaReplica);

            for (int indiceReplica = 1; indiceReplica <= numeroReplicas; indiceReplica++)
            {
                string rutaCarpetaIndexada = "";

                if (rutaCarpetaBaseDuplicada.Contains(GlobalVars.CARACTER_BANDERA))
                    rutaCarpetaIndexada = rutaCarpetaBaseDuplicada.Replace(GlobalVars.CARACTER_BANDERA, indiceReplica.ToString());
                else
                    rutaCarpetaIndexada = rutaCarpetaBaseDuplicada + "_" + indiceReplica.ToString();

                bool resultadoCopiar = GestorCarpetasArchivos.CopiarCarpeta(rutaCarpetaBase, rutaCarpetaIndexada, true);

                if (!resultadoCopiar)
                {
                    resultadoProceso.Completado = false;
                    resultadoProceso.Errores.Add($"No se pudo copiar carpeta desde {nombreBaseCarpetaReplica} a {urlCarpetaDestino}");
                    return resultadoProceso;
                }                
            }

            resultadoProceso.Completado = true;
            resultadoProceso.ResultadoContenido = "Proceso completado!!";

            return resultadoProceso;
        }

        public static ResultadoProceso ReplicarDebug(string urlCarpetaOrigen, string urlCarpetaDestino, string nombreBaseCarpetaReplica,
            int numeroReplicas, List<IndexedFileModel> archivosIndexados)
        {
            ResultadoProceso resultadoProceso = new ResultadoProceso();

            resultadoProceso = ValidarRequisitos(urlCarpetaOrigen, urlCarpetaDestino,
                                                  nombreBaseCarpetaReplica, numeroReplicas, archivosIndexados);
            if (!resultadoProceso.Completado)
                return resultadoProceso;
                                   
            string rutaCarpetaBaseDuplicada = Path.Combine(urlCarpetaDestino, nombreBaseCarpetaReplica);

            for (int indiceReplica = 1; indiceReplica <= numeroReplicas; indiceReplica++)
            {
                string rutaCarpetaIndexada = "";

                if (rutaCarpetaBaseDuplicada.Contains(GlobalVars.CARACTER_BANDERA))
                    rutaCarpetaIndexada = rutaCarpetaBaseDuplicada.Replace(GlobalVars.CARACTER_BANDERA, indiceReplica.ToString());
                else
                    rutaCarpetaIndexada = rutaCarpetaBaseDuplicada + "_" + indiceReplica.ToString();                
                
                bool resultadoCopiar = GestorCarpetasArchivos.CopiarCarpeta(rutaCarpetaBaseDuplicada, rutaCarpetaIndexada, true);

                if (!resultadoCopiar)
                {
                    resultadoProceso.Completado = false;
                    resultadoProceso.Errores.Add($"No se pudo copiar carpeta desde {nombreBaseCarpetaReplica} a {urlCarpetaDestino}");
                    return resultadoProceso;
                }

                if (archivosIndexados != null || archivosIndexados.Count > 0)
                {
                    ResultadoProceso resultadoIndexar = IndexarCarpeta(rutaCarpetaIndexada, archivosIndexados, indiceReplica, true);
                    if (!resultadoIndexar.Completado)
                    {
                        resultadoProceso.Completado = false;
                        resultadoProceso.Errores.AddRange(resultadoIndexar.Errores);
                        return resultadoProceso;
                    }
                }
            }

            resultadoProceso.Completado = true;
            resultadoProceso.ResultadoContenido = "Proceso completado!!";

            return resultadoProceso;
        }

        public static ResultadoProceso CopiarCarpetaBaseADestino(string rutaCarpetaBase, string rutaCarpetaDestino, string nombreNuevaCarpeta="")
        {
            ResultadoProceso resultadoProceso = new ResultadoProceso();
            bool copiado = false;

            string nombreCarpetaCopiada = GestorCarpetasArchivos.ObtenerNombreArchivo(rutaCarpetaBase);

            if (!string.IsNullOrWhiteSpace(nombreNuevaCarpeta))
                nombreCarpetaCopiada = nombreNuevaCarpeta;

            string nombreNuevaCarpetaDestino = Path.Combine(rutaCarpetaDestino, nombreCarpetaCopiada);

            GestorCarpetasArchivos.CopiarDirectorio(rutaCarpetaBase, nombreNuevaCarpetaDestino, true);
            copiado = true;

            resultadoProceso.Completado = copiado;
            resultadoProceso.ResultadoContenido = RutaCarpetaBase = nombreNuevaCarpetaDestino;

            return resultadoProceso;
        }
        
        
        private static ResultadoProceso ValidarRequisitos(string urlCarpetaOrigen, string urlCarpetaDestino, string nombreBaseCarpetaReplica,
            int numeroReplicas, List<IndexedFileModel> archivosIndexados = null)
        {
            ResultadoProceso resultadoProceso = new ResultadoProceso();
            resultadoProceso.Completado = true;

            

            if (string.IsNullOrEmpty(urlCarpetaOrigen) || !Directory.Exists(urlCarpetaOrigen))
            {
                resultadoProceso.Completado = false;                
                resultadoProceso.Errores.Add("La carpeta de origen no es válida.");

            }

            if (string.IsNullOrEmpty(urlCarpetaDestino) || !Directory.Exists(urlCarpetaDestino))
            {
                resultadoProceso.Completado = false;
                resultadoProceso.Errores.Add("La carpeta de destino no es válida.");
            }
            
            if (string.IsNullOrEmpty(nombreBaseCarpetaReplica))
            {
                resultadoProceso.Completado = false;
                resultadoProceso.Errores.Add("El nombre base de la carpeta replicada no puede estar vacío.");
            }
            
            if (archivosIndexados != null && archivosIndexados.Count == 0)
            {
                resultadoProceso.Completado = false;
                resultadoProceso.Errores.Add("No se han proporcionado archivos para indexar.");
            }
            
            if (numeroReplicas <= 0)
            {
                resultadoProceso.Completado = false;               
                resultadoProceso.Errores.Add("El número de réplicas debe ser mayor que cero.");
            }            
            
            return resultadoProceso;
        }      
        
        private static ResultadoProceso IndexarCarpeta(string rutaCarpetaBase, List<IndexedFileModel> archivosIndexados, int indice, bool revisarSubcarpetas)
        {
            ResultadoProceso resultadoProceso = new ResultadoProceso();

            try
            {
                foreach (var archivoIndexado in archivosIndexados)
                {
                    string rutaArchivoOriginal = archivoIndexado.Path;
                    string nombreArchivoIndexado = GestorCarpetasArchivos.ObtenerNombreArchivo(rutaArchivoOriginal);
                    string nuevoNombreArchivo = archivoIndexado.NombreIndexado.Replace(GlobalVars.CARACTER_BANDERA, indice.ToString());
                    string rutaNuevoArchivo = Path.Combine(rutaCarpetaBase, nuevoNombreArchivo);

                    var carpetaBaseIndexada = new DirectoryInfo(rutaCarpetaBase);

                    if (!carpetaBaseIndexada.Exists)
                    {
                        LOGRobotica.Controllers.LogApplication.LogWrite("GestorCarpetasArchivos -> IndexarCarpeta: " + $"directorio origen no existe {carpetaBaseIndexada.FullName}");

                        resultadoProceso.Completado = false;
                        resultadoProceso.Errores.Add($"directorio origen no existe {carpetaBaseIndexada.FullName}");

                        return resultadoProceso;
                    }

                    DirectoryInfo[] carpetaBaseIndexadaInfo = carpetaBaseIndexada.GetDirectories();

                    foreach (FileInfo file in carpetaBaseIndexada.GetFiles())
                    {
                        if (file.Name == archivoIndexado.Name)
                        {
                            if(file.Name != nuevoNombreArchivo) 
                                FileSystem.RenameFile(file.FullName, nuevoNombreArchivo);
                        }                        
                    }

                    if (revisarSubcarpetas)
                    {
                        foreach (DirectoryInfo subDir in carpetaBaseIndexadaInfo)
                        {
                            IndexarCarpeta(subDir.FullName, archivosIndexados, indice, true);
                        }
                    }
                                       
                }
                resultadoProceso.Completado = true;
                return resultadoProceso;
            }
            catch (Exception ex)
            {
                LOGRobotica.Controllers.LogApplication.LogWrite("Replicador -> IndexarCarpeta: Exception " + ex.Message);
                resultadoProceso.Completado = false;
                resultadoProceso.Errores.Add(ex.Message);
                return resultadoProceso;
            }

        }

        public static List<ClaveValorModel> LeerArchivoConfiguraciones(string rutaArchivoConfig)
        {
            List<ClaveValorModel> configuraciones = new List<ClaveValorModel>();
            try
            {
                if (File.Exists(rutaArchivoConfig))
                {
                    Dictionary<string, string> paresClaveValor = LectorArchivosConfiguracion.LeerArchivoConfiguracionExterno(rutaArchivoConfig);
                    
                    var configuracionesConfig = ConfigurationManager.AppSettings;
                    string[] claves = new string[configuracionesConfig.Count];
                    
                    foreach (var claveValor in paresClaveValor)
                    {
                        string clave = claveValor.Key;
                        string valor = claveValor.Value;

                        configuraciones.Add(new ClaveValorModel()
                        {
                            Clave = clave,
                            Valor = valor
                        });                        
                    }
                    
                }
            }
            catch (Exception ex)
            {
                LOGRobotica.Controllers.LogApplication.LogWrite("Replicador -> LeerArchivoConfiguraciones: Exception " + ex.Message);
            }
            return configuraciones;
        }

    }
}
