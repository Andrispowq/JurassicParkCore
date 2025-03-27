using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace JurassicPark.Core.OldModel.Connection
{
    internal class Connection
    {
        private readonly string _ip = "localhost";
        private readonly int _port = 7228;

        private readonly HttpClient _client;

        public Connection()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri($"https://{_ip}:{_port}");
        }
        
        public async Task<T?> Request<T>(IHttpRequest request) where T : class
        {
            (bool, string) result = await request.MakeRequest(_client);

            if (!result.Item1)
            {
                Console.WriteLine(result.Item2);
                return null;
            }

            if (typeof(T) == typeof(string))
            {
                return result.Item2 as T;
            }

            return TryDeserialize<T>(result.Item2);
        }

        private T? TryDeserialize<T>(string json) where T : class
        {
            try
            {
                JsonReader reader = new JsonTextReader(new System.IO.StringReader(json));
                JsonSerializer serializer = new JsonSerializer();
                return serializer.Deserialize<T>(reader);
            }
            catch
            {
                return null;
            }
        }
    }
}