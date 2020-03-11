using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KonvertIm.Helpers
{
    public class PhoneHelper
    {
        public static string PhoneReplacer(string phone)
            => "+" + (phone
                            .Replace(" ", String.Empty)
                            .Replace("+", String.Empty)
                            .Replace("-", String.Empty));
    }
}
