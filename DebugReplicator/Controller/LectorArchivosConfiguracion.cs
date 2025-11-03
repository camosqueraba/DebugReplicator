using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DebugReplicator.Controller
{
    public class LectorArchivosConfiguracion
    {
        /// <summary>
        /// Carga un archivo de configuración externo y devuelve sus pares clave-valor.
        /// Soporta formatos JSON, XML y texto plano tipo INI.
        /// </summary>
        public static Dictionary<string, string> LeerArchivoConfiguracionExterno(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentNullException(nameof(filePath));

            if (!File.Exists(filePath))
                throw new FileNotFoundException($"Archivo no encontrado: {filePath}");

            string extension = Path.GetExtension(filePath).ToLowerInvariant();

            Dictionary<string, string> claveValor = null;

            switch (extension)
            {
                case ".json":
                    //LoadJson(filePath);
                    break;
                
                case ".config":
                    claveValor = LoadXml(filePath);
                    break;

                case ".ini":
                case ".conf":
                case ".txt":
                    LoadKeyValue(filePath);
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
            var xml = XDocument.Load(filePath);

            if (xml.Root != null)
                ParseXmlElement(xml.Root, xml.Root.Name.LocalName, result);

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
    }
}
