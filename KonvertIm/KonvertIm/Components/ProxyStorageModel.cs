using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KonvertIm.Components
{
    public class ProxyStorageModel
    {
        StorageModel storage;

        public ProxyStorageModel(StorageModel storage)
        {
            this.storage = storage;
        }

        const string KEY = "proxy";

        public string Get()
        {
            return storage.Get(KEY);
        }

        public void Update(string val)
        {
            storage.Set(KEY, val);
        }
    }
}
