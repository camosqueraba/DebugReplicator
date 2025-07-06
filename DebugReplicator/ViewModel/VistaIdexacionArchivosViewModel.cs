using DebugReplicator.Controller;
using DebugReplicator.Model;
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

        private readonly NavigationStore _NavigationStore;

        private readonly VistaListaArchivosViewModel _VistaListaArchivosViewModel;

        public ObservableCollection<IndexedFileControl> FileItemsIndexados { get; set; }

        public VistaIdexacionArchivosViewModel(VistaListaArchivosViewModel vistaListaArchivosViewModel, NavigationStore navigationStore)
        {
            _VistaListaArchivosViewModel = vistaListaArchivosViewModel;
            _NavigationStore = navigationStore;
            FileItemsIndexados = CrearSelectedFileControls(vistaListaArchivosViewModel.FileItemsSeleccionados);

            VolverCommand = new RelayCommand(Volver);
            ContinuarCommand = new RelayCommand(ContinuarConFileItemmsSeleccionados, HayArchivosSeleccionados);
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

        private void ContinuarConFileItemmsSeleccionados()
        {
            /*
            FileItemsSeleccionados.Clear();

            foreach (var archivo in TotalFileItems)
            {
                if (archivo.File.Seleccionado)
                    FileItemsSeleccionados.Add(archivo);
            }
            */
        }

        private ObservableCollection<IndexedFileControl> CrearSelectedFileControls(ObservableCollection<FilesControl> fileItemsSeleccionados)
        {
            var fileItemsIndexados = new ObservableCollection<IndexedFileControl>();
            
            foreach (var fileItem in fileItemsSeleccionados)
            {
                IndexedFileModel indexedFileModel = new IndexedFileModel();

                indexedFileModel.Icon = fileItem.File.Icon;
                indexedFileModel.Name = fileItem.File.Name;
                indexedFileModel.Path = fileItem.File.Path;

                IndexedFileControl fileControl = new IndexedFileControl(indexedFileModel);
                fileItemsIndexados.Add(fileControl);
            }

            return fileItemsIndexados;
        }
    }

}
