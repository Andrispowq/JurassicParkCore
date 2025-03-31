using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace JurassicPark.Core.OldModel.Connection
{
    internal interface IHttpRequest
    {
        Task<HttpResult> MakeRequest(HttpClient client);
        string AsString { get; }
        Task<string> BodyAsStringAsync();
    }

    internal class GetRequest : IHttpRequest
    {
        private string Url { get; set; }
        public string AsString => $"GET {Url}";

        public GetRequest(string url)
        {
            Url = url;
        }

        public async Task<string> BodyAsStringAsync()
        {
            await Task.CompletedTask;
            return "";
        }

        public async Task<HttpResult> MakeRequest(HttpClient client)
        {
            var response = await client.GetAsync(Url);
            return await HttpResult.From(response);
        }
    }

    internal class PostRequest : IHttpRequest
    {
        private string Url { get; set; }
        private HttpContent? Content { get; set; }
        public string AsString => $"POST {Url}";

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
                Content = new StringContent(json, Encoding.UTF8, "application/json");
            }
        }

        public async Task<string> BodyAsStringAsync()
        {
            if (Content is null) return "";
            return await Content.ReadAsStringAsync();
        }

        public async Task<HttpResult> MakeRequest(HttpClient client)
        {
            var response = await client.PostAsync(Url, Content);
            return await HttpResult.From(response);
        }
    }

    internal class PutRequest : IHttpRequest
    {
        private string Url { get; set; }
        private HttpContent? Content { get; set; }
        public string AsString => $"PUT {Url}";

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
                Content = new StringContent(json, Encoding.UTF8, "application/json");
            }
        }

        public async Task<string> BodyAsStringAsync()
        {
            if (Content is null) return "";
            return await Content.ReadAsStringAsync();
        }

        public async Task<HttpResult> MakeRequest(HttpClient client)
        {
            var response = await client.PutAsync(Url, Content);
            return await HttpResult.From(response);
        }
    }

    internal class PatchRequest : IHttpRequest
    {
        private string Url { get; set; }
        private HttpContent? Content { get; set; }
        public string AsString => $"PATCH {Url}";

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
                Content = new StringContent(json, Encoding.UTF8, "application/json");
            }
        }

        public async Task<string> BodyAsStringAsync()
        {
            if (Content is null) return "";
            return await Content.ReadAsStringAsync();
        }

        public async Task<HttpResult> MakeRequest(HttpClient client)
        {
            var response = await client.PatchAsync(Url, Content);
            return await HttpResult.From(response);
        }
    }
    
    internal class DeleteRequest : IHttpRequest
    {
        private string Url { get; set; }
        private HttpContent? Content { get; set; }
        public string AsString => $"DELETE {Url}";

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
                Content = new StringContent(json, Encoding.UTF8, "application/json");
            }
        }

        public async Task<string> BodyAsStringAsync()
        {
            if (Content is null) return "";
            return await Content.ReadAsStringAsync();
        }

        public async Task<HttpResult> MakeRequest(HttpClient client)
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

            return await HttpResult.From(response);
        }
    }
}