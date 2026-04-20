using DebugReplicator.Controller;
using DebugReplicator.Controller.Services;
using DebugReplicator.Controller.Utilities;
using DebugReplicator.Model.DTOs;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
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
                    Validar(nameof(CarpetaOrigen));
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

        private string rangoFin;
        public string RangoFin
        {
            get => rangoFin;
            set
            {
                if (SetProperty(ref rangoFin, value))
                {
                    Validar(nameof(RangoFin));
                    Validar(nameof(RangoInicio));
                }
                    
            }
        }

        private int RangoFinInt { get; set; }

        private string rangoInicio;
        public string RangoInicio
        {
            get => rangoInicio;
            set
            {
                if (SetProperty(ref rangoInicio, value)) { 
                    Validar(nameof(RangoInicio));
                    Validar(nameof(RangoFin));
                }
            }
        }

        private int RangoInicioInt { get; set; }

        public string CarpetaOrigenError => GetFirstError(nameof(CarpetaOrigen));
        public string CarpetaDestinoError => GetFirstError(nameof(CarpetaDestino));
        public string NombreCarpetaReplicadaError => GetFirstError(nameof(NombreCarpetaReplicada));
        public string RangoError => GetFirstError(nameof(RangoFin));

        public ICommand SiguienteCommand { get; }
        public ICommand ReplicarCommand { get; }
        public ICommand SeleccionarCarpetaOrigenCommand { get; }
        public ICommand SeleccionarCarpetaDestinoCommand { get; }     


        Dictionary<string, List<string>> Errores = new Dictionary<string, List<string>>();

        public VistaPrincipalViewModel(NavigationStore navigationStore, IFolderDialogService folderDialog)
        {
            _navigationStore = navigationStore;
            _folderDialog = folderDialog;

            SiguienteCommand = new RelayCommand(Siguiente, () => !HasErrors);
            ReplicarCommand = new RelayCommand(Replicar, () => !HasErrors);

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

            if (string.IsNullOrWhiteSpace(RangoFin))
                RangoFin = "";

            if (string.IsNullOrWhiteSpace(RangoInicio))
                RangoInicio = "";

            Validar(CarpetaOrigen);
            Validar(CarpetaDestino);
            Validar(NombreCarpetaReplicada);
            Validar(RangoFin);
            Validar(RangoInicio);

            if (HasErrors)
            {
                MessageBox.Show("Corrige los errores antes de continuar.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            ResultadoProceso resultCopiar = Replicador.CopiarCarpetaBaseADestino(CarpetaOrigen, CarpetaDestino, NombreCarpetaReplicada);

            if (resultCopiar != null && resultCopiar.Completado)
            {
                DatosInicialesDTO datosInicialesDTO = new DatosInicialesDTO()
                {
                    RutaCarpetaDestino = this.CarpetaDestino,
                    RutaCarpetaOrigen = this.CarpetaOrigen,
                    RutaCarpetaReplicada = resultCopiar.ResultadoContenido,
                    NombreCarpetaReplicada = this.NombreCarpetaReplicada,
                    RangoFin = this.RangoFinInt,
                    RangoInicio = this.RangoInicioInt
                };

                VistaListaArchivosViewModel listaArchivoVM = new VistaListaArchivosViewModel(this, _navigationStore, datosInicialesDTO);
                _navigationStore.CurrentViewModel = listaArchivoVM;
            }
            else
            {
                MessageBox.Show("Error al copiar la carpeta origen a destino.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Replicar ()
        {
            if (string.IsNullOrWhiteSpace(CarpetaOrigen))
                CarpetaOrigen = "";

            if (string.IsNullOrWhiteSpace(CarpetaDestino))
                CarpetaDestino = "";

            if (string.IsNullOrWhiteSpace(NombreCarpetaReplicada))
                NombreCarpetaReplicada = "";

            if (string.IsNullOrWhiteSpace(RangoFin))
                RangoFin = "";

            Validar(CarpetaOrigen);
            Validar(CarpetaDestino);
            Validar(NombreCarpetaReplicada);
            Validar(RangoFin);

            if (HasErrors)
            {
                MessageBox.Show("Corrige los errores antes de continuar.", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            
            Replicador replicador = new Replicador();

            Replicador.ReplicarDebug(CarpetaOrigen, CarpetaDestino, NombreCarpetaReplicada, RangoFinInt, RangoInicioInt);
            //Mostrar mensaje de éxito aqui
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
                    if (!string.IsNullOrWhiteSpace(CarpetaOrigen) && Directory.Exists(CarpetaOrigen))
                        NombreCarpetaReplicada = SetNombreCarpetaBase(CarpetaOrigen);
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

                case nameof(RangoFin):
                    if (string.IsNullOrWhiteSpace(RangoFin))
                        errores.Add("Rango fin es requerido.");
                    
                    if (!int.TryParse(RangoFin, out int rangoFin) || rangoFin < 1)                    
                        errores.Add("Rango fin debe ser un número entero mayor a 0.");
                   
                    if (rangoFin < RangoInicioInt)                    
                        errores.Add("Rango fin debe ser un número entero mayor rango inicio.");
                    
                    RangoFinInt = rangoFin;

                    break;

                case nameof(RangoInicio):
                    if (string.IsNullOrWhiteSpace(RangoInicio))
                        errores.Add("Rango inicio es requerido.");

                    if (!int.TryParse(RangoInicio, out int rangoInicio) || rangoInicio < 1)                    
                        errores.Add("Rango inicio debe ser un número entero mayor a 0.");
                    
                    if (rangoInicio > RangoFinInt)                    
                        errores.Add("Rango inicio debe ser un número entero menor  al rango fin.");
                                        
                    RangoInicioInt = rangoInicio;
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
                case nameof(RangoFin):
                    error = nameof(RangoError);
                    break;

                case nameof(RangoInicio):
                    error = nameof(RangoError);
                    break;
                default:
                    break;
            }

            return error;            
        }

        private string SetNombreCarpetaBase(string RutaCarpetaOrigen)
        {
            string nombreCarpetaBase = "";

            nombreCarpetaBase = GestorCarpetasArchivos.ObtenerNombreCarpeta(RutaCarpetaOrigen);
            
            return nombreCarpetaBase;
        }
    }
}
