using System;


namespace Hw_14_try.Config
{
    public class LoggerConfig : IConfig
    {
        public int LineSeparator { get; set; }
        public string TimeFormat { get; set; }
        public string DirectoryPath { get; set; }
        public string BackUpDirectoryPath { get; set; }
        public string FileName { get; set; }
        public string FileExtention { get; set; }
    }
}
