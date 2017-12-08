using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Skyresponse.HttpWrappers
{
    public static class JsonHelper
    {
        public static async Task<T> GetJsonObject<T>(this HttpResponseMessage httpResponse, T definition)
        {
            var content = await httpResponse.Content.ReadAsStringAsync();
            var jsonObject = GetJsonObject(content, definition);
            return jsonObject;
        }

        public static T GetJsonObject<T>(this string content, T definition)
        {
            var json = JsonConvert.DeserializeAnonymousType(content, definition);
            return json;
        }
    }
}
