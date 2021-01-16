using System;
using System.Reflection;


namespace Hw_14_try.Config
{
    public class ConfigurationModel
    {
        public FieldInfo FieldInfo { get; set; }
        public object Instance { get; set; }
        public IConfig Config { get; set; }
    }
}
