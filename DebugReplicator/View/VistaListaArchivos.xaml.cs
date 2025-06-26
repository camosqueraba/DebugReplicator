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

        public VistaListaArchivos()
        {
            InitializeComponent();            
        }        
    }
}
