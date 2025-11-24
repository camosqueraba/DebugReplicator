using DebugReplicator.Model;
using DebugReplicator.Model.DTOs;
using MS.WindowsAPICodePack.Internal;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml;
using System.Xml.Linq;

namespace DebugReplicator.Controller
{
    public class GestorArchivosConfiguracion
    {
        /// <summary>
        /// Carga un archivo de configuración externo y devuelve sus pares clave-valor.
        /// Soporta formatos JSON, XML y texto plano tipo INI.
        /// </summary>
        public static Dictionary<string, string> LeerArchivoConfiguracionExterno(string rutaArchivoConfig)
        {
            if (string.IsNullOrWhiteSpace(rutaArchivoConfig))
                throw new ArgumentNullException(nameof(rutaArchivoConfig));

            if (!File.Exists(rutaArchivoConfig))
                throw new FileNotFoundException($"Archivo no encontrado: {rutaArchivoConfig}");

            string extension = Path.GetExtension(rutaArchivoConfig).ToLowerInvariant();

            Dictionary<string, string> claveValor = null;

            switch (extension)
            {
                case ".json":
                    //LoadJson(filePath);
                    break;
                
                case ".config":
                    claveValor = LoadXml(rutaArchivoConfig);
                    break;

                case ".ini":
                case ".conf":
                case ".txt":
                    LoadKeyValue(rutaArchivoConfig);
                    break;

                default:
                    break;
            }
            
            return claveValor;
        }

       /*
        private static Dictionary<string, string> LoadJson(string filePath)
        {

            var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            
            var jsonText = File.ReadAllText(filePath);

            using var doc = JsonDocument.Parse(jsonText);
            ParseJsonElement(doc.RootElement, "", result);
            
            return result;
        }

        private static void ParseJsonElement(JsonElement element, string prefix, Dictionary<string, string> dict)
        {
            foreach (var prop in element.EnumerateObject())
            {
                string key = string.IsNullOrEmpty(prefix) ? prop.Name : $"{prefix}:{prop.Name}";
                if (prop.Value.ValueKind == JsonValueKind.Object)
                {
                    ParseJsonElement(prop.Value, key, dict);
                }
                else
                {
                    dict[key] = prop.Value.ToString();
                }
            }
        }
        */

        //Formato XML
        private static Dictionary<string, string> LoadXml(string filePath)
        {
            var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            
            XmlDocument docXml = new XmlDocument();
            docXml.Load(filePath);

            XmlNodeList items = docXml.SelectNodes("/configuration/appSettings");
            
            foreach (XmlNode item in items)
            {
                if(item.ChildNodes.Count > 0)
                {
                    foreach (XmlNode configuracion in item.ChildNodes)
                    {
                        string algo = configuracion.OuterXml;
                        XElement algoParseado = XElement.Parse(algo);

                        // Extraemos los atributos "key" y "value"
                        string key = algoParseado.Attribute("key")?.Value ?? string.Empty;
                        string value = algoParseado.Attribute("value")?.Value ?? string.Empty;
                        result.Add(key, value);
                    }
                }

            }

            return result;
        }

        private static void ParseXmlElement(XElement element, string path, Dictionary<string, string> dict)
        {
            foreach (var attr in element.Attributes())
                dict[$"{path}@{attr.Name.LocalName}"] = attr.Value;

            if (!element.HasElements)
            {
                dict[path] = element.Value.Trim();
            }
            else
            {
                foreach (var child in element.Elements())
                    ParseXmlElement(child, $"{path}:{child.Name.LocalName}", dict);
            }
        }

        //Formato clave=valor (INI, CONF, TXT)
        private static Dictionary<string, string> LoadKeyValue(string filePath)
        {
            var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            foreach (var rawLine in File.ReadAllLines(filePath))
            {
                var line = rawLine.Trim();

                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("#") || line.StartsWith("//"))
                    continue;

                int separatorIndex = line.IndexOf('=');
                if (separatorIndex <= 0) continue;

                string key = line.Substring(0, separatorIndex).Trim();
                string value = line.Substring(separatorIndex + 1).Trim();

                if (!string.IsNullOrEmpty(key))
                    result[key] = value;
            }

            return result;
        }

        public static ResultadoProceso ModificarArchivoConfiguracionExterno(string rutaArchivoConfig, List<ClaveValorModel> nuevasConfiguraciones)
        {
            ResultadoProceso resultadoProceso = new ResultadoProceso();

            try
            {
                if (string.IsNullOrWhiteSpace(rutaArchivoConfig))
                    throw new ArgumentNullException(nameof(rutaArchivoConfig));

                if (!File.Exists(rutaArchivoConfig))
                    throw new FileNotFoundException($"Archivo no encontrado: {rutaArchivoConfig}");

                string extension = Path.GetExtension(rutaArchivoConfig).ToLowerInvariant();

               

                switch (extension)
                {
                    case ".json":
                        //LoadJson(rutaArchivoConfig);
                        break;

                    case ".config":
                        resultadoProceso = ModificarXml(rutaArchivoConfig, nuevasConfiguraciones);
                        break;

                    case ".ini":
                    case ".conf":
                    case ".txt":
                        resultadoProceso = ModificarKeyValue(rutaArchivoConfig, nuevasConfiguraciones);
                        break;

                    default:
                        break;
                }

                return resultadoProceso;
            }
            catch (Exception ex)
            {

                throw;
            }
               
        }

        private static ResultadoProceso ModificarKeyValue(string rutaArchivoConfig, List<ClaveValorModel> nuevasConfiguraciones)
        {
            throw new NotImplementedException();
        }

        private static ResultadoProceso ModificarXml(string rutaArchivoConfig, List<ClaveValorModel> nuevasConfiguraciones)
        {            
            
            ResultadoProceso resultadoProceso = new ResultadoProceso();

            try
            {
                XmlDocument docXml = new XmlDocument();
                docXml.Load(rutaArchivoConfig);

                XmlNodeList items = docXml.SelectNodes("/configuration/appSettings");

                foreach (XmlNode item in items)
                {
                    if (item.ChildNodes.Count > 0)
                    {
                        foreach (XmlNode configuracion in item.ChildNodes)
                        {
                            string tag = configuracion.OuterXml;
                            XElement tagParseada = XElement.Parse(tag);

                            // Extraemos los atributos "key" y "value"
                            string key = tagParseada.Attribute("key")?.Value ?? string.Empty;

                            if(key != string.Empty)
                            {
                                ClaveValorModel nuevaConfiguracion = nuevasConfiguraciones.FirstOrDefault(c => c.Clave == key);
                                if(nuevaConfiguracion != null)
                                {
                                    tagParseada.SetAttributeValue("value", nuevaConfiguracion.Valor);
                                    configuracion.InnerXml = tagParseada.ToString();
                                }
                            }

                            string value = tagParseada.Attribute("value")?.Value ?? string.Empty;
                            //result.Add(key, value);
                        }                        
                    }
                }

                docXml.Save(rutaArchivoConfig);

                return resultadoProceso;
            }
            catch (Exception ex)
            {

                throw;
            }            
        }
    }
}
