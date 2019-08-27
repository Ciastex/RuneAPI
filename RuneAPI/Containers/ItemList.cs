using Newtonsoft.Json;
using System.Collections.Generic;

namespace RuneAPI.Containers
{
    public class ItemList
    {
        [JsonProperty("total")]
        public int TotalCount { get; protected set; }

        [JsonProperty("items")]
        public List<Item> Items { get; protected set; }

        public ItemList()
        {
            Items = new List<Item>();
        }
    }
}
