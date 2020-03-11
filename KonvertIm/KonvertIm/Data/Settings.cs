using Leaf.xNet;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using static KonvertIm.Data.ProxySettings;

namespace KonvertIm.Data
{
    public class SettingsData
    {
        public string[] allowEmails;
        public int cntTryOnFault;

        static string FILE_NAME
        {
            get { return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Settings.json"; }
        }

        public static SettingsData Load()
        {
            string jsonString = System.IO.File.ReadAllText(FILE_NAME);
            SettingsData data = Newtonsoft.Json.JsonConvert.DeserializeObject<SettingsData>(jsonString);
            return data;
        }
    }

    public class ProxySettings
    {
        public class ProxySettingsItem
        {
            public int port { get; set; }
            public string ip { get; set; }
            public string username { get; set; }
            public string password { get; set; }

            public HttpProxyClient CreateProxyClient()
            {
                //Строка вида - протокол://хост:порт:имя_пользователя:пароль
                string proxyStr = String.Format(
                    "{0}:{1}:{2}:{3}",
                    ip,
                    port,
                    username,
                    password
                );

                return HttpProxyClient.Parse(proxyStr);
            }
        }

        static string FILE_NAME
        {
            get { return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\Proxy.json"; }
        }

        public static ProxySettingsItem[] Load()
        {
            if (!File.Exists(FILE_NAME))
                return null;

            string jsonString = System.IO.File.ReadAllText(FILE_NAME);
            return JsonConvert.DeserializeObject<ProxySettingsItem[]>(jsonString);
        }
    }

    public class Settings
    {
        public SettingsData data;
        public ProxySettingsItem[] proxyItems;
    }
}
