using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using Newtonsoft.Json;

namespace HappyDarioBot
{
    public static class DarioBotConfiguration
    {
        public const string BotTokenKey = "BotToken";
        public const string ForwardToIdKey = "ForwardToId";
        public const string ResourcesPathKey = "ResourcesPath";
        public const string RemoteResourcesPathKey = "RemoteResourcesPath";
        public const string StorageConnectionStringKey = "WEBSITE_CONTENTAZUREFILECONNECTIONSTRING";

        public static string Get(string key) => 
            ConfigurationManager.AppSettings[key] 
            ?? Environment.GetEnvironmentVariable(key) 
            ?? ReadFromLocal(key);

        private static string ReadFromLocal(string key)
        {
            var jsonContent = File.ReadAllText("local.settings.json");
            dynamic settings = JsonConvert.DeserializeObject(jsonContent);
            return settings.Values[key].Value;
        }
    }
}