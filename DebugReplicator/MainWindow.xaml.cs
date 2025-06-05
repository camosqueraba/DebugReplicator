using DebugReplicator.Controller;
using DebugReplicator.View;
using System;
using System.Net;
using System.Net.Sockets;
using System.Windows;
using System.Windows.Threading;

namespace DebugReplicator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DateTime TiempoInicial { get; set; }
        
        public MainWindow()
        {
            this.Title = GlobalVars.TituloVentana;

            string direccionIP = GlobalVars.DireccionIP;
            string nombreMaquina = GlobalVars.NombreMaquina;
            string usuarioRed = GlobalVars.UsuarioRed;

            DispatcherTimer LiveTime = new DispatcherTimer();
            LiveTime.Interval = TimeSpan.FromSeconds(1);
            LiveTime.Tick += TimerTick;
            LiveTime.Start();

            InitializeComponent();

            lbl_nombre_usuario.Content = usuarioRed + " ";
            lbl_informacion_maquina.Content = nombreMaquina + " " + direccionIP + " ";
            lbl_vesion_proyecto.Content = GlobalVars.VersionProject;
            ContenedorLoading.Visibility = Visibility.Hidden;

            GridContenidoPrincipal.Children.Clear();
            GridContenidoPrincipal.Children.Add(new VistaPrincipal());
        }


        #region Cerrar Ventana
        private void CerrarVentana(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                MessageBoxResult result = MessageBox.Show("¿Desea cerrar el aplicativo?", GlobalVars.TituloVentana, MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    TiempoInicial = DateTime.Now;
                    LOGRobotica.Controllers.LogApplication.LogWrite("MainWindow -> CerrarVentana" + "Se oprime el botón 'Yes' y se cierra la aplicación de manera manual");
                    //LOGRobotica.Controllers.LogWebServices.logsWS(TiempoInicial, IdUnique, "Cerrar RPA", "Exitosa", "Cierre manual", "", "", "", "", "", "", "SAC_Sim_Login");
                    Application.Current.Shutdown();
                }
                else
                {
                    e.Cancel = true;
                    LOGRobotica.Controllers.LogApplication.LogWrite("MainWindow -> CerrarVentana" + "Se oprime el botón 'No' y se continúa en la Aplicación");
                }
            }
            catch (Exception ex)
            {
                LOGRobotica.Controllers.LogApplication.LogWrite("MainWindow - CerrarVentana" + " -> " + "Exception: " + ex.Message);
            }
        }
        #endregion

        #region Reloj
        public void TimerTick(object sender, EventArgs e)
        {
            try
            {
                lbl_fecha.Content = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " ";
            }
            catch (Exception ex)
            {
                LOGRobotica.Controllers.LogApplication.LogWrite("MainWindow -> TimerTick" + " : " + " Exception: " + ex.Message);
            }
        }
        #endregion

        
    }
}
