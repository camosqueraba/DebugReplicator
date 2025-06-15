using DebugReplicator.Controller;
using DebugReplicator.DTOs;
using DebugReplicator.ViewModels;
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
        private static string RutaCarpetaOrigen { get; set; }
        private static string RutaCarpetaDestino { get; set; }        

        private MainWindow VentanaPrincipal = (MainWindow)Application.Current.MainWindow;

        public VistaPrincipal()
        {
            InitializeComponent();
        }

        private void ButtonSeleccionarCarpetaClick(object sender, RoutedEventArgs e)
        {
            string boton = (string)((Button)sender).Tag;

            /*
            WinForms.ToolStripMenuItem openMenuItem = new WinForms.ToolStripMenuItem();
            WinForms.OpenFileDialog openFileDialog1 = new WinForms.OpenFileDialog();
            WinForms.FolderBrowserDialog folderBrowserDialog1 = new WinForms.FolderBrowserDialog();            

            folderBrowserDialog1.SelectedPath = Environment.GetEnvironmentVariable("USERPROFILE") + @"\" + "Downloads";
            
            WinForms.DialogResult result = folderBrowserDialog1.ShowDialog();

            bool fileOpened = false;

            if (result == WinForms.DialogResult.OK)
            {
                string folderName = folderBrowserDialog1.SelectedPath;
                if (!fileOpened)
                {
                    // No file is opened, bring up openFileDialog in selected path.
                    openFileDialog1.InitialDirectory = folderName;
                    openFileDialog1.FileName = null;
                    openMenuItem.PerformClick();

                    if(boton == "carpeta_origen")
                        TextblockCarpetaOrigen.Text = RutaCarpetaOrigen = folderName;
                    if (boton == "carpeta_destino")
                        TextblockCarpetaDestino.Text = RutaCarpetaDestino = folderName;
                }
            }
            openFileDialog.FileName = null;
            */

            CommonOpenFileDialog openFileDialog = new CommonOpenFileDialog();
            openFileDialog.IsFolderPicker = true;
            openFileDialog.DefaultFileName = string.Empty;
            openFileDialog.InitialDirectory = Environment.GetEnvironmentVariable("USERPROFILE") + @"\" + "Downloads";
                         
            CommonFileDialogResult result = openFileDialog.ShowDialog();

            if (result == CommonFileDialogResult.Ok)
            {
                string folderName = openFileDialog.FileName;

                if (boton == "carpeta_origen")
                    TextblockCarpetaOrigen.Text = RutaCarpetaOrigen = folderName;
                if (boton == "carpeta_destino")
                    TextblockCarpetaDestino.Text = RutaCarpetaDestino = folderName;
            }

            /*
            var dialog = new WinForms.FolderBrowserDialog();
            dialog.SelectedPath = Environment.GetEnvironmentVariable("USERPROFILE") + @"\" + "Downloads";
            WinForms.DialogResult result = dialog.ShowDialog();

            if (result == WinForms.DialogResult.OK)
            {
                //textbox_ruta.Text = dialog.SelectedPath;
            }
            */
        }

        private void ButtonSiguiente_Click(object sender, RoutedEventArgs e)
        {
            Replicador replicador = new Replicador();
            string[] archivos_indexar = new string[2];
            //replicador.ReplicarCarpetaDebug(RutaCarpetaOrigen, "BOT_", 1, 10, RutaCarpetaDestino, archivos_indexar);
            ResultadoProceso resultCopiar = replicador.CopiarCarpetaBaseADestino(RutaCarpetaOrigen, RutaCarpetaDestino);

            if (resultCopiar != null && resultCopiar.Resultado)
            {
                string carpetaReplicada = resultCopiar.ResultadoContenido;
                VentanaPrincipal.GridContenidoPrincipal.Children.Clear();
                VentanaPrincipal.GridContenidoPrincipal.Children.Add(new VistaListaArchivos(carpetaReplicada));
            }

        }
    }
}
