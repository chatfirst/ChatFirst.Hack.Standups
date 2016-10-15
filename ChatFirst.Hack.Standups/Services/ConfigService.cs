namespace ChatFirst.Hack.Standups.Services
{
    using System;
    using System.Configuration;

    public class ConfigService
    {
        public static string Get(string settingName)
        {
            try
            {
                return ConfigurationManager.AppSettings.Get(settingName);
            }
            catch (Exception ex)
            {
                //throw new ArgumentException("Missing webconfig setting::" + settingName, ex);
                return null;
            }

        }

        public static V Get<V>(string settingName, V valueIfNull)
        {
            try
            {
                object val = ConfigurationManager.AppSettings.Get(settingName);
                return (V)Convert.ChangeType(val, typeof(V));

            }
            catch
            {
                return valueIfNull;
            }
        }
    }
}