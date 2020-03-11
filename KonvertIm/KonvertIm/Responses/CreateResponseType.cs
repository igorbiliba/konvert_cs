using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KonvertIm.Responses
{
    public class CreateResponseType
    {
        public string account;
        public string comment;
        public string btc_amount;
        public string ip;
        public string email;

        public string toJson() => Newtonsoft.Json.JsonConvert.SerializeObject(this);

        public bool IsValid()
        {
            if (account.Trim().Length == 0) return false;
            if (comment.Trim().Length == 0) return false;
            if (btc_amount.Length < 1 || btc_amount == "0" || btc_amount == null || btc_amount == "") return false;

            return true;
        }
    }
}
