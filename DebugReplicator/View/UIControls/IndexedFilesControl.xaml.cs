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
    /// Interaction logic for SelectedFilesControl.xaml
    /// </summary>
    public partial class IndexedFileControl : UserControl
    {
        public FileModel File
        {
            get => this.DataContext as FileModel;
            set => this.DataContext = value;
        }
        public IndexedFileControl()
        {
            InitializeComponent();
            File = new FileModel();
        }

        public IndexedFileControl(FileModel fModel)
        {
            InitializeComponent();
            File = fModel;
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left &&
                e.LeftButton == MouseButtonState.Pressed &&
                e.ClickCount == 2)
            {
                //NavigateToPathCallback?.Invoke(File);
            }
        }

        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                //NavigateToPathCallback?.Invoke(File);
            }
        }
    }
}
