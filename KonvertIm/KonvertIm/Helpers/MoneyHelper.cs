using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KonvertIm.Helpers
{
    public class MoneyHelper
    {
        public static double ToDouble(string val)
        {
            double amount = -1;
            try
            { amount = double.Parse(val.Replace(',', '.')); }
            catch (Exception)
            { amount = double.Parse(val.Replace('.', ',')); }

            return amount;
        }
    }
}
