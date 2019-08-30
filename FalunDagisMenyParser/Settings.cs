using System;

namespace FalunDagisMenyParser
{
    public class Settings
    {
        public string Url { get; set; }
        public string StorageConnectionString { get; internal set; }

        public string GenerateUrl(int weekOffset) => Url + $"&w={weekOffset}";
    }
}
