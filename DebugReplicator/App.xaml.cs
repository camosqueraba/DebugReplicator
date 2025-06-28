using DebugReplicator.Controller;
using DebugReplicator.Controller.Services;
using DebugReplicator.Model.DTOs;
using DebugReplicator.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace DebugReplicator
{
    /// <summary>
    /// Lógica de interacción para App.xaml
    /// </summary>
    public partial class App : Application
    {
        private readonly NavigationStore _navigationStore = new NavigationStore() ;
        private readonly IFolderDialogService _folderDialogService = new FolderDialogService();

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var formularioVM = new VistaPrincipalViewModel(_navigationStore, _folderDialogService);
            _navigationStore.CurrentViewModel = formularioVM;

            
            DatosInicialesDTO dto = new DatosInicialesDTO() { CarpetaDestino="",
            CarpetaOrigen="",
            NombreCarpetaReplicada= @"C:\Users\camos\Downloads\Pruebas_DebugReplicator\CarpetaDestino\Debug",
            };
            

            var vistaListaArchivosViewModel = new VistaListaArchivosViewModel(null, null, dto);
            _navigationStore.CurrentViewModel = vistaListaArchivosViewModel;
            

            var mainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel(_navigationStore)
            };

            mainWindow.Show();
        }
    }
}
