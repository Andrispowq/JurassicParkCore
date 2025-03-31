using System.Net.Http;
using System.Threading.Tasks;

namespace JurassicPark.Core.OldModel.Connection
{
    public class HttpResult
    {
        public bool IsSuccess { get; set; }
        public int StatusCode { get; set; }
        public string Content { get; set; }

        public static async Task<HttpResult> From(HttpResponseMessage message)
        {
            return new HttpResult()
            {
                IsSuccess = message.IsSuccessStatusCode,
                StatusCode = (int)message.StatusCode,
                Content = await message.Content.ReadAsStringAsync()
            };
        }
    }
}