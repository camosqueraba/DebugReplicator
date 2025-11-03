using DebugReplicator.Model;
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

namespace DebugReplicator.View.UIControls
{
    /// <summary>
    /// Interaction logic for ClaveValorControl.xaml
    /// </summary>
    public partial class ClaveValorControl : UserControl
    {
        public ClaveValorModel ClaveValor
        {
            get => this.DataContext as ClaveValorModel;
            set => this.DataContext = value;
        }

        public ClaveValorControl()
        {
            InitializeComponent();
            ClaveValor = new ClaveValorModel();
        }

        public ClaveValorControl(ClaveValorModel claveValorModel)
        {
            InitializeComponent();
            ClaveValor = claveValorModel;
        }
    }
}
