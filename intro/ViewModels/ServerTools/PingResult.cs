using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace intro.ViewModels.ServerTools
{
    public class PingResult
    {
        public long TotalMs { get;set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public System.Net.NetworkInformation.IPStatus Status{ get; set; }

        public string Address{ get; set; }
    }
}