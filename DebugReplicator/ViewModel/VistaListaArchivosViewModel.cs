using DebugReplicator.Controller;
using DebugReplicator.Explorer;
using DebugReplicator.Model;
using DebugReplicator.Model.DTOs;
using DebugReplicator.View.UIControls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace DebugReplicator.ViewModel
{
    public class VistaListaArchivosViewModel : BaseViewModel
    {
        public ICommand VolverCommand { get; }
        public ICommand DirectorioAnteriorCommand { get; }
        public ICommand ContinuarCommand { get; }

        private readonly NavigationStore _navigationStore;

        private readonly VistaPrincipalViewModel _VistaPrincipalViewModel;

        public ObservableCollection<FilesControl> FileItems { get; set; }
        public ObservableCollection<FilesControl> TotalFileItems { get; set; }
        public ObservableCollection<FilesControl> FileItemsSeleccionados { get; set; }

        public  Stack<string> RutasVisitadas { get; set; }

        private string actualRuta;
        public string ActualRuta
        {
            get => actualRuta;
            set { actualRuta = value; OnPropertyChanged(nameof(ActualRuta)); }
        }
        public VistaListaArchivosViewModel(VistaPrincipalViewModel vistaPrincipalViewModel, NavigationStore navigationStore, DatosInicialesDTO datosInicialesDTO)
        {

            _VistaPrincipalViewModel = vistaPrincipalViewModel;
            _navigationStore         = navigationStore;  

            FileItems               = new ObservableCollection<FilesControl>();
            FileItemsSeleccionados  = new ObservableCollection<FilesControl>();
            TotalFileItems          = new ObservableCollection<FilesControl>();

            this.TryNavigateToPath(datosInicialesDTO.NombreCarpetaReplicada);
            
            RutasVisitadas = new Stack<string>();
            RutasVisitadas.Push(datosInicialesDTO.NombreCarpetaReplicada);
            ActualRuta = datosInicialesDTO.NombreCarpetaReplicada;

            VolverCommand = new RelayCommand(Volver);
            ContinuarCommand = new RelayCommand(ContinuarConFileItemmsSeleccionados, HayArchivosSeleccionados);
            DirectorioAnteriorCommand = new RelayCommand(IrADirectorioAnterior);
        }

        private bool HayArchivosSeleccionados()
        {
            return TotalFileItems.Any(f => f.File?.Seleccionado == true);
        }

        private void ContinuarConFileItemmsSeleccionados()
        {
            FileItemsSeleccionados.Clear();

            foreach (var archivo in TotalFileItems)
            {
                if(archivo.File.Seleccionado)
                    FileItemsSeleccionados.Add(archivo);
            }

            VistaIdexacionArchivosViewModel vistaIdexacionArchivosViewModel = new VistaIdexacionArchivosViewModel(this, _navigationStore);
            _navigationStore.CurrentViewModel = vistaIdexacionArchivosViewModel;
        }

        private void IrADirectorioAnterior()
        {
            string ultimaRutaVisitada = ActualRuta = ExtraerUltimaRuta(); 
            TryNavigateToPath(ultimaRutaVisitada);

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

                    if(fc != null && !FileItemYaAgregadoAlTotal(fc))
                        TotalFileItems.Add(fc);
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
            RutasVisitadas.Push(file.Path);
            ActualRuta = file.Path;
        }

        #endregion

        public void AddFile(FilesControl file)
        {
            //if(file.Name)
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

        public string ExtraerUltimaRuta()
        {
            string ultimaRutaVisitada = string.Empty;
            int numeroDirectorios = RutasVisitadas.Count;

            if (numeroDirectorios > 2)
            {
                RutasVisitadas.Pop();
                ultimaRutaVisitada = RutasVisitadas.Pop();
            }
            else if (numeroDirectorios == 2)
            {
                RutasVisitadas.Pop();
                ultimaRutaVisitada = RutasVisitadas.Peek();
            }
            else if(numeroDirectorios == 1)
            {
                ultimaRutaVisitada = RutasVisitadas.Peek();
            }
            else
            {
                ultimaRutaVisitada = "";
            }

            return ultimaRutaVisitada;
        }

        private bool FileItemYaAgregadoAlTotal(FilesControl file)
        {
            bool fileItemAgregado = false;

            string ubicacion    = file.File.Path;
            string name         = file.File.Name;
            bool   seleccionado = file.File.Seleccionado;

            foreach (var fileItem in TotalFileItems)
            {
                string ubicacionfileItem    = fileItem.File.Path;
                string namefileItem         = fileItem.File.Name;
                bool seleccionadofileItem   = fileItem.File.Seleccionado;

                if (ubicacion == ubicacionfileItem && name == namefileItem && seleccionado == seleccionadofileItem)
                {
                    fileItemAgregado = true;
                    break;
                }
                else if (ubicacion == ubicacionfileItem && name == namefileItem && seleccionado != seleccionadofileItem)
                {
                    fileItem.File.Seleccionado = seleccionado;
                    fileItemAgregado = true;
                }
                else
                    fileItemAgregado = false;
            }

            return fileItemAgregado;
        }
    }
}
