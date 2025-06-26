using DebugReplicator.Controller;
using DebugReplicator.Controller.Services;
using DebugReplicator.Model.DTOs;
using DebugReplicator.View;
using System.Windows.Input;


namespace DebugReplicator.ViewModel
{
    public class VistaPrincipalViewModel: BaseViewModel
    {
        private readonly NavigationStore _navigationStore;
        private readonly IFolderDialogService _folderDialog;
        
        private string carpetaOrigen;
        public string CarpetaOrigen
        {
            get => carpetaOrigen;
            set { carpetaOrigen = value; OnPropertyChanged(); }
        }

        private string carpetaDestino;
        public string CarpetaDestino
        {
            get => carpetaDestino;
            set { carpetaDestino = value; OnPropertyChanged(); }
        }

        private string nombreCarpetaReplicada;
        public string NombreCarpetaReplicada
        {
            get => nombreCarpetaReplicada;
            set { nombreCarpetaReplicada = value; OnPropertyChanged(); }
        }

        public ICommand SiguienteCommand { get; }
        public ICommand SeleccionarCarpetaOrigenCommand { get; }
        public ICommand SeleccionarCarpetaDestinoCommand { get; }
        public ICommand SeleccionarCarpetaReplicadaCommand { get; }

        public VistaPrincipalViewModel(NavigationStore navigationStore, IFolderDialogService folderDialog)
        {
            _navigationStore = navigationStore;
            _folderDialog = folderDialog;

            SiguienteCommand = new RelayCommand(Siguiente);
            SeleccionarCarpetaOrigenCommand = new RelayCommand(SeleccionarCarpetaOrigen);
            SeleccionarCarpetaDestinoCommand = new RelayCommand(SeleccionarCarpetaDestino);
            
        }

        private void Siguiente()
        {
            /*
            if (string.IsNullOrWhiteSpace(Campo1) ||
                string.IsNullOrWhiteSpace(Campo2) ||
                string.IsNullOrWhiteSpace(Campo3) ||
                string.IsNullOrWhiteSpace(CarpetaSeleccionada) ||
                !Directory.Exists(Campo3) ||
                !Directory.Exists(CarpetaSeleccionada))
            {
                MessageBox.Show("Complete todos los campos y seleccione carpetas válidas.");
                return;
            }
            */

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

        

    }
}
