using AngleSharp.Dom;
using KonvertIm.Components;
using KonvertIm.Helpers;
using KonvertIm.Responses;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace KonvertIm.Drivers
{
    public class KonvertImDriver
    {
        public const string BASE_URL   = "https://konvert.im/";
        public const string TICKET_URL = "https://konvert.im/o/{order}/";

        public Leaf.xNet.HttpRequest httpRequest = new Leaf.xNet.HttpRequest();
        public Leaf.xNet.HttpResponse current = null;

        public KonvertImDriver()
        {
            httpRequest.UserAgentRandomize();
            httpRequest.ConnectTimeout = 10000;
            httpRequest.AllowAutoRedirect = true;
        }

        public void LoadMainPage()
        {
            current = httpRequest.Get(BASE_URL);
        }

        public double GetRate()
        {
            Parser parser = new Parser(current.ToString());
            return MoneyHelper.ToDouble(parser.GetPlaceholder("btc_amount"));
        }

        public void Create( double amountRUB, double amountBTC, string phone, string addressBTC, string email )
        {
            Parser parser = new Parser(current.ToString());
            
            IElement form = parser.GetForm("form");
            List<ParserInputData> inputs = parser.GetInputsByForm(form);
            parser.ModifyInputs(ref inputs, "rub_amount",  amountRUB.ToString())
                  .ModifyInputs(ref inputs, "btc_amount",  amountBTC.ToString())
                  .ModifyInputs(ref inputs, "btc_address", addressBTC)
                  .ModifyInputs(ref inputs, "number",      phone);

            string action = BASE_URL + form.GetAttribute("action").Substring(1);
            current = httpRequest.Post(action, HttpHelper.ConvertToRequestParams(inputs));
        }

        public CreateResponseType ParseFinalPage(out string orderId)
        {
            Parser parser = new Parser(current.ToString());

            string qiwiUrl   = parser.FindUrlBy("https://qiwi.com");
            string qiwiPhone = HttpHelper.GetVarUrl(qiwiUrl, "extra['account']=");
                   orderId   = parser.GetValueInput("order_id");

            return new CreateResponseType() {
                account = "+" + qiwiPhone,
                comment = "qiwi"
            };
        }

        public bool CheckIsWork()
        {
            try
            {
                Leaf.xNet.HttpResponse mainPageContent = httpRequest.Get(BASE_URL);
                if (mainPageContent.ToString().Length > 0) return true;
            }
            catch (Exception) { }

            return false;
        }
    }
}
