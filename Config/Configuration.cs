using System;
using System.Collections.Generic;
using System.Reflection;


namespace Hw_14_try.Config
{
    public class ConfigurationBuilder
    {
        private static readonly Lazy<ConfigurationBuilder> _instance = new Lazy<ConfigurationBuilder>(() => new ConfigurationBuilder());

        public static ConfigurationBuilder Instance => _instance.Value;
        private readonly List<ConfigurationModel> _configurationModels = new List<ConfigurationModel>();

        public ConfigurationBuilder SetConfig<TConfig>(IConfig config)
        {
            var type = typeof(TConfig);
            var instance = type.GetProperty("Instance").GetValue(null);
            var prop = type.GetField("_config", BindingFlags.NonPublic | BindingFlags.Instance);
            _configurationModels.Add(new ConfigurationModel { FieldInfo = prop, Instance = instance, Config = config });
            return this;
        }

        public void Build()
        {
            foreach (var item in _configurationModels)
            {
                item.FieldInfo.SetValue(item.Instance, item.Config);
            }
        }
    }
}
