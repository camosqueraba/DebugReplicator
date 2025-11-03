using DebugReplicator.Controller;
using DebugReplicator.Controller.Utilities;
using DebugReplicator.Model;
using DebugReplicator.Model.DTOs;
using DebugReplicator.View.UIControls;
using Shell32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DebugReplicator.ViewModel
{
    public class VistaEditarArchivosConfigViewModel : BaseViewModel
    {

        public ICommand VolverCommand { get; }

        public ICommand ContinuarCommand { get; }
        public ICommand ReplicarCommand { get; }
        
        private readonly NavigationStore _NavigationStore;

        private readonly VistaIdexacionArchivosViewModel _VistaIndexacionArchivosViewModel;

        public ObservableCollection<IndexedFileControl> FileItemsIndexados { get; set; }

        public ObservableCollection<FileModel> ArchivosConfig { get; set; }

        public ObservableCollection<ClaveValorControl> PropiedadesArchivoConfig { get; set; }

        private string mensajeInfo;
        public string MensajeInfo
        {
            get => mensajeInfo;
            set { mensajeInfo = value; OnPropertyChanged(nameof(MensajeInfo)); }
        }

        private FileModel archivoConfigSeleccionado; // Replace MyItem with the actual type of your ComboBox items

        public FileModel ArchivoConfigSeleccionado
        {
            get { return archivoConfigSeleccionado; }
            set
            {
                if (archivoConfigSeleccionado != value)
                {
                    archivoConfigSeleccionado = value;
                    OnPropertyChanged(nameof(ArchivoConfigSeleccionado));
                    // Perform actions based on the new selection here
                    HandleSelectionChange();
                }
            }
        }

        private void HandleSelectionChange()
        {
            FileModel archivoConfigSeleccionado = this.ArchivoConfigSeleccionado;

            List<ClaveValorModel> claveValorModels = Replicador.LeerArchivoConfiguraciones(archivoConfigSeleccionado.Path);

            foreach (var item in claveValorModels)
            {
                ClaveValorControl claveValorControl = new ClaveValorControl(item);
                PropiedadesArchivoConfig.Add(claveValorControl);
            }
             
        }

        public VistaEditarArchivosConfigViewModel(VistaIdexacionArchivosViewModel vistaIndexacionArchivosViewModel, NavigationStore navigationStore)
        {
            _VistaIndexacionArchivosViewModel = vistaIndexacionArchivosViewModel;
            _NavigationStore = navigationStore;
            

            VolverCommand = new RelayCommand(Volver);
            ContinuarCommand = new RelayCommand(ContinuarConFileItemmsSeleccionados, ArchivosSeleccionadosTienenCaraterBandera);
            ReplicarCommand = new RelayCommand(ReplicarConFileItemmsSeleccionados, ArchivosSeleccionadosTienenCaraterBandera);

            FileItemsIndexados = vistaIndexacionArchivosViewModel.FileItemsIndexados;
            ArchivosConfig = ObtenerArchivosConfig(vistaIndexacionArchivosViewModel.FileItemsIndexados);
            PropiedadesArchivoConfig = new ObservableCollection<ClaveValorControl>();
        }

        private void Volver()
        {
            _NavigationStore.CurrentViewModel = _VistaIndexacionArchivosViewModel;
        }

        private bool ArchivosSeleccionadosTienenCaraterBandera()
        {
            //return FileItems.Any(f => f.File?.Seleccionado == true);
            return true;
        }

        private async void ContinuarConFileItemmsSeleccionados()
        {
            try
            {
                /*
                List<IndexedFileModel> indexedFiles = new List<IndexedFileModel>();

                MainWindowViewModel.GetInstance(_NavigationStore).Show();

                foreach (var item in FileItemsIndexados)
                {
                    indexedFiles.Add(item.IndexedFile);
                }

                await Task.Run(() =>
                {
                    string rutaCarpetaBase = _VistaIndexacionArchivosViewModel.DatosInicialesDTO.RutaCarpetaOrigen;
                    string rutaCarpetaDestino = _VistaIndexacionArchivosViewModel.DatosInicialesDTO.RutaCarpetaDestino;
                    string nombreCarpetaReplicada = _VistaIndexacionArchivosViewModel.DatosInicialesDTO.NombreCarpetaReplicada;
                    int numeroReplicas = _VistaIndexacionArchivosViewModel.DatosInicialesDTO.NumeroReplicas;
                });
                */
            }
            catch (Exception ex)
            {
                LOGRobotica.Controllers.LogApplication.LogWrite("VistaIdexacionArchivosViewModel -> ContinuarConFileItemmsSeleccionados: Exception " + ex.Message);
            }
            finally
            {
                MainWindowViewModel.GetInstance(_NavigationStore).HideLoading();
            }
        }

        private async void ReplicarConFileItemmsSeleccionados()
        {
            try
            {
                List<IndexedFileModel> indexedFiles = new List<IndexedFileModel>();

                MainWindowViewModel.GetInstance(_NavigationStore).ShowLoading();

                foreach (var item in FileItemsIndexados)
                {
                    indexedFiles.Add(item.IndexedFile);
                }

                await Task.Run(() =>
                {
                    /*
                    string rutaCarpetaBase = _VistaIndexacionArchivosViewModel.DatosInicialesDTO.RutaCarpetaOrigen;
                    string rutaCarpetaDestino = _VistaIndexacionArchivosViewModel.DatosInicialesDTO.RutaCarpetaDestino;
                    string rutaCarpetaBaseReplicada = _VistaIndexacionArchivosViewModel.DatosInicialesDTO.RutaCarpetaReplicada;
                    string nombreCarpetaReplicada = _VistaIndexacionArchivosViewModel.DatosInicialesDTO.NombreCarpetaReplicada;
                    int numeroReplicas = _VistaIndexacionArchivosViewModel.DatosInicialesDTO.NumeroReplicas;

                    ResultadoProceso resultadoProceso = Replicador.ReplicarDebug(rutaCarpetaBase, rutaCarpetaDestino, nombreCarpetaReplicada, numeroReplicas, indexedFiles);

                    if (resultadoProceso != null && !resultadoProceso.Completado)
                    {
                        MensajeInfo = resultadoProceso.Errores[0] + resultadoProceso.ResultadoContenido;
                    }
                    */
                });
            }
            catch (Exception ex)
            {
                LOGRobotica.Controllers.LogApplication.LogWrite("VistaIdexacionArchivosViewModel -> ReplicarConFileItemmsSeleccionados: Exception " + ex.Message);
            }
            finally
            {
                MainWindowViewModel.GetInstance(_NavigationStore).HideLoading();
            }
        }


        private ObservableCollection<FileModel> ObtenerArchivosConfig(ObservableCollection<IndexedFileControl> fileItemsSeleccionados)
        {
            ObservableCollection<FileModel> archivosConfig = new ObservableCollection<FileModel>();

            foreach (IndexedFileControl fileItem in fileItemsSeleccionados)
            {
                if(GestorCarpetasArchivos.CompruebaTipoArchivoPorExtension(fileItem.IndexedFile.Path, ".config") ||
                    GestorCarpetasArchivos.CompruebaTipoArchivoPorExtension(fileItem.IndexedFile.Path, ".json"))
                {
                    archivosConfig.Add(fileItem.IndexedFile);
                }               
            }

            return archivosConfig;
        }
    }
}

