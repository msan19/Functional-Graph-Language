using Newtonsoft.Json;

namespace FileUtilities
{
    [JsonObject]
    public class Settings
    {
        [JsonProperty("inputFolderName")]
        public string InputFolderName {get;set;}
    }
}