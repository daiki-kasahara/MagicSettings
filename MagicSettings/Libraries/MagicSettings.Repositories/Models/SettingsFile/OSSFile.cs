﻿using System.Text.Json.Serialization;

namespace MagicSettings.Repositories.Models.SettingsFile
{
    public class OSSFile
    {
        public List<OSSProperty> OSS { get; set; } = [];
    }

    public class OSSProperty
    {
        public string Name { get; set; } = string.Empty;
        public string Path { get; set; } = string.Empty;
        [JsonIgnore]
        public string Content { get; set; } = string.Empty;
    }
}
