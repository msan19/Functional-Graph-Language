using Newtonsoft.Json;

namespace FileGeneratorLib
{
    [JsonObject]
    public class Settings
    {
        [JsonProperty("outputFolderName")]
        public string OutputFolderName {get;set;}
        
        [JsonProperty("gmlFileExtension")]
        public string GmlFileExtension {get;set;}

        [JsonProperty("dotFileExtension")]
        public string DotFileExtension { get; set; }
    }
}