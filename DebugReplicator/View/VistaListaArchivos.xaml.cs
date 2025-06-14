using DebugReplicator.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DebugReplicator.View
{
    /// <summary>
    /// Lógica de interacción para VistaListaArchivos.xaml
    /// </summary>
    public partial class VistaListaArchivos : UserControl
    {
        private MainWindow VentanaPrincipal = (MainWindow)Application.Current.MainWindow;
        public static string RutaCarpetaBase {  get; set; } 
        public VistaListaArchivosViewModel Model
        {
            get => this.DataContext as VistaListaArchivosViewModel;
            set => this.DataContext = value;
        }

        public VistaListaArchivos(string rutaCarpetaBase)
        {
            InitializeComponent();

            RutaCarpetaBase = rutaCarpetaBase;
            Model.TryNavigateToPath(rutaCarpetaBase);
        }

        private void ButtonDirectorioAnterior_Click(object sender, RoutedEventArgs e)
        {
            Model.TryNavigateToPath(RutaCarpetaBase);
        }

        private void ButtonVistaAnterior_Click(object sender, RoutedEventArgs e)
        {
            VentanaPrincipal.GridContenidoPrincipal.Children.Clear();
            VentanaPrincipal.GridContenidoPrincipal.Children.Add(new VistaPrincipal());
        }
    }
}
