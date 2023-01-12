using Newtonsoft.Json;
using System.Text;

namespace RuneAPI.Containers
{
    public class Item
    {
        [JsonProperty("icon")]
        public string SmallIconUrl { get; protected set; }

        [JsonProperty("icon_large")]
        public string LargeIconUrl { get; protected set; }

        [JsonProperty("id")]
        public int Id { get; protected set; }

        [JsonProperty("type")]
        public string Type { get; protected set; }

        [JsonProperty("typeIcon")]
        public string TypeIconUrl { get; protected set; }

        [JsonProperty("name")]
        public string Name { get; protected set; }

        [JsonProperty("description")]
        public string Description { get; protected set; }

        [JsonProperty("current")]
        public Price CurrentStats { get; protected set; }

        [JsonProperty("today")]
        public Price DailyStats { get; protected set; }

        [JsonProperty("members")]
        public bool MembersOnly { get; protected set; }

        [JsonProperty("day30")]
        public Price MonthlyStats { get; protected set; }

        [JsonProperty("day90")]
        public Price QuarterlyStats { get; protected set; }

        [JsonProperty("day180")]
        public Price HalfYearlyStats { get; protected set; }

        public override string ToString()
        {
            var sb = new StringBuilder();

            sb.AppendLine($"Item '{Name}' [{Description}]:");
            sb.AppendLine($"  ID: {Id}");
            sb.AppendLine($"  Type: {Type}");
            sb.AppendLine($"  URLs:");
            sb.AppendLine($"    Type icon: {TypeIconUrl}");
            sb.AppendLine($"    Small icon: {SmallIconUrl}");
            sb.AppendLine($"    Large icon: {LargeIconUrl}");
            sb.AppendLine($"  Members only: {MembersOnly}");
            sb.AppendLine($"  Statistics:");
            sb.AppendLine($"    Current:");
            sb.AppendLine($"      Price: {CurrentStats.PriceString}");
            sb.AppendLine($"      Trend: {CurrentStats.Trend}");
            sb.AppendLine($"    Daily:");
            sb.AppendLine($"      Price: {DailyStats.PriceString}");
            sb.AppendLine($"      Trend: {DailyStats.Trend}");
            sb.AppendLine($"    Monthly:");
            sb.AppendLine($"      Price: {MonthlyStats.PriceString}");
            sb.AppendLine($"      Trend: {MonthlyStats.Trend}");
            sb.AppendLine($"    Quarterly:");
            sb.AppendLine($"      Price: {QuarterlyStats.PriceString}");
            sb.AppendLine($"      Trend: {QuarterlyStats.Trend}");
            sb.AppendLine($"    Half-yearly:");
            sb.AppendLine($"      Price: {HalfYearlyStats.PriceString}");
            sb.AppendLine($"      Trend: {HalfYearlyStats.Trend}");

            return sb.ToString();
        }
    }
}
