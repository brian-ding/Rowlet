using System.Diagnostics;
using System.IO;
using Newtonsoft.Json.Linq;

namespace Rowlet
{
    public class ConfigManager
    {
        private static ConfigManager _manager;
        private static object _o = new object();

        private readonly JObject _config;

        private ConfigManager()
        {
            _config = JObject.Parse(File.ReadAllText("config.json"));

        }

        public static string GetConfig(string key)
        {
            if (_manager == null)
            {
                lock (_o)
                {
                    if (_manager == null)
                    {
                        _manager = new ConfigManager();
                    }
                }
            }

            string value = _manager._config[key].ToString();
            Debug.WriteLine($"Get config value: {value}");
            return value;
        }
    }
}