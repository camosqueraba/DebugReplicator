using DebugReplicator.ViewModel;
using System.Windows;
using System.Windows.Controls;

namespace DebugReplicator.View
{
    /// <summary>
    /// Lógica de interacción para VistaListaArchivos.xaml
    /// </summary>
    public partial class VistaListaArchivos : UserControl
    {
        private MainWindow VentanaPrincipal = (MainWindow)Application.Current.MainWindow;
        public static string RutaCarpetaBase {  get; set; } 
        public VistaListaArchivosViewModel ListaArchivosViewModel
        {
            get => this.DataContext as VistaListaArchivosViewModel;
            set => this.DataContext = value;
        }

        public VistaListaArchivos(string rutaCarpetaBase)
        {
            InitializeComponent();

            RutaCarpetaBase = rutaCarpetaBase;
            ListaArchivosViewModel.TryNavigateToPath(rutaCarpetaBase);
        }

        private void ButtonDirectorioAnterior_Click(object sender, RoutedEventArgs e)
        {
            ListaArchivosViewModel.TryNavigateToPath(RutaCarpetaBase);
        }

        private void ButtonVistaAnterior_Click(object sender, RoutedEventArgs e)
        {
            VentanaPrincipal.GridContenidoPrincipal.Children.Clear();
            VentanaPrincipal.GridContenidoPrincipal.Children.Add(new VistaPrincipal());
        }

        private void ButtonContinuar_Click(object sender, RoutedEventArgs e)
        {
            var algo = ListaArchivosViewModel.FileItems;

            foreach (var item in algo)
            {
                var algo_ = item.File;
            }
        }
    }
}
