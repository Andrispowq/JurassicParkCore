using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace JurassicPark.Core.OldModel.Connection
{
    internal class Connection
    {
        private readonly string _ip = "localhost";
        private readonly int _port = 5173;

        private readonly HttpClient _client;

        public Connection()
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri($"http://{_ip}:{_port}");
            _client.Timeout = TimeSpan.FromSeconds(1);
        }

        /// <summary>
        /// This method executes a request to a List of T type
        /// </summary>
        /// <param name="request">The request to be sent</param>
        /// <typeparam name="T">The type to be used as the List's generic (reference type)</typeparam>
        /// <returns>An empty list when the request fails, or the result</returns>
        public async Task<List<T>> RequestList<T>(IHttpRequest request) where T : class
        {
            var result = await Request<List<T>>(request);
            return result ?? new List<T>();
        }
        
        /// <summary>
        /// This method executes a request to a T type
        /// </summary>
        /// <param name="request">The request to be sent</param>
        /// <typeparam name="T">The type to be used as the List's generic (reference type)</typeparam>
        /// <returns>The result, or null if it fails</returns>
        public async Task<T?> Request<T>(IHttpRequest request) where T : class
        {
            try
            {
                var result = await request.MakeRequest(_client);

                if (!result.IsSuccess)
                {
                    var body = await request.BodyAsStringAsync();
                    if (body != "") body = $"\n\tbody: {body}";
                    Console.WriteLine(
                        $"ERROR: Request ({request.AsString}{body}) returned \n\tstatus code: {result.StatusCode}\n\tbody: {result.Content}");
                    return null;
                }

                if (typeof(T) == typeof(string))
                {
                    return result.Content as T;
                }

                return TryDeserialize<T>(result.Content);
            }
            catch (Exception e)
            {
                var body = await request.BodyAsStringAsync();
                if (body != "") body = $"\n\tbody: {body}";
                Console.WriteLine($"ERROR: Request ({request.AsString}{body}) failed with exception: {e}");
                return null;
            }
        }

        private static T? TryDeserialize<T>(string json) where T : class
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