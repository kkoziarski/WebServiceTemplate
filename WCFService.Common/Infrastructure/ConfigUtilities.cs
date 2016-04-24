namespace WCFService.Common.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;

    public class ConfigUtilities
    {
        public static string GetConnectionString(string configFilePath, string connectionStringName)
        {
            XmlDocument configDoc = new XmlDocument();
            configDoc.Load(configFilePath);
            
            XmlNode connectionStringsNode = configDoc.SelectSingleNode("/configuration/connectionStrings");
            XmlAttribute configSourceAttr = connectionStringsNode.Attributes["configSource"];
            XmlNode connectionStringNode = null;

            if (configSourceAttr != null && !String.IsNullOrEmpty(configSourceAttr.Value))
            {
                string connectionStringsFilePath = configFilePath.Substring(0, configFilePath.LastIndexOf('\\') + 1) + configSourceAttr.Value;
                XmlDocument connectionStringsDoc = new XmlDocument();
                connectionStringsDoc.Load(connectionStringsFilePath);

                connectionStringNode = connectionStringsDoc.SelectSingleNode("/connectionStrings/add[@name='" + connectionStringName + "']");
            }
            else
            {
                connectionStringNode = connectionStringsNode.SelectSingleNode("/connectionStrings/add[@name='" + connectionStringName + "']");
            }

            return connectionStringNode.Attributes["connectionString"].Value;
        }

        /// <summary>
        /// "key1=value1;key2=value2;key3=value3;" string to dictiory
        /// </summary>
        /// <param name="dictionaryString">"key1=value1;key2=value2;key3=value3;"</param>
        /// <returns>Dictionary<string, string></returns>
        public static Dictionary<string, string> StringToDictionary(string dictionaryString)
        {
            var dict = dictionaryString.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries)
                                       .Select(part => part.Split('='))
                                       .Where(part => part.Length == 2)
                                       .ToDictionary(split => split[0], split => split[1]);

            return dict;
        }
    }
}
