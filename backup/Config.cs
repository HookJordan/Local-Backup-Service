using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace backup
{
    class Config
    {
        [JsonPropertyName("inputDirectories")]
        public List<string> InputDirectories { get; set; }

        [JsonPropertyName("outputDirectory")]
        public string Output { get; set; }
    }
}
