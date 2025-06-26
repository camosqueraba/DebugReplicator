using DebugReplicator.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DebugReplicator.ViewModel
{
    public class MenuViewModel : BaseViewModel
    {
        public ICommand IrFormularioCommand { get; }

        public MenuViewModel(NavigationService irAFormulario)
        {
            IrFormularioCommand = new RelayCommand(() => irAFormulario.Navigate());
        }
    }
}
