using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.WebSockets;
using System.Threading.Tasks;
using Skyresponse.Services.Sound;
using Skyresponse.Services.User;
using Skyresponse.Wrappers.HttpWrappers;

namespace Skyresponse.Api
{
    public class SkyresponseApi : ISkyresponseApi
    {
        private readonly IHttpWrapper _httpRequest;
        private readonly IWebSocketWrapper _webSocket;
        private readonly ISoundService _soundService;
        private readonly IUserService _userService;
        private readonly List<string> _alreadyPlayed;
        private static readonly string WebSocketUrl = ConfigurationManager.AppSettings["WebSocketUrl"];
        private string _accesstoken;

        public SkyresponseApi(IHttpWrapper httpRequest, IWebSocketWrapper webSocket, ISoundService soundService, IUserService userService)
        {
            _httpRequest = httpRequest;
            _webSocket = webSocket;
            _soundService = soundService;
            _userService = userService;
            _alreadyPlayed = new List<string>();
        }

        /// <summary>
        /// Init method
        /// </summary>
        /// <returns></returns>
        public async Task InitAsync()
        {
            await Login();
        }

        /// <summary>
        /// Login to SkyResponse API
        /// </summary>
        /// <returns></returns>
        public async Task Login()
        {
            _accesstoken = await _userService.GetAccessToken();

            if (!string.IsNullOrEmpty(_accesstoken))
            {
                _userService.SaveUserInfo();

                try
                {
                    await _httpRequest.RegisterForPush(_accesstoken);
                }
                catch (HttpRequestException)
                {
                    ReConnect();
                    return;
                }

                try
                {
                    var webSocketUrl = string.Concat(WebSocketUrl, _accesstoken);
                    _webSocket.Connect(webSocketUrl);
                }
                catch (WebSocketException)
                {
                    ReConnect();
                    return;
                }

                _webSocket.OnMessage(OnMessageAsync);
                _webSocket.OnDisconnect(OnDisconnect);
            }
        }

        /// <summary>
        /// Try to reconnect
        /// </summary>
        /// <param name="webSocketWrapper"></param>
        private void OnDisconnect(WebSocketWrapper webSocketWrapper)
        {
            ReConnect();
        }

        /// <summary>
        /// Calls OnTimeUp when timer runs out
        /// </summary>
        private async void ReConnect()
        {
            await InitAsync();
        }

        /// <summary>
        /// Play sound when recieving message with active alarm
        /// </summary>
        /// <param name="message"></param>
        /// <param name="socketWrapper"></param>
        private async void OnMessageAsync(string message, WebSocketWrapper socketWrapper)
        {
            var webSocketDefinition = new { globalAlarmId = "", active = true }; //active = true is active alarm
            var webSocketMessage = message.GetJsonObject(webSocketDefinition);
            if (!_alreadyPlayed.Contains(webSocketMessage.globalAlarmId))
            {
                _alreadyPlayed.Add(webSocketMessage.globalAlarmId);
                var alarmInfo = await _httpRequest.GetAlarmInfo(webSocketMessage.globalAlarmId, _accesstoken);
                if (webSocketMessage.active && alarmInfo > 0)
                {
                    _soundService.PlaySound();
                }
            }
        }
    }
}
