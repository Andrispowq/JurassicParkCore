using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace JurassicPark.Core.OldModel.Connection
{
    internal interface IHttpRequest
    {
        Task<(bool, string)> MakeRequest(HttpClient client);
    }

    internal class GetRequest : IHttpRequest
    {
        private string Url { get; set; }

        public GetRequest(string url)
        {
            Url = url;
        }

        public async Task<(bool, string)> MakeRequest(HttpClient client)
        {
            var response = await client.GetAsync(Url);
            return (response.IsSuccessStatusCode, await response.Content.ReadAsStringAsync());
        }
    }

    internal class PostRequest : IHttpRequest
    {
        private string Url { get; set; }
        private HttpContent? Content { get; set; }

        public PostRequest(string url, HttpContent? content = null)
        {
            Url = url;
            Content = content;
        }

        public PostRequest(string url, object? content = null)
        {
            Url = url;

            if (content == null)
            {
                Content = null;
            }
            else
            {
                var json = JsonConvert.SerializeObject(content);
                var stringContent = new StringContent(json);
                Content = stringContent;
            }
        }

        public async Task<(bool, string)> MakeRequest(HttpClient client)
        {
            var response = await client.PostAsync(Url, Content);
            return (response.IsSuccessStatusCode, await response.Content.ReadAsStringAsync());
        }
    }

    internal class PutRequest : IHttpRequest
    {
        private string Url { get; set; }
        private HttpContent? Content { get; set; }

        public PutRequest(string url, HttpContent? content = null)
        {
            Url = url;
            Content = content;
        }

        public PutRequest(string url, object? content = null)
        {
            Url = url;

            if (content == null)
            {
                Content = null;
            }
            else
            {
                var json = JsonConvert.SerializeObject(content);
                var stringContent = new StringContent(json);
                Content = stringContent;
            }
        }

        public async Task<(bool, string)> MakeRequest(HttpClient client)
        {
            var response = await client.PutAsync(Url, Content);
            return (response.IsSuccessStatusCode, await response.Content.ReadAsStringAsync());
        }
    }

    internal class PatchRequest : IHttpRequest
    {
        private string Url { get; set; }
        private HttpContent? Content { get; set; }

        public PatchRequest(string url, HttpContent? content = null)
        {
            Url = url;
            Content = content;
        }

        public PatchRequest(string url, object? content = null)
        {
            Url = url;

            if (content == null)
            {
                Content = null;
            }
            else
            {
                var json = JsonConvert.SerializeObject(content);
                var stringContent = new StringContent(json);
                Content = stringContent;
            }
        }

        public async Task<(bool, string)> MakeRequest(HttpClient client)
        {
            var response = await client.PatchAsync(Url, Content);
            return (response.IsSuccessStatusCode, await response.Content.ReadAsStringAsync());
        }
    }
    
    internal class DeleteRequest : IHttpRequest
    {
        private string Url { get; set; }
        private HttpContent? Content { get; set; }

        public DeleteRequest(string url, HttpContent? content = null)
        {
            Url = url;
            Content = content;
        }

        public DeleteRequest(string url, object? content = null)
        {
            Url = url;

            if (content == null)
            {
                Content = null;
            }
            else
            {
                var json = JsonConvert.SerializeObject(content);
                var stringContent = new StringContent(json);
                Content = stringContent;
            }
        }

        public async Task<(bool, string)> MakeRequest(HttpClient client)
        {
            HttpResponseMessage? response;
            if (Content != null)
            {
                var deleteRequest = new HttpRequestMessage(HttpMethod.Delete, Url)
                {
                    Content = Content
                };

                response = await client.SendAsync(deleteRequest);
            }
            else
            {
                response = await client.DeleteAsync(Url);
            }

            return (response.IsSuccessStatusCode, await response.Content.ReadAsStringAsync());
        }
    }
}