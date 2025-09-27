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

        private bool _isVisible;
        public bool IsVisible
        {
            get => _isVisible;
            set { _isVisible = value; OnPropertyChanged(nameof(IsVisible)); }
        }


        private MainWindowViewModel(NavigationStore navigationStore)
        {
            _navigationStore = navigationStore;
            _navigationStore.PropertyChanged += (_, __) => OnPropertyChanged(nameof(CurrentViewModel));
            IsVisible = false;
        }

        private static MainWindowViewModel Instance;

        public static MainWindowViewModel GetInstance(NavigationStore navigationStore)
        {
            if (Instance == null)
            {
                Instance = new MainWindowViewModel(navigationStore);
            }
            return Instance;
        }        

        public void Show()
        {            
            IsVisible = true;
        }

        public void Hide()
        {
            IsVisible = false;
        }
    }
}
