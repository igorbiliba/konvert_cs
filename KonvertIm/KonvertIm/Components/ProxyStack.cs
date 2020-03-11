using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static KonvertIm.Data.ProxySettings;

namespace KonvertIm.Components
{
    public class ProxyStack
    {
        ProxyStorageModel proxyStorageModel;
        ProxySettingsItem[] proxyItems;

        public ProxyStack(ProxySettingsItem[] proxyItems, ProxyStorageModel proxyStorageModel)
        {
            this.proxyItems = proxyItems;
            this.proxyStorageModel = proxyStorageModel;
        }

        int GetLastUsedId()
        {
            string lastUsedIp = proxyStorageModel.Get();
            return proxyStorageModel.Get() == null
                ? this.proxyItems.Length - 1
                : this.proxyItems
                    .ToList()
                    .FindLastIndex(
                        el => el.ip.Trim().ToLower() == lastUsedIp.Trim().ToLower()
                    );
        }

        public ProxySettingsItem Next()
        {
            if (this.proxyItems.Count() == 0)
                return null;

            int lastUsedId = GetLastUsedId();
            if (lastUsedId >= this.proxyItems.Length - 1) lastUsedId = 0;
            else lastUsedId++;
            ProxySettingsItem proxy = this.proxyItems[lastUsedId];

            proxyStorageModel.Update(proxy.ip);

            return proxy;
        }
    }
}
