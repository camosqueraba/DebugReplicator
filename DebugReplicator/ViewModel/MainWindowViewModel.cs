using DebugReplicator.Controller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebugReplicator.ViewModel
{
    public class MainWindowViewModel : BaseViewModel
    {
        private readonly NavigationStore _navigationStore;
        public BaseViewModel CurrentViewModel => _navigationStore.CurrentViewModel;

        public MainWindowViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            _navigationStore.PropertyChanged += (_, __) => OnPropertyChanged(nameof(CurrentViewModel));
        }
    }
}
