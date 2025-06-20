﻿using System;
using System.Windows.Controls;
using System.Windows.Input;

namespace DebugReplicator.UIControls
{
    /// <summary>
    /// Interaction logic for FilesControl.xaml
    /// </summary>
    public partial class FilesControl : UserControl
    {
        public FileModel File
        {
            get => this.DataContext as FileModel;
            set => this.DataContext = value;
        }

        /// <summary>
        /// A callback used for telling 'something' to navigate to the path
        /// </summary>
        public Action<FileModel> NavigateToPathCallback { get; set; }

        public FilesControl()
        {
            InitializeComponent();
            File = new FileModel();
        }

        public FilesControl(FileModel fModel)
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
                NavigateToPathCallback?.Invoke(File);
            }
        }

        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                NavigateToPathCallback?.Invoke(File);
            }
        }

        private void CheckBox_Checked(object sender, System.Windows.RoutedEventArgs e)
        {
            File.Seleccionado = true;
        }

        private void CheckBox_Unchecked(object sender, System.Windows.RoutedEventArgs e)
        {
            File.Seleccionado = false;
        }
    }
}
