#region
using System;
using System.Configuration;
using System.Net;
using System.Net.Sockets;
#endregion

namespace DebugReplicator.Controller
{
    public class GlobalVars
    {
        #region Global Variables        
        public static readonly string CARACTER_BANDERA  = ConfigurationManager.AppSettings["CaracterBandera"];        
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

