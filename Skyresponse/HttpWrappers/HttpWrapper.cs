using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Skyresponse.HttpWrappers
{
    public interface IHttpWrapper
    {
        /// <summary>
        /// Returns an accesstoken
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        Task<string> GetAccessToken(Dictionary<string, string> dict);

        /// <summary>
        /// Register for push messages
        /// </summary>
        /// <param name="accesstoken"></param>
        /// <returns></returns>
        Task RegisterForPush(string accesstoken);

        /// <summary>
        /// Get info on the alarm
        /// </summary>
        /// <param name="alarmId"></param>
        /// <param name="accesstoken"></param>
        /// <returns></returns>
        Task<int> GetAlarmInfo(string alarmId, string accesstoken);
    }

    public class HttpWrapper : IHttpWrapper
    {
        private static readonly HttpClient Client = new HttpClient();
        private static readonly string BaseUrl = ConfigurationManager.AppSettings["BaseUrl"];
        private static readonly string TokenUrl = ConfigurationManager.AppSettings["TokenUrl"];
        private static readonly string RegisterUrl = ConfigurationManager.AppSettings["RegisterUrl"];
        private static readonly string AlarmUrl = ConfigurationManager.AppSettings["AlarmUrl"];
        private const string JsonContentType = "application/json";
        private const string Json = @"{ 'type':'SkygdWebApp','telephone':'+999999999','devicetoken':'do','UsingTwilio':false }";
        private string _url;

        /// <inheritdoc />
        /// <summary>
        /// Returns an accesstoken
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        public async Task<string> GetAccessToken(Dictionary<string, string> dict)
        {
            _url = string.Concat(BaseUrl, TokenUrl);
            var request = new HttpRequestMessage(HttpMethod.Post, _url) { Content = new FormUrlEncodedContent(dict) };
            var response = await Client.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                throw new UnauthorizedAccessException();

            var definition = new { access_token = "", token_type = "", expires_in = "" };
            var jsonObject = await response.GetJsonObject(definition);
            return await Task.FromResult(jsonObject.access_token);
        }

        /// <inheritdoc />
        /// <summary>
        /// Register for push messages
        /// </summary>
        /// <param name="accesstoken"></param>
        /// <returns></returns>
        public async Task RegisterForPush(string accesstoken)
        {
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", accesstoken);
            _url = string.Concat(BaseUrl, RegisterUrl);
            var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, _url) { Content = new StringContent(Json, Encoding.UTF8, JsonContentType) };
            var response = await Client.SendAsync(httpRequestMessage);

            if (!response.IsSuccessStatusCode)
                throw new HttpRequestException();
        }

        /// <inheritdoc />
        /// <summary>
        /// Get info on the alarm
        /// </summary>
        /// <param name="alarmId"></param>
        /// <param name="accesstoken"></param>
        /// <returns></returns>
        public async Task<int> GetAlarmInfo(string alarmId, string accesstoken)
        {
            var urlEnding = string.Concat(alarmId, "/basic");
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", accesstoken);
            _url = string.Concat(BaseUrl, AlarmUrl, urlEnding);
            var response = await Client.GetAsync(_url);
            var definition = new { Level = 0 };
            var jsonObject = await response.GetJsonObject(definition);
            return await Task.FromResult(jsonObject.Level);
        }
    }
}
