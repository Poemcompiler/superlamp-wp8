using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Superlamp.Services
{
    public class DataService : IDataService
    {
        private const string ServiceUrl
          = "http://www.galasoft.ch/labs/json/JsonDemo.ashx";
        private readonly HttpClient _client;
        public DataService()
        {
            _client = new HttpClient();
        }
        public async Task<int> AddLocation()
        {
            var request = new HttpRequestMessage(
              HttpMethod.Post,
              new Uri(ServiceUrl));
            var response = await _client.SendAsync(request);
            var result = await response.Content.ReadAsStringAsync();
            //var serializer = new JsonSerializer();
            //var deserialized = serializer.Deserialize<int>(result);
            //return deserialized.Friends;
            return 1;
        }
    }
}
