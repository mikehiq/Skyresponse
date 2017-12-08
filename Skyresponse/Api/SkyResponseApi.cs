using System;
using System.Collections.Generic;
using System.Configuration;
using System.Threading.Tasks;
using System.Windows.Forms;
using Skyresponse.Forms;
using Skyresponse.HttpWrappers;
using Skyresponse.Persistence;
using Skyresponse.Services;

namespace Skyresponse.Api
{
    public interface ISkyresponseApi
    {
        Task InitAsync();
        bool IsLoggedIn { get; set; }
    }

    public class SkyresponseApi : ISkyresponseApi
    {
        private readonly IPersistenceManager _persistenceManager;
        private readonly IHttpWrapper _httpRequest;
        private readonly IWebSocketWrapper _webSocket;
        private readonly ISoundService _soundService;
        private readonly ILoginForm _loginForm;
        private readonly Timer _timer;
        private readonly List<string> _alreadyPlayed;
        private const string UsernameKey = "UserName";
        private const string PasswordKey = "Password";
        private static readonly string WebSocketUrl = ConfigurationManager.AppSettings["WebSocketUrl"];
        private string _accesstoken;

        public bool IsLoggedIn { get; set; }

        public SkyresponseApi(IPersistenceManager persistenceManager, ILoginForm loginForm, IHttpWrapper httpRequest, IWebSocketWrapper webSocket, ISoundService soundService)
        {
            _persistenceManager = persistenceManager;
            _loginForm = loginForm;
            _httpRequest = httpRequest;
            _webSocket = webSocket;
            _soundService = soundService;
            _alreadyPlayed = new List<string>();
            _timer = new Timer { Interval = 10000 };
            _timer.Tick += OnTimeUp;
        }

        /// <summary>
        /// Init method
        /// </summary>
        /// <returns></returns>
        public async Task InitAsync()
        {
            var username = _persistenceManager.Read(UsernameKey);
            var password = _persistenceManager.ReadSecure(PasswordKey);

            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                var result = _loginForm.ShowDialog();
                if (result.Equals(DialogResult.OK))
                {
                    username = _loginForm.UserName;
                    password = _loginForm.Password;
                }
            }
            await Login(username, password);
        }

        /// <summary>
        /// Login to SkyResponse API and get access token
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private async Task Login(string username, string password)
        {
            var dict = new Dictionary<string, string>
            {
                {UsernameKey, username},
                {PasswordKey, password},
                {"grant_type", "password"}
            };
            var accessToken = await _httpRequest.GetAccessToken(dict);
            if (!string.IsNullOrEmpty(accessToken))
            {
                IsLoggedIn = true;
                _accesstoken = accessToken;
                _persistenceManager.Save(UsernameKey, username);
                _persistenceManager.SaveSecure(PasswordKey, password);

                await _httpRequest.RegisterForPush(_accesstoken);

                var webSocketUrl = string.Concat(WebSocketUrl, _accesstoken);
                _webSocket.Connect(webSocketUrl);
                _webSocket.OnMessage(OnMessageAsync);
                _webSocket.OnDisconnect(OnDisconnect);
                _timer.Stop();
            }
        }

        private void OnDisconnect(WebSocketWrapper webSocketWrapper)
        {
            _timer.Start();
        }

        private async void OnTimeUp(object sender, EventArgs e)
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
                if (webSocketMessage.active)
                {
                    switch (alarmInfo)
                    {
                        case 0:
                            //spela inget ljud
                            break;
                        case 1:
                            //spela level1.mp3
                            break;
                        case 2:
                            //spela level2.mp3
                            break;
                    }
                    _soundService.PlaySound();
                }
            }
        }
    }
}
