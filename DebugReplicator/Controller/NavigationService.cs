using DebugReplicator.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DebugReplicator.Controller
{
    public class NavigationService
    {
        private readonly NavigationStore _navigationStore;
        private readonly Func<BaseViewModel> _createViewModel;

        public NavigationService(NavigationStore navigationStore, Func<BaseViewModel> createViewModel)
        {
            _navigationStore = navigationStore;
            _createViewModel = createViewModel;
        }

        public void Navigate()
        {
            _navigationStore.CurrentViewModel = _createViewModel();
        }
    }
}
