using DebugReplicator.Controller;
using DebugReplicator.Model.DTOs;
using DebugReplicator.ViewModel;
using Microsoft.WindowsAPICodePack.Dialogs;
using System;
using System.Windows;
using System.Windows.Controls;
using WinForms = System.Windows.Forms;

namespace DebugReplicator.View
{
    /// <summary>
    /// Lógica de interacción para VistaPrincipal.xaml
    /// </summary>
    public partial class VistaPrincipal : UserControl
    {              

        private MainWindow VentanaPrincipal = (MainWindow)Application.Current.MainWindow;

        public VistaPrincipal()
        {
            InitializeComponent();
        }        
        
    }
}
