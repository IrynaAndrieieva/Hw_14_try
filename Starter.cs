using Hw_14_try.Config;
using Hw_14_try.Services;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Xml.Serialization;
using Newtonsoft.Json;

namespace Hw_14_try
{
    public class Starter
    {
        private readonly ConfigurationBuilder _configurationBuilder = ConfigurationBuilder.Instance;
        private readonly Logger _logger = Logger.Instance;

        private readonly FileService _fileService = FileService.Instance;

        private readonly LoggerConfig _loggerConfig;

        public Starter()
        {
            var configFile = File.ReadAllText("config.json");
            var config = JsonConvert.DeserializeObject<Config>(configFile);
            _loggerConfig = config.Logger;
            _loggerConfig.FileName = string.Format(_loggerConfig.FileName, DateTime.UtcNow.ToLongTimeString().ToString());

            Configuration();
            LoggerHandler();
        }

        private void Configuration()
        {
            _configurationBuilder
                .SetConfig<Logger>(_loggerConfig)
                .Build();

            Presetup();
        }

        private void Presetup()
        {
            if (!Directory.Exists(_loggerConfig.DirectoryPath))
            {
                Directory.CreateDirectory(_loggerConfig.DirectoryPath);
            }
            if (Directory.Exists(_loggerConfig.BackUpDirectoryPath))
            {
                Directory.CreateDirectory(_loggerConfig.BackUpDirectoryPath);
            }
        }

        private void LoggerHandler()
        {

            _logger.LogCountHandler += count =>
            {
                if (count % 10 == 0)
                {
                    return Task.FromResult($"------------------------------- {count} Line(s) -------------------------------");
                }
                else
                {
                    return Task.FromResult(string.Empty);
                }
            };

            _logger.LogCountHandler += async count =>
            {
                var additionalLines = count / _loggerConfig.LineSeparator;
                var path = $"{_loggerConfig.DirectoryPath}{_loggerConfig.FileName}{_loggerConfig.FileExtention}";
                var lines = await _fileService.ReadLinesAsync(path, count + additionalLines);



                var pathBackup = $"{_loggerConfig.BackUpDirectoryPath}{$"backup_from_{DateTime.UtcNow.ToLongTimeString()}_for_{_loggerConfig.FileName}"}{_loggerConfig.FileExtention}";
                await _fileService.WriteToFileAsync(pathBackup, string.Join(Environment.NewLine, lines));

                return null;
            };
        }

        private void SerializationSample()
        {
            var configFile = File.ReadAllText("config.json");
            var config = JsonConvert.DeserializeObject<Config>(configFile);

            XmlSerializer formatter = new XmlSerializer(typeof(Config));

            using (FileStream fs = new FileStream("config.xml", FileMode.OpenOrCreate))
            {
                formatter.Serialize(fs, config);
            }

            using (FileStream fs = new FileStream("config.xml", FileMode.OpenOrCreate))
            {
                Config xmlConfig = (Config)formatter.Deserialize(fs);
            }
        }
    }
}
