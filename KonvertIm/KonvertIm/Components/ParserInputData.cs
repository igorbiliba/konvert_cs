using AngleSharp.Dom;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KonvertIm.Components
{
    public class ParserInputData
    {
        IElement _input;
        public IElement input
        {
            get { return _input; }
        }

        public ParserInputData(IElement bullet)
        {
            this._input = bullet;
        }

        public string type
        {
            get { return _input.GetAttribute("type"); }
        }
        public string name
        {
            get { return _input.GetAttribute("name"); }
        }

        string _value = null;
        public string value
        {
            get
            {
                if (_value != null) return _value;
                return _input.GetAttribute("value");
            }
            set { _value = value; }
        }

        public string _checked
        {
            get { return _input.GetAttribute("checked"); }
        }

        public static bool CompareCookieItem(string haystack, string needle)
        {
            string haystackKey = haystack.Split('=').First().Trim();
            string needleKey = needle.Split('=').First().Trim();

            return haystackKey == needleKey;
        }
    }
}
