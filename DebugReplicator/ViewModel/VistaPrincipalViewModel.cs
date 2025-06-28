using DebugReplicator.Controller;
using DebugReplicator.Controller.Services;
using DebugReplicator.Model.DTOs;
using DebugReplicator.View;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;


namespace DebugReplicator.ViewModel
{
    public class VistaPrincipalViewModel: BaseViewModel, INotifyDataErrorInfo
    {
        private readonly NavigationStore _navigationStore;
        private readonly IFolderDialogService _folderDialog;
        
        private string carpetaOrigen;
        public string CarpetaOrigen
        {
            get => carpetaOrigen;
            set
            {
                
                if (SetProperty(ref carpetaOrigen, value))
                {
                    Validar(nameof(CarpetaOrigen));
                    Validar(nameof(CarpetaDestino)); // También depende de ella
                }
            }
        }

        private string carpetaDestino;
        public string CarpetaDestino
        {
            get => carpetaDestino;
            set
            {
                if (SetProperty(ref carpetaDestino, value))
                {
                    Validar(nameof(CarpetaDestino));
                    Validar(nameof(CarpetaOrigen)); // También depende de ella
                }
            }
        }

        private string nombreCarpetaReplicada;        

        public string NombreCarpetaReplicada
        {
            get => nombreCarpetaReplicada;
            set
            {
                if (SetProperty(ref nombreCarpetaReplicada, value))
                    Validar(nameof(NombreCarpetaReplicada));
            }
        }

        public string CarpetaOrigenError => GetFirstError(nameof(CarpetaOrigen));
        public string CarpetaDestinoError => GetFirstError(nameof(CarpetaDestino));
        public string NombreCarpetaReplicadaError => GetFirstError(nameof(NombreCarpetaReplicada));

        public ICommand SiguienteCommand { get; }
        public ICommand SeleccionarCarpetaOrigenCommand { get; }
        public ICommand SeleccionarCarpetaDestinoCommand { get; }
        


        Dictionary<string, List<string>> Errores = new Dictionary<string, List<string>>();
        



        public VistaPrincipalViewModel(NavigationStore navigationStore, IFolderDialogService folderDialog)
        {
            _navigationStore = navigationStore;
            _folderDialog = folderDialog;

            SiguienteCommand = new RelayCommand(Siguiente, () => !HasErrors);
            
            SeleccionarCarpetaOrigenCommand = new RelayCommand(SeleccionarCarpetaOrigen);
            SeleccionarCarpetaDestinoCommand = new RelayCommand(SeleccionarCarpetaDestino);
                   
            
        }

        private string GetFirstError(string propertyName)
        {
            return Errores.ContainsKey(propertyName) ? Errores[propertyName].FirstOrDefault() ?? "" : "";
        }


        private void Siguiente()
        {

            if (string.IsNullOrWhiteSpace(CarpetaOrigen))
                CarpetaOrigen = "";

            if (string.IsNullOrWhiteSpace(CarpetaDestino))
                CarpetaDestino = "";

            if (string.IsNullOrWhiteSpace(NombreCarpetaReplicada))
                NombreCarpetaReplicada = "";

            Validar(CarpetaOrigen);
            Validar(CarpetaDestino);
            Validar(NombreCarpetaReplicada);

            if (HasErrors)
            {
                MessageBox.Show("Corrige los errores antes de continuar.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            //string 
            Replicador replicador = new Replicador();
            string[] archivos_indexar = new string[2];
            //replicador.ReplicarCarpetaDebug(RutaCarpetaOrigen, "BOT_", 1, 10, RutaCarpetaDestino, archivos_indexar);
            ResultadoProceso resultCopiar = replicador.CopiarCarpetaBaseADestino(CarpetaOrigen, CarpetaDestino);

            if (resultCopiar != null && resultCopiar.Resultado)
                NombreCarpetaReplicada = resultCopiar.ResultadoContenido;
                


            DatosInicialesDTO datosInicialesDTO = new DatosInicialesDTO()
                                                    {
                                                        CarpetaDestino = this.CarpetaDestino,
                                                        CarpetaOrigen = this.CarpetaOrigen,
                                                        NombreCarpetaReplicada = this.NombreCarpetaReplicada
                                                    };            


            VistaListaArchivosViewModel listaArchivoVM = new VistaListaArchivosViewModel(this, _navigationStore, datosInicialesDTO);
            _navigationStore.CurrentViewModel = listaArchivoVM;
        }

        private void SeleccionarCarpetaOrigen()
        {
            var path = _folderDialog.SeleccionarCarpeta();
            if (!string.IsNullOrWhiteSpace(path))
                CarpetaOrigen = path;
        }

        private void SeleccionarCarpetaDestino()
        {
            var path = _folderDialog.SeleccionarCarpeta();
            if (!string.IsNullOrWhiteSpace(path))
                CarpetaDestino = path;
        }


        public event EventHandler<DataErrorsChangedEventArgs> ErrorsChanged;
        public bool HasErrors => Errores.Any();

        public IEnumerable GetErrors(string propertyName)
        {
            if (Errores.ContainsKey(propertyName))
            {
                return Errores[propertyName];
            }
            else
            {
                return Enumerable.Empty<string>();
            }
        }

        public void Validar(string propiedad)
        {
            Errores.Remove(propiedad);

            var errores = new List<string>();

            switch (propiedad)
            {
                case nameof(CarpetaOrigen):
                    if (string.IsNullOrWhiteSpace(CarpetaOrigen))
                        errores.Add("Seleccione una carpeta.");
                    if (!Directory.Exists(CarpetaOrigen))
                        errores.Add("La carpeta no existe.");
                    if (CarpetaOrigen == CarpetaDestino)
                        errores.Add("Las carpetas deben ser diferentes.");
                    break;

                case nameof(CarpetaDestino):
                    if (string.IsNullOrWhiteSpace(CarpetaDestino))
                        errores.Add("Seleccione una carpeta.");
                    if (!Directory.Exists(CarpetaDestino))
                        errores.Add("La carpeta no existe.");
                    if (CarpetaDestino == CarpetaOrigen)
                        errores.Add("Las carpetas deben ser diferentes.");
                    break;

                case nameof(NombreCarpetaReplicada):
                    if (string.IsNullOrWhiteSpace(NombreCarpetaReplicada))
                        errores.Add("Carpeta replicada es requerido.");
                    break;
            }

            if(errores.Any())
                Errores[propiedad] = errores;

            ErrorsChanged?.Invoke(this, new DataErrorsChangedEventArgs(propiedad));
            OnPropertyChanged(nameof(HasErrors));
            OnPropertyChanged(GetErrorPropertyName(propiedad));
            CommandManager.InvalidateRequerySuggested(); // Actualiza estado de botones
            
        }

        private string GetErrorPropertyName(string propiedad)
        {
            string error = "";
            switch (propiedad)
            {
                case nameof(CarpetaOrigen):
                    error = nameof(CarpetaOrigenError);
                    break;
                case nameof(CarpetaDestino):
                    error = nameof(CarpetaDestinoError);
                    break;
                case nameof(NombreCarpetaReplicada):
                    error = nameof(NombreCarpetaReplicadaError);
                    break;
                default:
                    break;
            }

            return error;
            
        }
    }
}
