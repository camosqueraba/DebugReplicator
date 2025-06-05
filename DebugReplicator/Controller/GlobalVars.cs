#region
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
#endregion

namespace DebugReplicator.Controller
{  
    public class GlobalVars
    {
        #region Global Variables        
        public static string LOGs_GUID { get; set; } = Guid.NewGuid().ToString("N");
        public static bool OrderCerrar { get; set; }
        public static string RutaDescargaCarta { get; set; }

        #endregion

        #region Datos Maquina/ BOT
        

        public static readonly string NameRPA  = ConfigurationManager.AppSettings["NameRPA"];
        public static readonly string VersionProject        = ConfigurationManager.AppSettings["VersionProject"];
        public static readonly string TituloVentana         = ConfigurationManager.AppSettings["TituloVentana"];

        public static readonly string NombreMaquina = Environment.MachineName;
        public static readonly string UsuarioRed = Environment.UserName;
        public static readonly string DireccionIP = GetIPAddress();
        
        #endregion

        #region Obtener IP
        private static string GetIPAddress()
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
    }
}

