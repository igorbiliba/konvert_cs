using AngleSharp.Dom;
using AngleSharp.Html.Parser;
using KonvertIm.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace KonvertIm.Components
{
    public class Parser
    {
        string content;
        public Parser(string content)
        {
            this.content = content;
        }

        public IElement GetForm(string className)
        {
            var parser = new HtmlParser();
            var document = parser.ParseDocument(content);

            var forms = document.GetElementsByTagName("form");
            foreach (var form in forms)
            {
                if (form.ClassName.Trim().ToLower() == className.Trim().ToLower())
                    return form;
            }

            return null;
        }

        public string GetContent(string className)
        {
            var parser = new HtmlParser();
            var document = parser.ParseDocument(content);

            return document
                .GetElementsByClassName(className)
                .First()
                .TextContent;
        }

        public string GetValueInput(string name)
        {
            var parser = new HtmlParser();
            var document = parser.ParseDocument(content);

            return document
                .GetElementsByName(name)
                .First()
                .GetAttribute("value");
        }

        public List<ParserInputData> GetInputsByForm(IElement form)
        {
            List<ParserInputData> list = new List<ParserInputData>();

            foreach (IElement input in form.QuerySelectorAll("input"))
            {
                list.Add(new ParserInputData(input));
            }

            return list;
        }

        public string GetCsrf(List<ParserInputData> inputs)
        {
            try
            {
                return inputs.Where(el => el.name == "_csrf")
                    .First()
                    .value;
            }
            catch (Exception) { }

            return null;
        }

        public Parser ModifyInputs(ref List<ParserInputData> inputs, string name, string value)
        {
            for (int i = 0; i < inputs.Count(); i++)
                if (inputs[i].name.ToLower().Trim() == name.ToLower().Trim())
                    inputs[i].value = value;

            return this;
        }

        public string GetHref(string className)
        {
            var parser = new HtmlParser();
            var document = parser.ParseDocument(content);

            return document
                .GetElementsByClassName(className)
                .First()
                .GetAttribute("href");
        }

        public double GetAmount(string className, string currency)
        {
            var parser = new HtmlParser();
            var document = parser.ParseDocument(content);
            var elements = document.GetElementsByClassName(className);

            foreach (var element in elements)
            {
                string elemContent = element.TextContent.ToLower();
                if (elemContent.IndexOf(currency.ToLower().Trim()) == -1) continue;

                string pattern = @"\d+\.\d+";
                Regex regex = new Regex(pattern);
                string numberStr = regex.Match(elemContent).ToString();

                return MoneyHelper.ToDouble(numberStr);
            }

            return -1;
        }

        public double GetRate(string start = "1BTC", string end = "<")
        {
            var parser = new HtmlParser();
            var document = parser.ParseDocument(content);

            var urls = document.GetElementsByTagName("a");

            foreach (IElement url in urls)
            {
                if (url.GetAttribute("href") != "/btc2qiwi/") continue;
                string content = url
                    .InnerHtml
                    .ToUpper()
                    .Replace(" ", "");

                int startPos  = content.IndexOf(start.ToUpper()) + start.Length;
                int endPos    = content.IndexOf(end.ToUpper(), startPos);
                int amountLen = endPos - startPos;

                string draftAmount = content.Substring(startPos, amountLen);
                string strAmount = Regex.Match(draftAmount, @"\d+\,?\d+").Value;

                return MoneyHelper.ToDouble(strAmount.Replace(",", ""));
            }

            return 0;
        }

        public string FindUrlBy(string bullet)
        {
            var parser = new HtmlParser();
            var document = parser.ParseDocument(content);

            var urls = document.GetElementsByTagName("a");
            foreach (IElement url in urls)
            {
                if (url.GetAttribute("href").IndexOf(bullet) == 0)
                    return url
                        .GetAttribute("href")
                        .Replace("%5B", "[")
                        .Replace("%27", "'")
                        .Replace("%5D", "]");
            }

            return null;
        }

        public string GetPlaceholder(string name)
        {
            var parser = new HtmlParser();
            var document = parser.ParseDocument(content);
            
            return document
                .GetElementsByName(name)
                .First()
                .GetAttribute("placeholder");
        }
    }
}
