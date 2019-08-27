using Newtonsoft.Json;
using RuneAPI.Enums;

namespace RuneAPI.Containers
{
    public class Price
    {
        [JsonProperty("trend")]
        public Trend Trend { get; protected set; }
        
        [JsonProperty("price")]
        public string PriceString { get; protected set; }
    }
}
