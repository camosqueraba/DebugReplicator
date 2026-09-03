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

        public ObservableCollection<ArchivoConfigModel> ArchivosConfig { get; set; }

        public ObservableCollection<ClaveValorControl> PropiedadesArchivoConfig { get; set; }
        public DatosInicialesDTO DatosInicialesDTO { get; set; }

        private string mensajeInfo;
        public string MensajeInfo
        {
            get => mensajeInfo;
            set { mensajeInfo = value; OnPropertyChanged(nameof(MensajeInfo)); }
        }

        private ArchivoConfigModel archivoConfigSeleccionado;

        public ArchivoConfigModel ArchivoConfigSeleccionado
        {
            get { return archivoConfigSeleccionado; }
            set
            {
                if (archivoConfigSeleccionado != value)
                {
                    archivoConfigSeleccionado = value;
                    OnPropertyChanged(nameof(ArchivoConfigSeleccionado));
                    // Perform actions based on the new selection here
                    ComboBoxArchivosConfigSelectionChange();
                }
            }
        }

        private void ComboBoxArchivosConfigSelectionChange()
        {
            FileModel archivoConfigSeleccionado = this.ArchivoConfigSeleccionado;

            List<ClaveValorModel> claveValorModels = Replicador.LeerArchivoConfiguraciones(archivoConfigSeleccionado.Path);

            foreach (var item in claveValorModels)
            {
                ClaveValorControl claveValorControl = new ClaveValorControl(item);
                PropiedadesArchivoConfig.Add(claveValorControl);
            }

            ArchivoConfigSeleccionado.PropiedadesArchivoConfig = PropiedadesArchivoConfig;
             
        }

        public VistaEditarArchivosConfigViewModel(VistaIdexacionArchivosViewModel vistaIndexacionArchivosViewModel, NavigationStore navigationStore, DatosInicialesDTO datosInicialesDTO)
        {
            _VistaIndexacionArchivosViewModel = vistaIndexacionArchivosViewModel;
            _NavigationStore = navigationStore;
            
            DatosInicialesDTO = datosInicialesDTO;

            VolverCommand = new RelayCommand(Volver);
            //ContinuarCommand = new RelayCommand(ContinuarConFileItemmsSeleccionados, ArchivosSeleccionadosTienenCaraterBandera);
            ReplicarCommand = new RelayCommand(ReplicarConArchivosConfigEditados, ArchivosSeleccionadosTienenCaracterBandera);

            FileItemsIndexados = vistaIndexacionArchivosViewModel.FileItemsIndexados;
            ArchivosConfig = ObtenerArchivosConfig(vistaIndexacionArchivosViewModel.FileItemsIndexados);
            PropiedadesArchivoConfig = new ObservableCollection<ClaveValorControl>();
        }

        private void Volver()
        {
            _NavigationStore.CurrentViewModel = _VistaIndexacionArchivosViewModel;
        }

        private bool ArchivosSeleccionadosTienenCaracterBandera()
        {
            //return FileItems.Any(f => f.File?.Seleccionado == true);
            return true;
        }
       

        private async void ReplicarConArchivosConfigEditados()
        {
            try
            {
                List<IndexedFileModel> indexedFiles = new List<IndexedFileModel>();

                MainWindowViewModel.GetInstance(_NavigationStore).ShowLoading();

                foreach (var item in FileItemsIndexados)
                {
                    foreach (var archivoConfig in ArchivosConfig)
                    {
                        if(item.IndexedFile.Path == archivoConfig.Path)
                        {
                            item.IndexedFile.EsArchivoConfig = true;

                            
                            List<ClaveValorModel> listaPropiedades = ExtraerPropiedadesClaveValor(archivoConfig.PropiedadesArchivoConfig);
                            
                            item.IndexedFile.PropiedadesArchivoConfig = listaPropiedades;
                        }
                    }

                    indexedFiles.Add(item.IndexedFile);
                }

                await Task.Run(() =>
                {
                    
                    string rutaCarpetaBase          = DatosInicialesDTO.RutaCarpetaOrigen;
                    string rutaCarpetaDestino       = DatosInicialesDTO.RutaCarpetaDestino;
                    string rutaCarpetaBaseReplicada = DatosInicialesDTO.RutaCarpetaReplicada;
                    string nombreCarpetaReplicada   = DatosInicialesDTO.NombreCarpetaReplicada;
                    int rangoFin                    = DatosInicialesDTO.RangoFin;
                    int rangoInicio                 = DatosInicialesDTO.RangoInicio;

                    ResultadoProceso resultadoProceso = Replicador.ReplicarDebug(rutaCarpetaBase, rutaCarpetaDestino, nombreCarpetaReplicada, rangoFin, rangoInicio,indexedFiles);

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
                MainWindowViewModel.GetInstance(_NavigationStore).HideLoading();
            }
        }

        private List<ClaveValorModel> ExtraerPropiedadesClaveValor(ObservableCollection<ClaveValorControl> propiedadesArchivoConfig)
        {
            try
            {

                List<ClaveValorModel> listaPropiedades = new List<ClaveValorModel>();

                foreach (var item in propiedadesArchivoConfig)
                {
                    string clave = item.ClaveValor.Clave;
                    string valor = item.ClaveValor.Valor;

                    listaPropiedades.Add(new ClaveValorModel()
                    {
                        Clave = clave,
                        Valor = valor
                    });
                }

                return listaPropiedades;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        private ObservableCollection<ArchivoConfigModel> ObtenerArchivosConfig(ObservableCollection<IndexedFileControl> fileItemsSeleccionados)
        {
            ObservableCollection<ArchivoConfigModel> archivosConfig = new ObservableCollection<ArchivoConfigModel>();

            foreach (IndexedFileControl fileItem in fileItemsSeleccionados)
            {
                if(GestorCarpetasArchivos.CompruebaTipoArchivoPorExtension(fileItem.IndexedFile.Path, ".config") ||
                    GestorCarpetasArchivos.CompruebaTipoArchivoPorExtension(fileItem.IndexedFile.Path, ".json"))
                {
                    ArchivoConfigModel archivoConfig = new ArchivoConfigModel()
                    {
                        Name = fileItem.IndexedFile.Name,
                        Path = fileItem.IndexedFile.Path,
                        PropiedadesArchivoConfig = new ObservableCollection<ClaveValorControl>()

                    };

                    archivosConfig.Add(archivoConfig);
                }               
            }

            return archivosConfig;
        }
    }
}

