namespace WCFService.Infrastructure
{
    using System.Configuration;
    using System.Web.Configuration;

    public class AppConfig
    {
        #region Database

        public static string DatabaseConnectionString
        {
            get
            {
                //Azure obsolete

                //if (RoleEnvironment.IsAvailable)
                //{
                //    string value = RoleEnvironment.GetConfigurationSettingValue("DatabaseConnectionString");
                //    if (value.Equals("UseDevelopmentConnectionString=true"))
                //    {
                //        if (WebConfigurationManager.ConnectionStrings["DataModel"] != null)
                //        {
                //            return WebConfigurationManager.ConnectionStrings["DataModel"].ConnectionString;
                //        }
                //        else // web.config has not been loaded yet
                //        {
                //            return ConfigUtilities.GetConnectionString(System.Environment.CurrentDirectory + "\\Web.config", "DataModel");
                //        }
                //    }
                //    else
                //    {
                //        return value;
                //    }
                //}
                //else
                //{
                //    return WebConfigurationManager.ConnectionStrings["DataModel"].ConnectionString;
                //}

                return WebConfigurationManager.ConnectionStrings["DataModel"].ConnectionString;

            }
        }

        //public static string ELMAHConnectionString
        //{
        //    get
        //    {
        //        //return @"data source=AFSKRIS-PC\SQLEXPRESS2013;database=BalfourBeattyRail;integrated security=true;";
        //        return AppConfig.ExtractConnectionStringFromEntityFramework(DatabaseConnectionString);
        //    }
        //}

        #endregion

        #region Time Zone
        public static string TimeZoneId
        {
            get
            {
                return getApplicationSetting("TimeZoneId");
            }
        }

        #endregion

        
        #region Helper Methods

        private static string getApplicationSetting(string settingName)
        {
            // Azure option - obsolete

            //if (RoleEnvironment.IsAvailable)
            //{
            //    return RoleEnvironment.GetConfigurationSettingValue(settingName);
            //}
            //else
            //{
            //    return ConfigurationManager.AppSettings[settingName];
            //}

            return ConfigurationManager.AppSettings[settingName];
        }

        //public static string ExtractConnectionStringFromEntityFramework(string connectionString)
        //{
        //    using (var ec = new System.Data.EntityClient.EntityConnection(connectionString))
        //    {
        //        return ec.StoreConnection.ConnectionString;
        //    }
        //}
        #endregion
    }
}