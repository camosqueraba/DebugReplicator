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
        public VistaListaArchivosViewModel Model
        {
            get => this.DataContext as VistaListaArchivosViewModel;
            set => this.DataContext = value;
        }

        public VistaListaArchivos()
        {
            InitializeComponent();

            Model.TryNavigateToPath(@"C:\Users\camos\OneDrive\Documentos\Carlos\TrabajoKonecta\Simuladores\Bots_SAC\HUBIntegration_BOT64");
        }
    }
}
