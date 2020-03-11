using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KonvertIm.Components
{
    public class EmailStorageModel
    {
        const string KEY = "email";
        StorageModel storage;

        public EmailStorageModel(StorageModel storage)
        {
            this.storage = storage;
        }

        public string Get()
            => storage.Get(KEY);

        public void Update(string val)
            => storage.Set(KEY, val);
    }
}
