using DebugReplicator.Controller;
using DebugReplicator.Explorer;
using DebugReplicator.Model;
using DebugReplicator.Model.DTOs;
using DebugReplicator.View.UIControls;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace DebugReplicator.ViewModel
{
    public class VistaListaArchivosViewModel : BaseViewModel
    {
        /*
         public string Campo1 { get; }
        public string Campo2 { get; }
        public string Campo3 { get; }
        public string Carpeta { get; }
        */

        public ICommand VolverCommand { get; }
        private readonly NavigationStore _navigationStore;

        private readonly VistaPrincipalViewModel _VistaPrincipalViewModel;         

        public ObservableCollection<FilesControl> FileItems { get; set; }

        public VistaListaArchivosViewModel(VistaPrincipalViewModel vistaPrincipalViewModel, NavigationStore navigationStore, DatosInicialesDTO datosInicialesDTO)
        {

            _VistaPrincipalViewModel = vistaPrincipalViewModel;
            _navigationStore = navigationStore;  

            FileItems = new ObservableCollection<FilesControl>();

            this.TryNavigateToPath(datosInicialesDTO.NombreCarpetaReplicada);

            VolverCommand = new RelayCommand(Volver);
        }

        private void Volver()
        {  
            _navigationStore.CurrentViewModel = _VistaPrincipalViewModel;
        }

        #region Navigation

        public void TryNavigateToPath(string path)
        {
            // is a drive
            if (path == string.Empty)
            {
                ClearFiles();

                foreach(FileModel drive in ExploradorDirectorios.GetDrives())
                {
                    FilesControl fc = CreateFileControl(drive);
                    AddFile(fc);
                }
            }

            else if (path.IsFile())
            {
                // Open the file
                MessageBox.Show($"Opening {path}");
            }

            else if (path.IsDirectory())
            {
                ClearFiles();
                /*
                foreach(FileModel dir in ExploradorDirectorios.GetDirectories(path))
                {
                    FilesControl fc = CreateFileControl(dir);
                    AddFile(fc);
                }

                
                foreach (FileModel file in ExploradorDirectorios.GetFiles(path))
                {
                    FilesControl fc = CreateFileControl(file);
                    AddFile(fc);
                }
                */

                foreach (FileModel file in ExploradorDirectorios.ObtenerContenidoCarpeta(path))
                {
                    FilesControl fc = CreateFileControl(file);
                    AddFile(fc);
                }
            }

            else
            {
                // something bad has happened...
            }
        }

        public void NavigateFromModel(FileModel file)
        {
            TryNavigateToPath(file.Path);
        }

        #endregion

        public void AddFile(FilesControl file)
        {
            FileItems.Add(file);
        }

        public void RemoveFile(FilesControl file)
        {
            FileItems.Remove(file);
        }

        public void ClearFiles()
        {
            FileItems.Clear();
        }

        public FilesControl CreateFileControl(FileModel fModel)
        {
            FilesControl fc = new FilesControl(fModel);
            SetupFileControlCallbacks(fc);
            return fc;
        }

        public void SetupFileControlCallbacks(FilesControl fc)
        {
            fc.NavigateToPathCallback = NavigateFromModel;
        }
    }
}
