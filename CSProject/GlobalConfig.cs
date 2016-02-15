using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Reflection;

namespace CSProject
{
    class GlobalConfig
    {
        public static string ClientId { get; private set; }
        public static string DevToken { get; private set; }
        public static long CustomerId { get; private set; }
        public static long AccountId { get; private set; }
        public static string OAuthDesktopMobileImplicitGrantUrl { get; private set; }

        public static bool Initialize()
        {
            var appSettings = ConfigurationManager.AppSettings;

            //debug
            CommonHelper.OutputSuccessMessage("Read Config K/V Pairs from AppSettings...");
            foreach (var key in appSettings.AllKeys)
            {
                CommonHelper.OutputMessage(string.Format("Key: {0}, Value: {1}", key, appSettings[key]));
            }

            ClientId = appSettings["ClientId"];
            DevToken = appSettings["DevToken"];
            CustomerId = long.Parse(appSettings["CustomerId"]);
            AccountId = long.Parse(appSettings["AccountId"]);

            OutputAllProperty();

            return true;
        }

        //using reflection to get all properties
        public static void OutputAllProperty()
        {
            CommonHelper.OutputSuccessMessage("Output All Properties...");

            Type globalConfigType = typeof(GlobalConfig);
            foreach (PropertyInfo property in globalConfigType.GetProperties())
            {
                object value = property.GetValue(globalConfigType);
                string name = property.Name;
                CommonHelper.OutputMessage(string.Format("Reflection: {0} = {1}", name, value == null ? "null" : value.ToString()));
            }
        }
    }
}
