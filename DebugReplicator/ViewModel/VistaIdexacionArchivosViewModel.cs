using DebugReplicator.Controller;
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
    public class VistaIdexacionArchivosViewModel : BaseViewModel
    {
        public ICommand VolverCommand { get; }
        
        public ICommand ContinuarCommand { get; }
        public ICommand ReplicarCommand { get; }

        private readonly NavigationStore _NavigationStore;

        private readonly VistaListaArchivosViewModel _VistaListaArchivosViewModel;

        public ObservableCollection<IndexedFileControl> FileItemsIndexados { get; set; }

        private bool isVisibleLoading;
        public bool IsVisibleLoading
        {
            get => isVisibleLoading;
            set { isVisibleLoading = value; OnPropertyChanged(nameof(IsVisibleLoading)); }
        }

        private string mensajeInfo;
        public string MensajeInfo
        {
            get => mensajeInfo;
            set { mensajeInfo = value; OnPropertyChanged(nameof(MensajeInfo)); }
        }

        public VistaIdexacionArchivosViewModel(VistaListaArchivosViewModel vistaListaArchivosViewModel, NavigationStore navigationStore)
        {
            _VistaListaArchivosViewModel = vistaListaArchivosViewModel;
            _NavigationStore = navigationStore;
            FileItemsIndexados = CrearSelectedFileControls(vistaListaArchivosViewModel.FileItemsSeleccionados);

            VolverCommand = new RelayCommand(Volver);
            ContinuarCommand = new RelayCommand(ContinuarConFileItemmsSeleccionados, HayArchivosSeleccionados);
            ReplicarCommand = new RelayCommand(ReplicarConFileItemmsSeleccionados, HayArchivosSeleccionados);

            IsVisibleLoading = false;
        }

        private void Volver()
        {
            _NavigationStore.CurrentViewModel = _VistaListaArchivosViewModel;
        }

        private bool HayArchivosSeleccionados()
        {
            //return FileItems.Any(f => f.File?.Seleccionado == true);
            return true;
        }

        private async void ContinuarConFileItemmsSeleccionados()
        {
            try
            {
                List<IndexedFileModel> indexedFiles = new List<IndexedFileModel>();
                IsVisibleLoading = true;

                foreach (var item in FileItemsIndexados)
                {
                    indexedFiles.Add(item.IndexedFile);
                }

                await Task.Run(() =>
                {
                    string rutaCarpetaBase = _VistaListaArchivosViewModel.DatosInicialesDTO.RutaCarpetaOrigen;
                    string rutaCarpetaDestino = _VistaListaArchivosViewModel.DatosInicialesDTO.RutaCarpetaDestino;
                    string nombreCarpetaReplicada = _VistaListaArchivosViewModel.DatosInicialesDTO.NombreCarpetaReplicada;
                    int numeroReplicas = _VistaListaArchivosViewModel.DatosInicialesDTO.NumeroReplicas;                    
                });
            }
            catch (Exception ex)
            {
                LOGRobotica.Controllers.LogApplication.LogWrite("VistaIdexacionArchivosViewModel -> ContinuarConFileItemmsSeleccionados: Exception " + ex.Message);
            }
            finally
            {
                IsVisibleLoading = false;
            }
        }

        private async void ReplicarConFileItemmsSeleccionados()
        {
            try
            {
                List<IndexedFileModel> indexedFiles = new List<IndexedFileModel>();
                IsVisibleLoading = true;

                foreach (var item in FileItemsIndexados)
                {
                    indexedFiles.Add(item.IndexedFile);
                }

                await Task.Run(() =>
                {
                    string rutaCarpetaBase = _VistaListaArchivosViewModel.DatosInicialesDTO.RutaCarpetaOrigen;
                    string rutaCarpetaDestino = _VistaListaArchivosViewModel.DatosInicialesDTO.RutaCarpetaDestino;
                    string rutaCarpetaBaseReplicada = _VistaListaArchivosViewModel.DatosInicialesDTO.RutaCarpetaReplicada;
                    string nombreCarpetaReplicada = _VistaListaArchivosViewModel.DatosInicialesDTO.NombreCarpetaReplicada;
                    int numeroReplicas = _VistaListaArchivosViewModel.DatosInicialesDTO.NumeroReplicas;
                    

                    ResultadoProceso resultadoProceso = Replicador.ReplicarDebug(rutaCarpetaBase, rutaCarpetaDestino, nombreCarpetaReplicada, numeroReplicas, indexedFiles);

                    if (resultadoProceso != null && !resultadoProceso.Completado)
                    {
                        MensajeInfo = resultadoProceso.Errores[0] + resultadoProceso.ResultadoContenido;
                    }
                });
            }
            catch (Exception ex)
            {
                LOGRobotica.Controllers.LogApplication.LogWrite("VistaIdexacionArchivosViewModel -> ReplicarConFileItemmsSeleccionados: Exception " + ex.Message);
            }
            finally
            {
                IsVisibleLoading = false;
            }
        }


        private ObservableCollection<IndexedFileControl> CrearSelectedFileControls(ObservableCollection<FilesControl> fileItemsSeleccionados)
        {
            var fileItemsIndexados = new ObservableCollection<IndexedFileControl>();
            
            foreach (var fileItem in fileItemsSeleccionados)
            {
                IndexedFileModel indexedFileModel = new IndexedFileModel();

                indexedFileModel.Icon           = fileItem.File.Icon;
                indexedFileModel.Name           = fileItem.File.Name;
                indexedFileModel.Path           = fileItem.File.Path;
                indexedFileModel.NombreIndexado = fileItem.File.Name;
                
                IndexedFileControl fileControl = new IndexedFileControl(indexedFileModel);

                fileItemsIndexados.Add(fileControl);
            }

            return fileItemsIndexados;
        }        
    }
}
