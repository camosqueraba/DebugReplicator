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
        public IndexedFileModel IndexedFile
        {
            get => this.DataContext as IndexedFileModel;
            set => this.DataContext = value;
        }

        /// <summary>
        /// A callback used for telling 'something' to navigate to the path
        /// </summary>
        public Action<IndexedFileModel> BindToTextblockCallback { get; set; }

        public IndexedFileControl()
        {
            InitializeComponent();
            IndexedFile = new IndexedFileModel();
        }

        public IndexedFileControl(IndexedFileModel indexedFileModel)
        {
            InitializeComponent();
            IndexedFile = indexedFileModel;
        }

        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left &&
                e.LeftButton == MouseButtonState.Pressed &&
                e.ClickCount == 2)
            {
                BindToTextblockCallback?.Invoke(IndexedFile);
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
