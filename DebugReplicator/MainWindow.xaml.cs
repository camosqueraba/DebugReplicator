using DebugReplicator.Controller;
using DebugReplicator.View;
using DebugReplicator.ViewModel;
using System;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Threading;

namespace DebugReplicator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private double AlturaNormal { get; set; }
        private double AnchoNormal { get; set; }

        CompositionTarget WindowCompositionTarget { get; set; }

        double CachedMinWidth { get; set; }

        double CachedMinHeight { get; set; }

        POINT CachedMinTrackSize { get; set; }

        public MainWindow()
        {

            string direccionIP = GetIPAddress();
            string nombreMaquina = GlobalVars.NombreMaquina ;
            string usuarioRed = GlobalVars.UsuarioRed;

            DispatcherTimer LiveTime = new DispatcherTimer();
            LiveTime.Interval = TimeSpan.FromSeconds(1);
            LiveTime.Tick += Timer_Tick;
            LiveTime.Start();

            InitializeComponent();
            SourceInitialized += (s, e) =>
            {
                WindowCompositionTarget = PresentationSource.FromVisual(this).CompositionTarget;
                HwndSource.FromHwnd(new WindowInteropHelper(this).Handle).AddHook(WindowProc);
            };

            AlturaNormal = this.Height;
            AnchoNormal = this.Width;

            texblock_titulo_ventana.Text = GlobalVars.TituloVentana;

            lbl_nombre_usuario.Content = usuarioRed + " ";
            lbl_informacion_maquina.Content = nombreMaquina + " " + direccionIP + " ";
            lbl_vesion_proyecto.Content = GlobalVars.VersionProject;

            boton_restaurar.Visibility = Visibility.Collapsed;
        }

        #region Cerrar Ventana
        private void Cerrar_Ventana(object sender, RoutedEventArgs e)
        {
            try
            {
                MessageBoxResult result = MessageBox.Show("¿Desea cerrar el aplicativo?", $"Konecta - {GlobalVars.NameRPA}", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result == MessageBoxResult.Yes)
                {
                    DateTime tiempo_Inicial = DateTime.Now;

                    LOGRobotica.Controllers.LogApplication.LogWrite("MainWindow -> Cerrar_Ventana" + " : " + "Se oprime el botón 'Yes' y se cierra la aplicación de manera manual");
                    //LOGRobotica.Controllers.LogWebServices.logsWS(tiempo_Inicial, GlobalVars.LOGs_GUID, "Cerrar aplicacion", "Exitosa", "Cierre manual");

                    Application.Current.Shutdown();
                    this.Close();
                }
                else
                {
                    LOGRobotica.Controllers.LogApplication.LogWrite("MainWindow -> Cerrar_Ventana" + " : " + "Se oprime el botón 'No' y se continúa en la Aplicación");
                }
            }
            catch (Exception ex)
            {
                LOGRobotica.Controllers.LogApplication.LogWrite("MainWindow -> Cerrar_Ventana" + " : " + "Exception: " + ex.Message);
            }
        }
        #endregion

        #region Minimizar
        private void Minimizar_Ventana(object sender, RoutedEventArgs e)
        {
            try
            {
                WindowState = WindowState.Minimized;
            }
            catch (Exception ex)
            {
                LOGRobotica.Controllers.LogApplication.LogWrite("MainWindow -> Minimizar_Ventana : Exception" + ex.Message);
            }
        }
        #endregion

        #region Maximizar
        private void Maximizar_Ventana(object sender, RoutedEventArgs e)
        {
            try
            {                
                if (this.WindowState == WindowState.Maximized)
                {
                    this.WindowState = WindowState.Normal;
                }
                else
                {
                    this.WindowState = WindowState.Maximized;
                }                

                boton_restaurar.Visibility = Visibility.Visible;
                boton_maximizar.Visibility = Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                LOGRobotica.Controllers.LogApplication.LogWrite("MainWindow -> Maximizar_Ventana : Exception" + ex.Message);
            }
        }
        #endregion

        #region Restaurar
        private void Restaurar_Ventana(object sender, RoutedEventArgs e)
        {
            try
            {
                this.WindowState = WindowState.Normal;
                this.Height = AlturaNormal;
                this.Width = AnchoNormal;


                boton_restaurar.Visibility = Visibility.Collapsed;
                boton_maximizar.Visibility = Visibility.Visible;

            }
            catch (Exception ex)
            {
                LOGRobotica.Controllers.LogApplication.LogWrite("MainWindow -> Restaurar_Ventana : Exception" + ex.Message);
            }
        }
        #endregion
        private void Window_StateChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.WindowState == WindowState.Normal)
                {
                    boton_restaurar.Visibility = Visibility.Collapsed;
                    boton_maximizar.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                LOGRobotica.Controllers.LogApplication.LogWrite("MainWindow -> Restaurar_Ventana : Exception" + ex.Message);
            }

        }

        #region evitar cierre
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                e.Cancel = true;
            }
            catch (Exception ex)
            {
                LOGRobotica.Controllers.LogApplication.LogWrite("MainWindow -> Wi_Closin" + " : " + "Exception" + ex.Message);
            }
        }
        #endregion

        #region Obtener IP
        public static string GetIPAddress()
        {
            try
            {
                var host = Dns.GetHostEntry(Dns.GetHostName());

                foreach (var ip in host.AddressList)
                {
                    if (ip.AddressFamily == AddressFamily.InterNetwork)
                    {
                        return ip.ToString();
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                LOGRobotica.Controllers.LogApplication.LogWrite("MainWindow -> GetIPAddress" + " : " + "Exception" + ex.Message);
                return null;
            }
        }
        #endregion

        #region Reloj
        public void Timer_Tick(object sender, EventArgs e)
        {
            try
            {
                lbl_fecha.Content = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + " ";
            }
            catch (Exception ex)
            {
                LOGRobotica.Controllers.LogApplication.LogWrite("MainWindow -> timer_Tick" + " : " + " Exception: " + ex.Message);
            }
        }
        #endregion


        #region logica manejo de tamanio de pantalla
        IntPtr WindowProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case 0x0024:
                    MINMAXINFO mmi = (MINMAXINFO)Marshal.PtrToStructure(lParam, typeof(MINMAXINFO));
                    IntPtr monitor = MonitorFromWindow(hwnd, 0x00000002 /*MONITOR_DEFAULTTONEAREST*/);
                    if (monitor != IntPtr.Zero)
                    {
                        MONITORINFO monitorInfo = new MONITORINFO { };
                        GetMonitorInfo(monitor, monitorInfo);
                        RECT rcWorkArea = monitorInfo.rcWork;
                        RECT rcMonitorArea = monitorInfo.rcMonitor;
                        mmi.ptMaxPosition.x = Math.Abs(rcWorkArea.left - rcMonitorArea.left);
                        mmi.ptMaxPosition.y = Math.Abs(rcWorkArea.top - rcMonitorArea.top);
                        mmi.ptMaxSize.x = Math.Abs(rcWorkArea.right - rcWorkArea.left);
                        mmi.ptMaxSize.y = Math.Abs(rcWorkArea.bottom - rcWorkArea.top);
                        if (!CachedMinTrackSize.Equals(mmi.ptMinTrackSize) || CachedMinHeight != MinHeight && CachedMinWidth != MinWidth)
                        {
                            mmi.ptMinTrackSize.x = (int)((CachedMinWidth = MinWidth) * WindowCompositionTarget.TransformToDevice.M11);
                            mmi.ptMinTrackSize.y = (int)((CachedMinHeight = MinHeight) * WindowCompositionTarget.TransformToDevice.M22);
                            CachedMinTrackSize = mmi.ptMinTrackSize;
                        }
                    }
                    Marshal.StructureToPtr(mmi, lParam, true);
                    handled = true;
                    break;
            }
            return IntPtr.Zero;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int x;
            public int y;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MINMAXINFO
        {
            public POINT ptReserved;
            public POINT ptMaxSize;
            public POINT ptMaxPosition;
            public POINT ptMinTrackSize;
            public POINT ptMaxTrackSize;
        };

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        public class MONITORINFO
        {
            public int cbSize = Marshal.SizeOf(typeof(MONITORINFO));
            public RECT rcMonitor = new RECT { };
            public RECT rcWork = new RECT { };
            public int dwFlags = 0;
        }

        [StructLayout(LayoutKind.Sequential, Pack = 0)]
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;
        }

        [DllImport("user32")]
        internal static extern bool GetMonitorInfo(IntPtr hMonitor, MONITORINFO lpmi);

        [DllImport("User32")]
        internal static extern IntPtr MonitorFromWindow(IntPtr handle, int flags);
        #endregion

    }
}
