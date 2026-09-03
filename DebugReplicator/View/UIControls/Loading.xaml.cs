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
    /// Lógica de interacción para Loading.xaml
    /// </summary>
    public partial class Loading : UserControl
    {
        public Loading()
        {
            InitializeComponent();
        }

        //public static readonly DependencyProperty IsVisibleProperty =
        //    DependencyProperty.Register(nameof(IsVisible), typeof(bool), typeof(Loading), new PropertyMetadata(false));

        /*
        public static readonly DependencyProperty IsIndeterminateProperty =
            DependencyProperty.Register(nameof(IsIndeterminate), typeof(bool), typeof(BusyIndicator), new PropertyMetadata(true));
        */

        /*
        public static readonly DependencyProperty CancelCommandProperty =
            DependencyProperty.Register(nameof(CancelCommand), typeof(ICommand), typeof(BusyIndicator), new PropertyMetadata(null));
        */

        //public bool IsVisible
        //{
        //    get => (bool)GetValue(IsVisibleProperty);
        //    set => SetValue(IsVisibleProperty, value);
        //}

        /*
        public bool IsIndeterminate
        {
            get => (bool)GetValue(IsIndeterminateProperty);
            set => SetValue(IsIndeterminateProperty, value);
        }

        public ICommand CancelCommand
        {
            get => (ICommand)GetValue(CancelCommandProperty);
            set => SetValue(CancelCommandProperty, value);
        }
        */
    }
}
