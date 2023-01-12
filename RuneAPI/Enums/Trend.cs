using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace RuneAPI.Enums
{
    [JsonConverter(typeof(StringEnumConverter))]
    public enum Trend
    {
        Neutral,
        Positive,
        Negative
    }
}
