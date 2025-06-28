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
        public ObservableCollection<FilesControl> SelectedFileItems { get; set; }

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

            FileItems         = new ObservableCollection<FilesControl>();
            SelectedFileItems = new ObservableCollection<FilesControl>();

            this.TryNavigateToPath(datosInicialesDTO.NombreCarpetaReplicada);
            
            RutasVisitadas = new Stack<string>();
            RutasVisitadas.Push(datosInicialesDTO.NombreCarpetaReplicada);
            ActualRuta = datosInicialesDTO.NombreCarpetaReplicada;

            VolverCommand = new RelayCommand(Volver);
            ContinuarCommand = new RelayCommand(ContinuarConArchivosIndexados, HayArchivosSeleccionados);
            DirectorioAnteriorCommand = new RelayCommand(IrADirectorioAnterior);
        }

        private bool HayArchivosSeleccionados()
        {
            return FileItems.Any(f => f.File?.Seleccionado == true);
        }

        private void ContinuarConArchivosIndexados()
        {
            SelectedFileItems.Clear();

            foreach (var archivo in FileItems)
            {
                if(archivo.File.Seleccionado)
                    SelectedFileItems.Add(archivo);
            }            
        }

        private void IrADirectorioAnterior()
        {
            string ultimaRutaVisitada = ExtraerUltimaRuta(); 
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
    }
}
