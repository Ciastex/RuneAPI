using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using RuneAPI.Containers;
using RuneAPI.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace RuneAPI
{
    public class GrandExchange
    {
        public Dictionary<string, int> ItemDb { get; }

        public GrandExchange()
        {
            ItemDb = new Dictionary<string, int>();
        }

        public GrandExchange(string itemDbJson)
        {
            ItemDb = JsonConvert.DeserializeObject<Dictionary<string, int>>(itemDbJson);

            if (ItemDb == null)
                throw new ArgumentException("Invalid item database JSON provided.");
        }

        public Dictionary<char, int> GetAlphabeticalCatalogue()
        {
            var request = new RestRequest("catalogue/category.json?category=1");
            var response = API.Instance.GetRawResponse(request);

            var jo = JsonConvert.DeserializeObject<JObject>(response);

            var dict = new Dictionary<char, int>();
            foreach (var token in (jo["alpha"] as JArray))
            {
                dict.Add(token.Value<char>("letter"), token.Value<int>("items"));
            }

            return dict;
        }

        public ItemList GetCataloguePage(char letter, int page)
        {
            var request = new RestRequest("catalogue/items.json?category=1&alpha={alpha}&page={page}");
            request.AddUrlSegment("alpha", letter);
            request.AddUrlSegment("page", page);
            var response = API.Instance.GetResponse(request);
            var content = response.Content;

            var itemList = JsonConvert.DeserializeObject<ItemList>(content);

            return itemList;
        }

        public Item GetItemDetails(int id)
        {
            var request = new RestRequest("catalogue/detail.json?item={id}");
            request.AddUrlSegment("id", id);

            var response = API.Instance.GetResponse(request);

            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            var content = response.Content;

            var tok = (JsonConvert.DeserializeObject(content) as JToken);
            var item = JsonConvert.DeserializeObject<Item>(tok["item"].ToString());

            return item;
        }

        public Item GetItemDetails(string name)
        {
            if (!ItemDb.ContainsKey(name))
                throw new KeyNotFoundException("The provided name does not exist in the item database.");

            return GetItemDetails(ItemDb[name]);
        }

        public Dictionary<string, int> GetGraphData(int itemId)
        {
            var request = new RestRequest("graph/{id}.json");
            request.AddUrlSegment("id", itemId);

            var response = API.Instance.GetResponse(request);

            if (response.StatusCode == HttpStatusCode.NotFound)
                return null;

            var content = response.Content;

            var tok = (JsonConvert.DeserializeObject(content) as JToken);
            var item = JsonConvert.DeserializeObject<Dictionary<string, int>>(tok["daily"].ToString());

            return item;
        }

        public Dictionary<string, int> GetGraphData(string name)
        {
            if (!ItemDb.ContainsKey(name))
                throw new KeyNotFoundException("The provided name does not exist in the item database.");

            return GetGraphData(ItemDb[name]);
        }

        public int GetLatestPrice(int itemId)
        {
            var graphData = GetGraphData(itemId);

            if (graphData != null)
                return graphData.Last().Value;

            return -1;
        }

        public int GetLatestPrice(string name)
        {
            if (!ItemDb.ContainsKey(name))
                throw new KeyNotFoundException("The provided name does not exist in the item database.");

            return GetLatestPrice(ItemDb[name]);
        }
    }
}
