using RestSharp;
using System;
using System.Diagnostics;

namespace RuneAPI.Network
{
    public class API
    {
        private RestClient Client { get; }
        public Uri BaseURI { get; }

        private static API _instance;
        public static API Instance
        {
            get => _instance ?? (_instance = new API("http://services.runescape.com/m=itemdb_oldschool/api/"));
        }

        private API(string baseUri)
        {
            BaseURI = new Uri(baseUri);
            Client = new RestClient(BaseURI);
        }

        public string GetRawResponse(RestRequest req)
        {
            var response = string.Empty;

            try
            {
                response = Client.Execute(req).Content;
            }
            catch(Exception e)
            {
                Debug.WriteLine(e);
            }

            return response;
        }

        public IRestResponse GetResponse(RestRequest req)
        {
            IRestResponse response;

            try
            {
                response = Client.Execute(req);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
                return null;
            }

            return response;
        }

        public string GetMethod(string method) => $"{new Uri(BaseURI, method).AbsoluteUri}";
    }
}
