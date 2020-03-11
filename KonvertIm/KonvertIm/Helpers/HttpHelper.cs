using AngleSharp.Dom;
using KonvertIm.Components;
using Leaf.xNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
namespace KonvertIm.Helpers
{
    public class HttpHelper
    {
        public static RequestParams ConvertToRequestParams(List<ParserInputData> inputs, List<string> excludeKeys = null)
        {
            var formParams = new RequestParams();

            foreach (var input in inputs)
            {
                if (excludeKeys != null && excludeKeys.IndexOf(input.name) != -1)
                    continue;

                string valParam = input.value;
                string keyParam = input.name;

                formParams.Add(new KeyValuePair<string, string>(keyParam, valParam));
            }

            return formParams;
        }

        public static string GetVarUrl(string url, string var)
        {
            return url.Replace(" ", "")
                .Split('?').Last()
                .Split('&').Where(itemVar => itemVar.IndexOf(var) == 0).First()
                .Replace(var, "");
        }
    }
}
