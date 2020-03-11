using AngleSharp.Dom;
using KonvertIm.Components;
using KonvertIm.Data;
using KonvertIm.Drivers;
using KonvertIm.Helpers;
using KonvertIm.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static KonvertIm.Data.ProxySettings;

namespace KonvertIm
{
    class Program
    {
        static int MAX_CNT_CHECK_PROXY = 30;
        const int ACTION_ID = 0;
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("no args");
                return;
            }

            Settings settings                   = new Settings() { data       = SettingsData.Load(),
                                                                   proxyItems = ProxySettings.Load() };
            DB db                               = new DB();
            StorageModel storageModel           = new StorageModel(db).MigrateUp();
            EmailStorageModel emailStorageModel = new EmailStorageModel(storageModel);
            EmailStack emailStack               = new EmailStack(emailStorageModel, settings.data.allowEmails);
            ProxyStorageModel proxyStorageModel = new ProxyStorageModel(storageModel);
            ProxySettingsItem proxy             = null;
            KonvertImDriver driver              = new KonvertImDriver();

            switch (args[ACTION_ID])
            {
                case "--create":
                    for (int nTry = 0; nTry < settings.data.cntTryOnFault; nTry++)
                    {
                        try
                        {
                            if (settings.proxyItems != null)
                            {
                                ProxyStack proxyStack = new ProxyStack(settings.proxyItems, proxyStorageModel);
                                while (--MAX_CNT_CHECK_PROXY > 0)
                                {
                                    try
                                    {
                                        var tester = new KonvertImDriver();
                                        proxy = proxyStack.Next();
                                        tester.httpRequest.Proxy = proxy.CreateProxyClient();

                                        if (tester.CheckIsWork()) break;
                                        else proxy = null;
                                    }
                                    catch (Exception) { }
                                }
                            }

                            if (proxy != null)
                                driver.httpRequest.Proxy = proxy.CreateProxyClient();

                            driver.LoadMainPage();

                            double amountRUB  = MoneyHelper.ToDouble(args[1]);
                            double amountBTC  = amountRUB * (driver.GetRate() / 5000);
                            string phone      = PhoneHelper.PhoneReplacer(args[2]);
                            string addressBTC = args[3];
                            string email      = new String(phone.Where(Char.IsDigit).ToArray()) + emailStack.Next();
                            
                            driver.Create(amountRUB, amountBTC, phone, addressBTC, email);
                            string orderId;
                            CreateResponseType response = driver.ParseFinalPage(out orderId);
                            response.btc_amount         = amountBTC.ToString();
                            response.email              = KonvertImDriver.TICKET_URL.Replace("{order}", orderId);
                            response.ip                 = proxy == null ? "local" : proxy.ip;

                            if (!response.IsValid())
                                throw new Exception();

                            Console.Write(response.toJson());                            
                            return;
                        }
                        catch (Exception) { }
                    }
                    break;
            }
        }
    }
}
