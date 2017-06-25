using Mars.Server.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mars.Server.App.Core
{
    public class AppServerDataInitializer
    {
        public static void Init()
        {
            //AppClients = new DAO_Common<AppClientsEntity>().QueryList().ToDictionary(e => e.AppKey);
            AppClients.Add("Mars.Mobile", new AppClientsEntity() { AppKey = "Mars.Mobile", AppSecrect = "41578020073147cdb707f99dbb6eeb46" });
            AppClients.Add("Mars.Mobile.iOS", new AppClientsEntity() { AppKey = "Mars.Mobile.iOS", AppSecrect = "88A63725299A4B4EB6B11039B3A4F265" });
        }


        public static Dictionary<string, AppClientsEntity> AppClients = new Dictionary<string, AppClientsEntity>();

    }
}
